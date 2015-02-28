using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel.physics
{
	public class B2FlxWorld
	{
		public static Vector2 gravity = new Vector2 (0, 100);
		public static bool sleep = false;

		public static World world = new World(gravity,sleep);

		/**
		 * Set the gravity for this world
		 */
		public static Vector2 setGravity(Vector2 grav)
		{
			return world.Gravity = grav;
		}

		/**
		 * After the collision occured set the objects to sleep? It may improve the performance.
		 */ 
		public static bool setSleep(bool sleep)
		{
			return world._allowSleep = sleep;
		}

	}
}

