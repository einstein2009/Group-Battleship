using System;

namespace Project6
{
    public class BattleShip : Ship
    {
        public BattleShip() :
            base(4)
        {
            Color = ConsoleColor.Blue;
            ShipSymbol = 'B';
        }

        public override bool IsBattleShip
        {
            get
            {
                return true;
            }
        }

        public override object Clone()
        {
            BattleShip newShip = new BattleShip();
            newShip.Initialize(this);
            return newShip;
        }
    }
}
