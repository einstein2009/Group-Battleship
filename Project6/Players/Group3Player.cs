using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project6
{
	public class Group3Player : Player
	{
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
			// Initialize the player data structures.
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
				// compute position here and assign to p
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
