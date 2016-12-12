using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsd311.Week6.Group3
{
    public class RandomPlayer : Player
    {
        private static Random rnd = new Random();
        private LinkedList<Position> targets;   // all Positions in order
        private Stack<Position> activeTargets;  // Positions in randomized order

        public RandomPlayer(String name) :
            base(name)
        {

        }
        /// <summary>
        /// Gets the player ready to play a new game.  First creates
        /// a list of all game Positions in order.  Then randomly picks
        /// Positions from this list and moves them to the random active
        /// target list that will be used when playing the game.
        /// </summary>
        /// <param name="game">Game for the player to play.</param>
        public override void StartGame(BattleShipGame game)
        {
            base.StartGame(game);
            targets = new LinkedList<Position>();
            activeTargets = new Stack<Position>();

            int max = Game.GridSize * Game.GridSize;
            for (int i = 0; i < max; ++i)
            {
                Position p = new Position(i / Game.GridSize, i % Game.GridSize);
                targets.AddLast(p);
            }
            for (int i = 0; i < max; ++i)
            {
                int index = rnd.Next((int)targets.LongCount());
                Position p = targets.ElementAt(index);
                targets.Remove(p);
                activeTargets.Push(p);
            }
        }

        /// <summary>
        /// Returns the next Position to attack from the target list.
        /// Checks to make sure that the Position hasn't already been
        /// played due to logic in Hit() processing.
        /// </summary>
        /// <returns>Position to attack for the turn.</returns>
        public override Position Attack()
        {
            Position p;
            do
            {
                p = activeTargets.Pop();
            } while (Game.HitOrMissAt(p) != BattleShipGame.HitOrMissEnum.UNKNOWN);
            return p;
        }

        /// <summary>
        /// Notifies the player that the Position was a hit.  To
        /// optimize the chances of future hits we push the four
        /// neighboring Positions onto our active target list.
        /// </summary>
        /// <param name="p">Hit position</param>
        public override void Hit(Position p)
        {
            if (!Game.ShipSunkAt(p))
            {
                if (p.Row > 0)
                    activeTargets.Push(new Position(p.Row - 1, p.Column));
                if (p.Row < Game.GridSize - 1)
                    activeTargets.Push(new Position(p.Row + 1, p.Column));
                if (p.Column > 0)
                    activeTargets.Push(new Position(p.Row, p.Column - 1));
                if (p.Column < Game.GridSize - 1)
                    activeTargets.Push(new Position(p.Row, p.Column + 1));
            }
        }
    }
}
