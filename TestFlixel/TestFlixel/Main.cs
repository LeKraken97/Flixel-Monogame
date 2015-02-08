using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace org.flixel
{
    partial class FlxGame : DrawableGameComponent
    {
        public FlxGame()
            : base(FlxG.Game)
        {
            initGame(320, 240, new TestFlixel.State(), new Color(0x13, 0x1c, 0x1b), false, new Color(0x3a, 0x5c, 0x39));
        }
    }
}