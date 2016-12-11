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

            for (int i = 0; i<Game.GridSize; i++)
            {
                for (int j = 0; j < Game.GridSize; j++)
                {
                    opponentGrid[i, j] = 1;

                    if (i <= (Game.GridSize / 6) && j <= (Game.GridSize / 6))
                    {
                        opponentGrid[i, j] = 1;
                    }
                    else if (i <= (Game.GridSize / 4) && j <= (Game.GridSize / 4))
                    {
                        opponentGrid[i, j] = 2;
                    }
                    else if (i <= (Game.GridSize / 2) && j <= (Game.GridSize / 2))
                    {
                        opponentGrid[i, j] = 3;
                    }
                    else
                    {
                        opponentGrid[i, j] = 4;
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
            int row = 0;
            int column = 0;
            int shipSearchLength = 4;
            double currentProbability = 0;
            double previousProbability = 0;
            bool addToProbability = true;
            
            //This section sets all missed attacks to 0 
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

            //This section assigns a higher probability to cells that can contain the ship of 
            //the current shipSearchLength
            for (int i = 0; i < Game.GridSize; i++)
            {
                for (int j = 0; j < Game.GridSize; j++)
                {
                    if(opponentGrid [i,j] >= 1)
                    {
                        //Checks cells to the right
                        for (int s = 0; s < shipSearchLength; s++)
                        {
                            int k = j;
                            if (k + s < Game.GridSize)
                            {
                                if (opponentGrid[i, k + s] >= 1)
                                {
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

                        //Checks cells to the left
                        for (int s = 0; s < shipSearchLength; s++)
                        {
                            int k = j;
                            if (k - s >= 0)
                            {
                                if (opponentGrid[i, k - s] >= 1)
                                {
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
                        if(currentProbability > previousProbability)
                        {
                            row = i;
                            column = j;
                        }
                        previousProbability = opponentGrid[i, j]; 
                    }
                }
                p = new Position(row, column); //Sets the position to be attatcked,
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

                if (p.Row > 0)
                    opponentGrid [p.Row - 1, p.Column] = opponentGrid[p.Row - 1, p.Column] + 1000;
                if (p.Row < Game.GridSize - 1)
                    opponentGrid[p.Row + 1, p.Column] = opponentGrid[p.Row + 1, p.Column] + 1000;
                if (p.Column > 0)
                    opponentGrid[p.Row, p.Column -1] = opponentGrid[p.Row, p.Column -1] + 1000;
                if (p.Column < Game.GridSize - 1)
                    opponentGrid[p.Row, p.Column + 1] = opponentGrid[p.Row, p.Column + 1] + 1000;
                // Strategy for dealing with Positions near the hit.
            }
            else
            {
                opponentGrid[p.Row, p.Column] = 0;
            }
        }
    }
}
