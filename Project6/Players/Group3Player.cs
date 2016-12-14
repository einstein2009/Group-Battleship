// ***********************************************************************
// Assembly         : Project6
// Author           : Matthew Wyant
// Created          : 12-11-2016
//
// Last Modified By : Matthew Wyant
// Last Modified On : 12-12-2016
// ***********************************************************************
// <copyright file="Group3Player.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary>Group3 AI player class</summary>
// ***********************************************************************


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsd311.Week6.Group3
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
            int row = 0;
            int column = 0;
            double currentProbability = 0;
            double maxProbability = 0;
            

            AttackGridReset();
            AddToProbability();
            MissedAttacks();
            HitAttacks();

            //This section of code will pick the position in the grid with the highest probability of 
            //hitting a ship with the current shipSearchLength
            for (int i = 0; i < Game.GridSize; i++)
            {
                for (int j = 0; j < Game.GridSize; j++)
                {
                    currentProbability = opponentGrid[i, j];
                    if(currentProbability > maxProbability)
                    {
                        row = i;
                        column = j;
                        maxProbability = currentProbability;
                    }
                }
            }
            p = new Position(row, column); //Sets the position to be attacked,
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
                SurroundingHitCell();
                //opponentGrid[p.Row, p.Column] = 0;
            }
        }

        //Resets Grid so that the probability doesn't get messed up
        public void AttackGridReset()
        {
            for (int i = 0; i < Game.GridSize; i++)
            {
                for (int j = 0; j < Game.GridSize; j++)
                {
                    if (i <= (Game.GridSize / 4) && j <= (Game.GridSize / 4))
                    {
                        if (opponentGrid[i, j] != 10)
                        {
                            opponentGrid[i, j] = 1;
                        }
                        
                    }
                    else if (i <= (Game.GridSize / 3) && j <= (Game.GridSize / 3))
                    {
                        if (opponentGrid[i, j] != 10)
                        {
                            opponentGrid[i, j] = 1.25;
                        }
                    }
                    else if (i <= (Game.GridSize / 2) && j <= (Game.GridSize / 2))
                    {
                        if (opponentGrid[i, j] != 10)
                        {
                            opponentGrid[i, j] = 1.5;
                        }
                    }
                    else if (opponentGrid[i, j] >= 1)
                    {
                        if (opponentGrid[i, j] != 10)
                        {
                            opponentGrid[i, j] = 1.75;
                        }
                    }
                }
            }
        }

        //Sets cells surrounding a hit cell to a high probability
        //TBD: This could use more work
        public void SurroundingHitCell ()
        {
            int probabilityInt = 10;
            Position p;
            for (int i = 0; i < Game.GridSize; i++)
            {
                for (int j = 0; j < Game.GridSize; j++)
                {
                    p = new Position(i, j);
                    if (Game.HitOrMissAt(p) == BattleShipGame.HitOrMissEnum.HIT)
                    {
                        if (i > 0)
                        {
                            p = new Position(i-1, j);
                            if (Game.HitOrMissAt(p) == BattleShipGame.HitOrMissEnum.UNKNOWN)
                            {
                                opponentGrid[i - 1, j] = opponentGrid[i - 1, j] + probabilityInt;
                            }
                        }
                        if (i < Game.GridSize - 1)
                        {
                            p = new Position(i + 1, j);
                            if (Game.HitOrMissAt(p) == BattleShipGame.HitOrMissEnum.UNKNOWN)
                            {
                                opponentGrid[i + 1, j] = opponentGrid[i + 1, j] + probabilityInt;
                            }
                        }
                            
                        if (j > 0)
                        {
                            p = new Position(i, j - 1);
                            if (Game.HitOrMissAt(p) == BattleShipGame.HitOrMissEnum.UNKNOWN)
                            {
                                opponentGrid[i, j - 1] = opponentGrid[i, j - 1] + probabilityInt;
                            }
                        }
                            
                        if (j < Game.GridSize - 1)
                        {
                            p = new Position(i, j + 1);
                            if (Game.HitOrMissAt(p) == BattleShipGame.HitOrMissEnum.UNKNOWN)
                            {
                                opponentGrid[i, j + 1] = opponentGrid[i, j + 1] + probabilityInt;
                            }
                        }
                            
                    }
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

        //Manage Hit Attacks
        public void HitAttacks()
        {
            Position p;
            for (int i = 0; i < Game.GridSize; i++)
            {
                for (int j = 0; j < Game.GridSize; j++)
                {
                    p = new Position(i, j);
                    if (Game.HitOrMissAt(p) == BattleShipGame.HitOrMissEnum.HIT)
                    {
                        opponentGrid[i, j] = 0;
                    }
                }
            }
        }

        //Manage Ship probability
        //TBD This section stills needs lots of work
        public void AddToProbability()
        {
            int shipSearchLength = 4;
            bool addToProbability = true;
            Position p;

            //foreach ()

            for (int i = 0; i < Game.GridSize; i++)
            {
                for (int j = 0; j < Game.GridSize; j++)
                {
                    p = new Position(i, j);
                    if (Game.HitOrMissAt(p) == BattleShipGame.HitOrMissEnum.UNKNOWN)
                    {
                        //Checks cells to the right
                        for (int s = 0; s < shipSearchLength; s++)
                        {
                            p = new Position(i, j + s);
                            if(j + s < Game.GridSize)
                            {
                                if (Game.HitOrMissAt(p) != BattleShipGame.HitOrMissEnum.UNKNOWN)
                                {
                                    addToProbability = false;
                                }
                            }
                            else
                            {
                                addToProbability = false;
                            }
                        }
                        if (addToProbability)
                        {
                            opponentGrid[i, j] = opponentGrid[i, j] + 2;
                        }
                        else
                        {
                            addToProbability = true;
                        }

                        //Checks cells to the left
                        for (int s = 0; s < shipSearchLength; s++)
                        {
                            p = new Position(i, j - s);
                            if (j - s > Game.GridSize)
                            {
                                if (Game.HitOrMissAt(p) != BattleShipGame.HitOrMissEnum.UNKNOWN)
                                {
                                    addToProbability = false;
                                }
                            }
                            else
                            {
                                addToProbability = false;
                            }
                        }
                        if (addToProbability)
                        {
                            opponentGrid[i, j] = opponentGrid[i, j] + 2;
                        }
                        else
                        {
                            addToProbability = true;
                        }

                        //Checks cells down
                        for (int s = 0; s < shipSearchLength; s++)
                        {
                            p = new Position(i + s, j);
                            if (i + s < Game.GridSize)
                            {
                                if (Game.HitOrMissAt(p) != BattleShipGame.HitOrMissEnum.UNKNOWN)
                                {
                                    addToProbability = false;
                                }
                            }
                            else
                            {
                                addToProbability = false;
                            }
                        }
                        if (addToProbability)
                        {
                            opponentGrid[i, j] = opponentGrid[i, j] + 2;
                        }
                        else
                        {
                            addToProbability = true;
                        }

                        //Checks cells up
                        for (int s = 0; s < shipSearchLength; s++)
                        {
                            p = new Position(i - s, j);
                            if (i - s > Game.GridSize)
                            {
                                if (Game.HitOrMissAt(p) != BattleShipGame.HitOrMissEnum.UNKNOWN)
                                {
                                    addToProbability = false;
                                }
                            }
                            else
                            {
                                addToProbability = false;
                            }
                        }
                        if (addToProbability)
                        {
                            opponentGrid[i, j] = opponentGrid[i, j] + 2;
                        }
                        else
                        {
                            addToProbability = true;
                        }
                    }
                }
            }
        }
    }
}
