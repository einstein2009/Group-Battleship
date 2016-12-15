// ***********************************************************************
// Assembly         : Project6
// Author           : Matthew Wyant
// Created          : 12-11-2016
//
// Last Modified By : Sean Srock
// Last Modified On : 12-14-2016
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
                    opponentGrid[i, j] = 7;

                    double Accuracy;

                    Accuracy = Math.Ceiling(Game.GridSize / 2.5d);
                    if (i < Accuracy && j < Accuracy || i < Accuracy && j >= Accuracy || j < Accuracy || i > Game.GridSize - Accuracy - 1 || j > Game.GridSize - Accuracy - 1)
                    {
                        opponentGrid[i, j] = 1;
                    }

                    Accuracy = Math.Ceiling(Game.GridSize / 3.5d);
                    if (i < Accuracy && j < Accuracy || i < Accuracy && j >= Accuracy || j < Accuracy || i > Game.GridSize - Accuracy - 1 || j > Game.GridSize - Accuracy - 1)
                    {
                        opponentGrid[i, j] = 4;
                    }

                    Accuracy = Math.Ceiling(Game.GridSize / 4d);
                    if (i < Accuracy && j < Accuracy || i < Accuracy && j >= Accuracy || j < Accuracy || i > Game.GridSize - Accuracy - 1 || j > Game.GridSize - Accuracy - 1)
                    {
                        opponentGrid[i, j] = 6;
                    }

                    Accuracy = Math.Ceiling(Game.GridSize / 6d);
                    if (i < Accuracy && j < Accuracy || i < Accuracy && j >= Accuracy || j < Accuracy || i > Game.GridSize - Accuracy - 1 || j > Game.GridSize - Accuracy - 1)
                    {
                        opponentGrid[i, j] = 3;
                    }

                    Accuracy = Math.Ceiling(Game.GridSize / 8d);
                    if (i < Accuracy && j < Accuracy || i < Accuracy && j >= Accuracy || j < Accuracy || i > Game.GridSize - Accuracy - 1 || j > Game.GridSize - Accuracy - 1)
                    {
                        opponentGrid[i, j] = 5;
                    }

                    Accuracy = Math.Floor(Game.GridSize / 10d);
                    if (i < Accuracy && j < Accuracy || i < Accuracy && j >= Accuracy || j < Accuracy || i > Game.GridSize - Accuracy - 1 || j > Game.GridSize - Accuracy - 1)
                    {
                        opponentGrid[i, j] = 2;
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

            Random rnd = new Random();

            double currentProbability = 0;
            double maxProbability = 0;

            int tempSpaces = 0;
            int numOfSpaces = (Game.GridSize + 1) *(Game.GridSize + 1);

            Position[] posList = new Position[numOfSpaces];
            Position[] posListCurrent = new Position[numOfSpaces];
            
            //
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
                        //row = i;
                        //column = j;
                        maxProbability = currentProbability;
                    }
                }
            }

            // Generate an array with all of the positions for quick parsing.
            tempSpaces = 0;
            for (int i = 0; i < Game.GridSize; i++)
            {
                for (int j = 0; j < Game.GridSize; j++)
                {
                    
                    tempSpaces++;
                    posList[tempSpaces] = new Position(i, j);
                }
            }

            // Create a new array for storing the results that are the max probability.
            tempSpaces = -1;
            for (int i = 0; i < numOfSpaces; i++)
            {
                if (opponentGrid[posList[i].Row, posList[i].Column] == maxProbability)
                {
                    tempSpaces++;
                    posListCurrent[tempSpaces] = posList[i];
                }
            }
            Array.Resize(ref posListCurrent, tempSpaces+1);
            
            int randomNumber = rnd.Next(0, tempSpaces); 

            return posListCurrent[randomNumber];
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
                opponentGrid[p.Row, p.Column] = 0;
                SurroundingHitCell(p);
            }
            else
            {
                opponentGrid[p.Row, p.Column] = 0;
            }
        }
        
        //Sets cells surrounding a hit cell to a high probability
        public void SurroundingHitCell (Position p)
        {
            Position testingPosition;

            if (p.Row > 0)
            {
                testingPosition = new Position(p.Row - 1, p.Column);
                if (Game.HitOrMissAt(testingPosition) == BattleShipGame.HitOrMissEnum.UNKNOWN)
                {
                    opponentGrid[p.Row - 1, p.Column] = 15;
                }
            }
            if (p.Row < Game.GridSize - 1)
            {
                testingPosition = new Position(p.Row + 1, p.Column);
                if (Game.HitOrMissAt(testingPosition) == BattleShipGame.HitOrMissEnum.UNKNOWN)
                {
                    opponentGrid[p.Row + 1, p.Column] = 15;
                }
            }

            if (p.Column > 0)
            {
                testingPosition = new Position(p.Row, p.Column - 1);
                if (Game.HitOrMissAt(testingPosition) == BattleShipGame.HitOrMissEnum.UNKNOWN)
                {
                    opponentGrid[p.Row, p.Column - 1] = 15;
                }
            }

            if (p.Column < Game.GridSize - 1)
            {
                testingPosition = new Position(p.Row, p.Column + 1);
                if (Game.HitOrMissAt(p) == BattleShipGame.HitOrMissEnum.UNKNOWN)
                {
                    opponentGrid[p.Row, p.Column + 1] = 15;
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
                    if (Game.HitOrMissAt(p) == BattleShipGame.HitOrMissEnum.MISS)
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
