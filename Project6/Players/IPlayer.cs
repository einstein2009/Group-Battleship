using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project6
{
    public interface IPlayer
    {
        /// <summary>
        /// Name of the player property.
        /// </summary>
        /// <value>Name of player.</value>
        string Name { get; set; }

        /// <summary>
        /// Game that the player is playing property.
        /// </summary>
        /// <value>Game to play.</value>
        BattleShipGame Game { get; set; }

        /// <summary>
        /// Gets the player ready to play a new game.
        /// </summary>
        /// <param name="game">Game for the player to play.</param>
        void StartGame(BattleShipGame game);

        /// <summary>
        /// Returns a position to attack.
        /// </summary>
        /// <returns>Position to attack for the turn.</returns>
        Position Attack();

        /// <summary>
        /// Notifies the player that the position was a hit.
        /// </summary>
        /// <param name="p">Hit position</param>
        void Hit(Position p);
    }
}
