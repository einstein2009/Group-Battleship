using System;

namespace Project6
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
