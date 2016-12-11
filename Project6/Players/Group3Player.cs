using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project6
{
    public class Group3Player : Player
    {
        private int[,] opponentGrid;
        public Position attackPosition { get; set; }


        public Group3Player(String name) :
            base(name)
        {

        }

        /// <summary>
        /// Gets the player ready to play a new game.  Since the
        /// player can be reused for mutliple games this method
        /// must initialize all needed data structures.
        /// </summary>
        /// <param name="game">Game for the player to play.</param>
        public override void StartGame(BattleShipGame game)
        {
            base.StartGame(game);
            opponentGrid = new int[Game.GridSize, Game.GridSize];
            attackPosition = new Position();

            for (int i = 0; i<Game.GridSize; i++)
            {
                for (int j = 0; j < Game.GridSize; j++)
                {
                    if(i <= (Game.GridSize / 6) && j <= (Game.GridSize / 6))
                    {
                            opponentGrid[i, j] = 1;
                    }
                    else if (i <= (Game.GridSize / 4) && j <= (Game.GridSize / 4))
                    {
                        opponentGrid[i, j] = 2;
                    }
                    else if (i <= (Game.GridSize / 2) && j <= (Game.GridSize / 2))
                    {
                        opponentGrid[i, j] = 3;
                    }
                    else
                    {
                        opponentGrid[i, j] = 4;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the next Position to attack from the target list.
        /// Check to make sure that the Position hasn't already been
        /// played due to logic in Hit() processing.
        /// </summary>
        /// <returns>Position to attack for the turn.</returns>
        public override Position Attack()
        {
            Position p;
            do
            {
                for (int i = 0; i < Game.GridSize; i++)
                {
                    for (int j = 0; j < Game.GridSize; j++)
                    {
                        
                    }
                }
                p = attackPosition;
            } while (Game.HitOrMissAt(p) != BattleShipGame.HitOrMissEnum.UNKNOWN);
            return p;
        }

        /// <summary>
        /// Notifies the player that the Position was a hit.  To
        /// optimize the chances of future hits we should have
        /// a strategy for trying neighboring Positions when
        /// Attack() is called next.
        /// </summary>
        /// <param name="p">Hit position</param>
        public override void Hit(Position p)
        {
            if (!Game.ShipSunkAt(p))
            {
            // Strategy for dealing with Positions near the hit.
            }
        }
    }
}
