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
        private Vector2 _pos;
        protected float _rotation;

        /**
         * Tells the camera to follow this <code>FlxCore</code> object around.
        */
        static public FlxObject followTarget;
        /**
         * Used to force the camera to look ahead of the <code>followTarget</code>.
         */
        static public Vector2 followLead;
        /**
         * Used to smoothly track the camera as it follows.
         */
        static public float followLerp;
        /**
         * Stores the top and left edges of the camera area.
         */
        static public Point followMin;
        /**
         * Stores the bottom and right edges of the camera area.
         */
        static public Point followMax;
        /**
         * Internal, used to assist camera and scrolling.
         */
        static protected Vector2 _scrollTarget;
        /**
         * Stores the basic parallax scrolling values.
         */
        static public Vector2 scroll;
        
        public FlxCamera()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
        }
        // Sets and gets zoom
        public float zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }

        public float rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /**
         * Set position of the camera
         */ 
        public void setPosition(Vector2 position)
        {
            _pos = position;
        }

        /**
         * Set position of the camera
         */ 
        public void setPosition(float x,float y)
        {
            _pos.X = x;
            _pos.Y = y;
        }

        /**
         * Auxiliary function to move the camera
         */
        public void move(Vector2 amount)
        {
            _pos += amount * FlxG.elapsed;
        }

        public void move(float x,float y)
        {
            _pos.X += x * FlxG.elapsed;
            _pos.Y += y * FlxG.elapsed;
        }

        /**
         * Get set position
         */
        public Vector2 getPosition
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            _transform = Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(rotation) *
                                         Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(FlxG.windowWidth * 0.5f, FlxG.windowHeight * 0.5f, 0));
            return _transform;
        }

        /**
         *Gets the scroll of camera
         */
        public Vector2 getScroll() 
        {
            return scroll;
        }

        //@desc		Tells the camera subsystem what FlxCore object to follow
        //@param	Target		The object to follow
        //@param	Lerp		How much lag the camera should have (can help smooth out the camera movement)
        public void follow(FlxObject Target, float Lerp)
        {
            followTarget = Target;
            followLerp = Lerp;

            if (Target == null)
                return;

            scroll.X = _scrollTarget.X = (FlxG.width >> 1) - followTarget.x - ((int)followTarget.width >> 1);
            scroll.Y = _scrollTarget.Y = (FlxG.height >> 1) - followTarget.y - ((int)followTarget.height >> 1);
        }

        //@desc		Specify an additional camera component - the velocity-based "lead", or amount the camera should track in front of a sprite
        //@param	LeadX		Percentage of X velocity to add to the camera's motion
        //@param	LeadY		Percentage of Y velocity to add to the camera's motion
        public void followAdjust(float LeadX, float LeadY)
        {
            followLead = new Vector2(LeadX, LeadY);
        }

        /**
         * Specify the boundaries of the level or where the camera is allowed to move.
         * 
         * @param	MinX				The smallest X value of your level (usually 0).
         * @param	MinY				The smallest Y value of your level (usually 0).
         * @param	MaxX				The largest X value of your level (usually the level width).
         * @param	MaxY				The largest Y value of your level (usually the level height).
         * @param	UpdateWorldBounds	Whether the quad tree's dimensions should be updated to match.
         */
        public void followBounds(int MinX, int MinY, int MaxX, int MaxY)
        {
            followBounds(MinX, MinY, MaxX, MaxY, true);
        }
        
        public void followBounds(int MinX, int MinY, int MaxX, int MaxY, bool UpdateWorldBounds)
        {
            followMin = new Point(-MinX, -MinY);
            followMax = new Point(-MaxX + FlxG.width, -MaxY + FlxG.height);
            if (followMax.X > followMin.X)
                followMax.X = followMin.X;
            if (followMax.Y > followMin.Y)
                followMax.Y = followMin.Y;
            if (UpdateWorldBounds)
                FlxU.setWorldBounds(MinX, MinY, MaxX - MinX, MaxY - MinY);
            doFollow();
        }


        /**
         * Internal function that updates the camera and parallax scrolling.
         */
        public void doFollow()
        {
            if (followTarget != null)
            {
                if (followTarget.exists && !followTarget.dead)
                {
                    _scrollTarget.X = (FlxG.width >> 1) - followTarget.x - ((int)followTarget.width >> 1);
                    _scrollTarget.Y = (FlxG.height >> 1) - followTarget.y - ((int)followTarget.height >> 1);
                    if ((followLead != null) && (followTarget is FlxSprite))
                    {
                        _scrollTarget.X -= (followTarget as FlxSprite).velocity.X * followLead.X;
                        _scrollTarget.Y -= (followTarget as FlxSprite).velocity.Y * followLead.Y;
                    }
                }
                scroll.X += (_scrollTarget.X - scroll.X) * followLerp * FlxG.elapsed;
                scroll.Y += (_scrollTarget.Y - scroll.Y) * followLerp * FlxG.elapsed;

                if (followMin != null)
                {
                    if (scroll.X > followMin.X)
                        scroll.X = followMin.X;
                    if (scroll.Y > followMin.Y)
                        scroll.Y = followMin.Y;
                }

                if (followMax != null)
                {
                    if (scroll.X < followMax.X)
                        scroll.X = followMax.X;
                    if (scroll.Y < followMax.Y)
                        scroll.Y = followMax.Y;
                }
            }
        }

        /**
         * Stops and resets the camera.
         */
        public void unfollow()
        {
            followTarget = null;
            followLead = Vector2.Zero;
            followLerp = 1;
            followMin = Point.Zero;
            followMax = Point.Zero;
            scroll = new Vector2();
            _scrollTarget = new Vector2();
        }

    }
}
