// ***********************************************************************
// Assembly         : BattleshipSimple
// Author           : Richard Lesh
// Created          : 11-17-2016
//
// Last Modified By : Richard Lesh
// Last Modified On : 12-01-2016
// ***********************************************************************
// <copyright file="Grid.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary>This class implements the Grid of a Battleship game.</summary>
// ***********************************************************************
using System;

namespace Gsd311.Week6.Group3
{
    /// <summary>
    /// Class that represents the playing board of the game Battleship.
    /// </summary>
    internal class Grid
    {
        public int GridSize { get; }
        private char[,] CellSymbols;  // Ship symbol, '.' if sea and 'X' if bombed
        private ConsoleColor[,] CellColors;
        public const string ColumnLabels = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid"/> class.
        /// </summary>
        /// <param name="gridsize">The width and height of the playing area.</param>
        public Grid(int gridsize)
        {
            if (gridsize < 5) gridsize = 5;
            if (gridsize > 26) gridsize = 26;
            GridSize = gridsize;
            CellSymbols = new char[gridsize, gridsize];
            CellColors = new ConsoleColor[gridsize, gridsize];
            Clear();
        }

        /// <summary>
        /// Clears this playing board to all open sea.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < GridSize; ++i)
            {
                for (int j = 0; j < GridSize; ++j)
                {
                    CellSymbols[i, j] = '.';
                    CellColors[i, j] = ConsoleColor.Black;
                }
            }
        }

        /// <summary>
        /// Sets the fleet on this playing board.
        /// </summary>
        public void SetFleet(Fleet fleet)
        {
            Clear();
            foreach (Ship s in fleet.Ships)
            {
                for (int i = 0; i < s.Length; ++i)
                {
                    SetCell(s.GetPosition(i), s.Color, s.ShipSymbol);
                }
            }
        }

        /// <summary>
        /// Sets grid cell color and symbol.
        /// </summary>
        /// <param name="p">The Position to set.</param>
        /// <param name="c">The ConsoleColor</param>
        /// <param name="symbol">The symbol character</param>
        public void SetCell(Position p, ConsoleColor c, char symbol)
        {
            CellSymbols[p.Row, p.Column] = symbol;
            CellColors[p.Row, p.Column] = c;
        }
        
        /// <summary>
        /// Draws the playing board.
        /// </summary>
        public void Draw()
        {
            // Draw top labels
            Console.Write("   ");
            for (int i = 0; i < GridSize; ++i)
            {
                Console.Write(ColumnLabels[i]);
            }
            Console.WriteLine();
 
            // Draw row
            for (int i = 0; i < GridSize; i++)
            {
                Console.Write((i + 1).ToString("00") + " ");
                for (int j = 0; j < GridSize; j++)
                {
                    char symbolToDraw = CellSymbols[i, j];
                    Console.BackgroundColor = CellColors[i, j];
                    Console.Write(symbolToDraw);
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
            }
        }
    }
}
