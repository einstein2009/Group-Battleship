﻿using System;

namespace Gsd311.Week6.Group3
{
    public class Cruiser : Ship
    {
        public Cruiser() :
            base(3)
        {
            Color = ConsoleColor.Cyan;
            ShipSymbol = 'C';
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
            Cruiser newShip = new Cruiser();
            newShip.Initialize(this);
            return newShip;
        }
    }
}
