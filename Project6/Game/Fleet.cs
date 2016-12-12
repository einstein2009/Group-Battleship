// ***********************************************************************
// Assembly         : Project6
// Author           : Richard Lesh
// Created          : 11-23-2016
//
// Last Modified By : Richard Lesh
// Last Modified On : 11-26-2016
// ***********************************************************************
// <copyright file="Fleet.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary>Class to represent a fleet of ships.</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsd311.Week6.Group3
{
    class ShipCollisionException : Exception
    {
    }

    /// <summary>
    /// Class that represents a fleet of ships.
    /// </summary>
    public class Fleet
    {
        public List<Ship> Ships { get; private set; } = new List<Ship>();

        /// <summary>
        /// Adds the specified ship to the fleet of ships.  
        /// Throws an exception if the new ship collides with an
        /// existing ship in the fleet.
        /// </summary>
        /// <param name="newShip">The ship to add.</param>
        /// <exception cref="Project6.ShipCollisionException"></exception>
        public void Add(Ship newShip)
        {
            for (int i = 0; i < Ships.Count; ++i)
            {
                if (Collision(newShip, Ships.ElementAt(i))) throw new ShipCollisionException();
            }
            Ships.Add(newShip);
        }

        /// <summary>
        /// Adds a clone of the specified ship to the fleet of ships.  
        /// Throws an exception if the new ship collides with an
        /// existing ship in the fleet.
        /// </summary>
        /// <param name="newShip">The ship to add.</param>
        /// <exception cref="Project6.ShipCollisionException"></exception>
        public void AddClone(Ship newShip)
        {
            for (int i = 0; i < Ships.Count; ++i)
            {
                if (Collision(newShip, Ships.ElementAt(i))) throw new ShipCollisionException();
            }
            Ships.Add((Ship)newShip.Clone());
        }

        /// <summary>
        /// Clears this instance of ships.
        /// </summary>
        public void clear()
        {
            Ships.Clear();
        }

        /// <summary>
        /// Indicates whether or not the BattleShip has been sunk.
        /// </summary>
        /// <value><c>true</c> if the BattleShip is sunk; otherwise, <c>false</c>.</value>
        public bool SunkMyBattleship
        {
            get
            {
                foreach (Ship s in Ships)
                {
                    if (s.IsBattleShip)
                    {
                        return s.Sunk;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Indicates whether or not the ship at a given Position is sunk.
        /// </summary>
        /// <param name="p">The Position to attack.</param>
        /// <returns><c>true</c> if the ship at p is sunk, <c>false</c> otherwise.</returns>
        public bool ShipSunkAt(Position p)
        {
            foreach (Ship s in Ships)
            {
                for (int i = 0; i < s.Length; ++i)
                {
                    if (p == s.GetPosition(i))
                    {
                        return s.Sunk;
                    }
                }
            }
            return false;
        }
        
        /// <summary>
        /// Indicates whether or not the entire fleet has been sunk.
        /// </summary>
        /// <value><c>true</c> if ships are sunk; otherwise, <c>false</c>.</value>
        public bool AllSunk
        {
            get
            {
                foreach (Ship s in Ships)
                {
                    if (!s.Sunk)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Attacks the specified position.
        /// </summary>
        /// <param name="p">The Position to attack.</param>
        /// <returns><c>true</c> if a ship is hit, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool Attack(Position p)
        {
            if (p.Row < 0 || p.Column < 0) throw new ArgumentException();
            foreach (Ship s in Ships)
            {
                if (s.Attack(p)) return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the two ships passed share a common
        /// position, i.e. they collide.
        /// </summary>
        /// <param name="ship1">The first ship</param>
        /// <param name="ship2">The second ship</param>
        /// <returns><c>true</c> if the ships have a common position, <c>false</c> otherwise.</returns>
        static public bool Collision(Ship ship1, Ship ship2)
        {
            for (int i = 0; i < ship1.Length; ++i)
            {
                for (int j = 0; j < ship2.Length; ++j)
                {
                    if (ship1.GetPosition(i) == ship2.GetPosition(j)) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Prints the fleet of ships.  For each ship print the 
        /// ship itself (using ToString()) on one line followed by
        /// all the ships coordinates on a second line.
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < Ships.Count; ++i)
            {
                Ship Ship = Ships.ElementAt(i);
                Console.WriteLine(Ship);
                Console.Write("\t");
                for (int j = 0; j < Ship.Length; ++j)
                {
                    Console.Write(Ship.GetPosition(j));
                }
                Console.WriteLine();
            }
        }
    }
}
