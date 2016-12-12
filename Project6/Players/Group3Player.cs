using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project6
{
    public class Group3Player : Player
    {
        private double[,] opponentGrid;


        public Group3Player(String name) :
            base(name)
        {

        }

        /// <summary>
        /// Gets the player ready to play a new game.  Since the
        /// player can be reused for mutliple games this method
        /// must initialize all needed data structures.
        /// </summary>
        /// <param name="game">Game for the player to play.</param>
        public override void StartGame(BattleShipGame game)
        {
            base.StartGame(game);
            opponentGrid = new double[Game.GridSize, Game.GridSize];

            for (int i = 0; i < Game.GridSize; i++)
            {
                for (int j = 0; j < Game.GridSize; j++)
                {
                    if (i <= (Game.GridSize / 6) && j <= (Game.GridSize / 6))
                    {
                        opponentGrid[i, j] = 1;
                    }
                    else if (i <= (Game.GridSize / 4) && j <= (Game.GridSize / 4))
                    {
                        opponentGrid[i, j] = 1.25;
                    }
                    else if (i <= (Game.GridSize / 2) && j <= (Game.GridSize / 2))
                    {
                        opponentGrid[i, j] = 1.5;
                    }
                    else
                    {
                        opponentGrid[i, j] = 1.75;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the next Position to attack from the target list.
        /// Check to make sure that the Position hasn't already been
        /// played due to logic in Hit() processing.
        /// </summary>
        /// <returns>Position to attack for the turn.</returns>
        public override Position Attack()
        {
            Position p;
            
            AttackGridReset();
            SurroundingHitCell();
            MissedAttacks();

            p = nextProbable();

            return p;
        }

        /// <summary>
        /// Notifies the player that the Position was a hit.  To
        /// optimize the chances of future hits we should have
        /// a strategy for trying neighboring Positions when
        /// Attack() is called next.
        /// </summary>
        /// <param name="p">Hit position</param>
        public override void Hit(Position p)
        {
            if (!Game.ShipSunkAt(p))
            {
                opponentGrid[p.Row, p.Column] = -1;
            }
            else
            {
                opponentGrid[p.Row, p.Column] = 0;
            }
        }

        //Resets Grid so that the probability doesn't get messed up
        public void AttackGridReset()
        {
            for (int i = 0; i < Game.GridSize; i++)
            {
                for (int j = 0; j < Game.GridSize; j++)
                {
                    if (i <= (Game.GridSize / 6) && j <= (Game.GridSize / 6) && opponentGrid[i, j] > 1)
                    {
                        opponentGrid[i, j] = 1;
                    }
                    else if (i <= (Game.GridSize / 4) && j <= (Game.GridSize / 4) && opponentGrid[i, j] > 1)
                    {
                        opponentGrid[i, j] = 1.25;
                    }
                    else if (i <= (Game.GridSize / 2) && j <= (Game.GridSize / 2) && opponentGrid[i, j] > 1)
                    {
                        opponentGrid[i, j] = 1.5;
                    }
                    else if (opponentGrid[i, j] > 1)
                    {
                        opponentGrid[i, j] = 1.75;
                    }
                }
            }
        }

        //Sets cells surrounding a hit cell to a high probability
        public void SurroundingHitCell()
        {

            for (int i = 0; i < Game.GridSize; i++)
            {
                for (int j = 0; j < Game.GridSize; j++)
                {
                    if (i > 0)
                        opponentGrid[i - 1, j] = opponentGrid[i - 1, j] + 10;
                    if (i < Game.GridSize - 1)
                        opponentGrid[i + 1, j] = opponentGrid[i + 1, j] + 10;
                    if (j > 0)
                        opponentGrid[i, j - 1] = opponentGrid[i, j - 1] + 10;
                    if (j < Game.GridSize - 1)
                        opponentGrid[i, j + 1] = opponentGrid[i, j + 1] + 10;
                }
            }
        }

        //Manage Missed Attacks
        public void MissedAttacks()
        {
            Position p;
            for (int i = 0; i < Game.GridSize; i++)
            {
                for (int j = 0; j < Game.GridSize; j++)
                {
                    p = new Position(i, j);
                    if (Game.HitOrMissAt(p) == BattleShipGame.HitOrMissEnum.MISS)
                    {
                        opponentGrid[i, j] = 0;
                    }
                }
            }
        }

        public Position nextProbable()
        {
            int row = 0;
            int column = 0;
            double currentProbability = 0;
            double previousProbability = 0;
            int shipSearchLength = 4;
            bool addToProbability = true;

            //This section assigns a higher probability to cells that can contain the ship of 
            //the current shipSearchLength
            for (int i = 0; i < Game.GridSize; i++)
            {
                for (int j = 0; j < Game.GridSize; j++)
                {
                    if (opponentGrid[i, j] >= 1)
                    {
                        //Checks cells to the right
                        for (int s = 0; s < shipSearchLength; s++)
                        {
                            int k = j;
                            if (k + s < Game.GridSize)
                            {
                                if (opponentGrid[i, k + s] >= 1)
                                {
                                    addToProbability = true;
                                }
                                else
                                {
                                    addToProbability = false;
                                }
                            }
                        }
                        
                        //Checks cells to the left
                        for (int s = 0; s < shipSearchLength; s++)
                        {
                            int k = j;
                            if (k - s >= 0)
                            {
                                if (opponentGrid[i, k - s] >= 1)
                                {
                                    addToProbability = true;
                                }
                                else
                                {
                                    addToProbability = false;
                                }
                            }
                        }

                        //Checks cells above
                        for (int s = 0; s < shipSearchLength; s++)
                        {
                            int k = i;
                            if (k + s < Game.GridSize)
                            {
                                if (opponentGrid[k + s, j] >= 1)
                                {
                                    addToProbability = true;
                                }
                                else
                                {
                                    addToProbability = false;
                                }
                            }
                        }

                        //Checks cells underneath
                        for (int s = 0; s < shipSearchLength; s++)
                        {
                            int k = i;
                            if (k + s < 0)
                            {
                                if (opponentGrid[k - s, j] >= 1)
                                {
                                    addToProbability = true;
                                }
                                else
                                {
                                    addToProbability = false;
                                }
                            }
                        }
                        if (addToProbability)
                        {
                            opponentGrid[i, j] = opponentGrid[i, j] + 1;
                        }
                        else
                        {
                            addToProbability = true;
                        }
                    }
                }

            }


            //This section of code will pick the position in the grid with the highest probability of 
            //hitting a ship with the current shipSearchLength
            for (int i = 0; i < Game.GridSize; i++)
            {
                for (int j = 0; j < Game.GridSize; j++)
                {
                    currentProbability = opponentGrid[i, j];
                    if (currentProbability > previousProbability)
                    {
                        row = i;
                        column = j;
                    }
                    previousProbability = opponentGrid[i, j];
                }
            }
            return new Position(row, column); //Sets the position to be attacked,
        }
    }
}
