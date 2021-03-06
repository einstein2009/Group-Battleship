﻿using System;

namespace Project6
{
    public class Submarine : Ship
    {
        public Submarine() :
            base(3)
        {
            Color = ConsoleColor.Magenta;
            ShipSymbol = 'S';
        }

        public override bool IsBattleShip
        {
            get
            {
                return false;
            }
        }

        public override object Clone()
        {
            Submarine newShip = new Submarine();
            newShip.Initialize(this);
            return newShip;
        }
    }
}
