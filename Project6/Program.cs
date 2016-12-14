// ***********************************************************************
// Assembly         : Project6
// Author           : Richard Lesh
// Created          : 11-23-2016
//
// Last Modified By : Sean Srock
// Last Modified On : 12-14-2016
// ***********************************************************************
// <copyright file="Program.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary>Program to test the ship fleet class Fleet.</summary>
// ***********************************************************************
using System;

//delete this row, it is only added to push the project.

namespace Gsd311.Week6.Group3
{
    class Program
    {
        static void Main(string[] args)
        {
            int gridSize = 15;
            var winCriteria = BattleShipGame.WinCriteriaEnum.ALL;
            int numTrials = 100;

            // Define the ships to use for testing.
            Ship[] shipsToPlace = 
            {
                new AircraftCarrier(),
                new BattleShip(),
                new Cruiser(),
                new Submarine(),
                new PatrolBoat()
            };

            // Define the AI players to test.
            IPlayer[] players = {
                //new DumbPlayer("Dumb Top Down", true),
                //new DumbPlayer("Dumb Bottom Up", false),
                new RandomPlayer("Random"),
                new Group3Player("Group3AI") //Added our player
            };
            
            int[] wins = new int[players.Length];
            for (int trial = 0; trial < numTrials; ++trial)
            {
                Fleet fleet = new Fleet();

                // Place the ships randomly.  Will be used for all players.
                foreach (Ship s in shipsToPlace)
                {
                    bool shipAddedFlag = false;
                    do
                    {
                        try
                        {
                            shipAddedFlag = false;
                            s.RandomPlace(gridSize);
                            fleet.Add(s); // Might throw exception here
                            shipAddedFlag = true;
                        }
                        catch (Exception ex)
                        {
                            // do nothing
                        }
                    } while (!shipAddedFlag);
                }
                
                // Setup a game for each player and let the player know we are starting.
                BattleShipGame[] games = new BattleShipGame[players.Length];
                for (int i = 0; i < players.Length; ++i)
                {
                    games[i] = new BattleShipGame(gridSize, players[i], shipsToPlace, winCriteria);
                    players[i].StartGame(games[i]);
                }
                
                // Play the game seeing who wins first.
                bool gameOver = false;
                while (!gameOver)
                {
                    
                    for (int i = 0; i < games.Length; ++i)
                    {
                        games[i].Turn();
                        if (games[i].GameOver())
                        {
                            gameOver = true;
                            ++wins[i];
                            Console.WriteLine();
                            Console.WriteLine(players[i].Name + " won!");
                            games[i].Draw();
                        }
                    }
                    
                }
            }

            for (int i = 0; i < players.Length; ++i)
            {
                Console.WriteLine("{0} wins: {1}", players[i].Name, wins[i]);
            }
            System.Console.ReadLine();
        }
    }
}
