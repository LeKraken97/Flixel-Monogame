using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Utilities;
using Microsoft.Xna.Framework.Windows;

namespace org.flixel
{
    public class FlxCamera
    {
        protected float _zoom;
        public Matrix _transform;
        public Vector2 _pos;
        protected float _rotation;

        public FlxCamera()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
        }
        // Sets and gets zoom
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /**
         * Auxiliary function to move the camera
         */
        public void Move(Vector2 amount)
        {
            _pos += amount;
        }
        /**
         * Get set position
         */
        public Vector2 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }
        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            _transform = Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(FlxG.windowWidth * 0.5f, FlxG.windowHeight* 0.5f, 0));
            return _transform;
        }
    }
}
