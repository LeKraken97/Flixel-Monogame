using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace org.flixel
{
    public class FlxFactory : Microsoft.Xna.Framework.Game
    {
        //primary display buffer constants
#if !WINDOWS_PHONE
        private int resX = 680; //DO NOT CHANGE THESE VALUES!!
        private int resY = 420;  //your game should only be concerned with the
                                 //resolution parameters used when you call
                                 //initGame() in your FlxGame class.
#else
        private int resX = 480; //DO NOT CHANGE THESE VALUES!!
        private int resY = 800;  //your game should only be concerned with the
                                 //resolution parameters used when you call
                                 //initGame() in your FlxGame class.
#endif

#if XBOX360
        private bool _fullScreen = true;
#else
        private bool _fullScreen = false;
#endif
        //graphics management
        private GraphicsDeviceManager _graphics;
        //other variables
        private FlxGame _flixelgame;

        //nothing much to see here, typical XNA initialization code
        public FlxFactory()
        {
            //set up the graphics device and the content manager
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            if (_fullScreen)
            {
                resX = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                resY = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }

            FlxG.Game = this;
        }

        protected override void Initialize()
        {
            //load up the master class, and away we go!
            _graphics.PreferMultiSampling = false;
            _graphics.PreferredBackBufferWidth = resX;
            _graphics.PreferredBackBufferHeight = resY;

            FlxG.windowWidth = resX;
            FlxG.windowHeight = resY;

            if (_fullScreen && _graphics.IsFullScreen == false)
            {
                _graphics.ToggleFullScreen();
            }
            _graphics.ApplyChanges();

            _flixelgame = new FlxGame();
            Components.Add(_flixelgame);
            base.Initialize();
        }

    }

    #region Application entry point

    static class Program
    {
        //application entry point
        static void Main(string[] args)
        {
            using (FlxFactory game = new FlxFactory())
            {
                game.Run();
            }
        }
    }

    #endregion
}
