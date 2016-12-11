using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project6
{
    public class BattleShipGame
    {
        public int GridSize { get; protected set; }
        private IPlayer Player;
        private Grid GameGrid;
        private Fleet Fleet;
        private HitOrMissEnum[,] HitsAndMisses;
        public WinCriteriaEnum WinCriteria { get; protected set; }

        public enum HitOrMissEnum { UNKNOWN, MISS, HIT};
        public enum WinCriteriaEnum { ALL, BATTLESHIP};

        public BattleShipGame(int gridSize, IPlayer player, Ship[] ships, WinCriteriaEnum win)
        {
            GridSize = gridSize;
            GameGrid = new Grid(gridSize);
            HitsAndMisses = new HitOrMissEnum[gridSize, gridSize];
            Fleet = new Fleet();
            foreach (Ship s in ships)
            {
                Fleet.AddClone(s);
            }
            GameGrid.SetFleet(Fleet);
            Player = player;
            WinCriteria = win;
        }

        public void Turn()
        {
            Position p = Player.Attack();
            bool isHit = Fleet.Attack(p);

            if (isHit)
            {
                Player.Hit(p);
                HitsAndMisses[p.Row, p.Column] = HitOrMissEnum.HIT;
                GameGrid.SetCell(p, ConsoleColor.Red, 'X');
            }
            else
            {
                HitsAndMisses[p.Row, p.Column] = HitOrMissEnum.MISS;
                GameGrid.SetCell(p, ConsoleColor.Black, 'X');
            }
        }

        public bool GameOver()
        {
            bool result = false;

            switch (WinCriteria) {
                case WinCriteriaEnum.BATTLESHIP:
                    result = Fleet.SunkMyBattleship;
                    break;
                default:
                    result = Fleet.AllSunk;
                    break;
            }
            return result;
        }

        public HitOrMissEnum HitOrMissAt(Position p)
        {
            return HitsAndMisses[p.Row, p.Column];
        }

        public bool ShipSunkAt(Position p)
        {
            return Fleet.ShipSunkAt(p);
        }

        public void Draw()
        {
            GameGrid.Draw();
        }
    }
}
