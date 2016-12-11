// ***********************************************************************
// Assembly         : Project6
// Author           : Richard Lesh
// Created          : 11-23-2016
//
// Last Modified By : Richard Lesh
// Last Modified On : 12-01-2016
// ***********************************************************************
// <copyright file="Program.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary>Immutable structure to represent a 2D position on the grid.</summary>
// ***********************************************************************
using System;

namespace Project6
{
    /// <summary>
    /// Immutable structure to represent a 2D position on the grid.
    /// </summary>
    public struct Position
    {
        public int Row { get; }
        public int Column { get; }
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is Position)) return false;
            Position p = (Position)obj;
            if (Row != p.Row) return false;
            if (Column != p.Column) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return 31 * Row + Column;
        }

        public static bool operator ==(Position lhs, Position rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Position lhs, Position rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", Row, Column);
        }
    }
}
