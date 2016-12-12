using System;

namespace Gsd311.Week6.Group3
{
    public class PatrolBoat : Ship
    {
        public PatrolBoat() :
            base(2)
        {
            Color = ConsoleColor.Yellow;
            ShipSymbol = 'P';
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
            PatrolBoat newShip = new PatrolBoat();
            newShip.Initialize(this);
            return newShip;
        }
    }
}
