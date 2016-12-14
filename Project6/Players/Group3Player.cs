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
        private double levelOfAccuracy = 7;

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
                    opponentGrid[i, j] = levelOfAccuracy + 1;

                    for (double k = levelOfAccuracy; k > 0; k--)
                    {
                        if (i < k && j < k || i < k && j > k || j <= k || i >= Game.GridSize - k - 1 || j >= Game.GridSize - k - 1)
                        {
                            opponentGrid[i, j] = k;
                        }
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
            bool drawAIGrid = false;

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
                        row = i;
                        column = j;
                        maxProbability = currentProbability;
                    }
                }
            }
            if (drawAIGrid)
            {
                Console.Clear();
                Draw();
                Console.Read();
            }
            
            p = new Position(row, column); //Sets the position to be attacked,
            return p;
        }

        public void Draw()
        {
            // Draw top labels
            Console.Write("   ");
            for (int i = 0; i < Game.GridSize; ++i)
            {
                Console.Write(Grid.ColumnLabels[i]);
            }
            Console.WriteLine();

            // Draw row
            for (int i = 0; i < Game.GridSize; i++)
            {
                Console.Write((i + 1).ToString("00") + " ");
                for (int j = 0; j < Game.GridSize; j++)
                {
                    string symbolToDraw = opponentGrid[i, j].ToString();
                    Console.Write(symbolToDraw);
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
            }
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
                    opponentGrid[p.Row - 1, p.Column] = levelOfAccuracy + 1;
                }
            }
            if (p.Row < Game.GridSize - 1)
            {
                testingPosition = new Position(p.Row + 1, p.Column);
                if (Game.HitOrMissAt(testingPosition) == BattleShipGame.HitOrMissEnum.UNKNOWN)
                {
                    opponentGrid[p.Row + 1, p.Column] = levelOfAccuracy + 1;
                }
            }

            if (p.Column > 0)
            {
                testingPosition = new Position(p.Row, p.Column - 1);
                if (Game.HitOrMissAt(testingPosition) == BattleShipGame.HitOrMissEnum.UNKNOWN)
                {
                    opponentGrid[p.Row, p.Column - 1] = levelOfAccuracy + 1;
                }
            }

            if (p.Column < Game.GridSize - 1)
            {
                testingPosition = new Position(p.Row, p.Column + 1);
                if (Game.HitOrMissAt(p) == BattleShipGame.HitOrMissEnum.UNKNOWN)
                {
                    opponentGrid[p.Row, p.Column + 1] = levelOfAccuracy + 1;
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
