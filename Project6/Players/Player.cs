using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsd311.Week6.Group3
{
    public abstract class Player : IPlayer
    {
        public string Name { get; set; }
        public BattleShipGame Game { get; set; }

        public Player(String name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the player ready to play a new game.  In overridden
        /// methods be sure to call base.StartGame(game) to call this 
        /// base method.
        /// </summary>
        /// <param name="game">Game for the player to play.</param>
        public virtual void StartGame(BattleShipGame game)
        {
            Game = game;
        }

        public abstract Position Attack();
        public abstract void Hit(Position p);
    }
}
