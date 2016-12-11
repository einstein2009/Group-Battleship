using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project6
{
    public class DumbPlayer : Player
    {
        private int count;
        private bool CountUp;

        public DumbPlayer(String name, bool topToBottom) :
            base(name)
        {
            CountUp = topToBottom;
        }

        public override void StartGame(BattleShipGame game)
        {
            base.StartGame(game);
            if (CountUp)
            {
                count = 0;
            }
            else
            {
                count = Game.GridSize * Game.GridSize - 1;
            }
        }

        public override Position Attack()
        {
            Position p = new Position(count / Game.GridSize, count % Game.GridSize);
            if (CountUp)
            {
                ++count;
            }
            else
            {
                --count;
            }
            return p;
        }

        public override void Hit(Position p)
        {
            // Don't do anything special.
        }
    }
}
