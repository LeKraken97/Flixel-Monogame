using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.flixel;
using Microsoft.Xna.Framework;

namespace org.flixel
{
    public class FlxSlopeSprite : FlxSprite
    {
        // slope related variables
        protected int _snapping = 0;
        protected Vector2 _slopePoint = new Vector2();
        protected Vector2 _objPoint = new Vector2();

        public static int SLOPE_FLOOR_LEFT = 0;
        public static int SLOPE_FLOOR_RIGHT = 1;
        public static int SLOPE_CEIL_LEFT = 2;
        public static int SLOPE_CEIL_RIGHT = 3;

        private int _tileWidth = 32;
        private int _tileHeight = 32;

        public FlxSlopeSprite(float x,float y):base(x,y)
        {
                
        }

        public FlxSlopeSprite()
            : base()
        {

        }

        public void setBoundingBoxSize(int tileWidth, int tileHeight)
        {
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
        }

        /**
         * Bounds the slope point to the slope
         * 
         * @param slope the slope to fix the slopePoint for
         */
        private void fixSlopePoint(FlxObject slope)
        {
            _slopePoint.X = FlxU.bound(_slopePoint.X, slope.x, slope.x + _tileWidth);
            _slopePoint.Y = FlxU.bound(_slopePoint.Y, slope.y, slope.y + _tileHeight);
        }

        /**
         * Is called if an object collides with a floor slope
         * @param slope the floor slope
         * @param obj the object that collides with that slope
         */
        public void onCollideFloorSlope(FlxObject slope, FlxObject obj)
        {
            // set the object's touching flag
            obj.onFloor = true;

            // adjust the object's velocity
            obj.velocity.Y = 0;

            // reposition the object
            obj.y = _slopePoint.Y - obj.height;
            if (obj.y < slope.y - obj.height)
            {
                obj.y = slope.y - obj.height;
            }
        }

        /**
         * Is called if an object collides with a ceiling slope
         * 
         * @param slope the ceiling slope
         * @param obj the object that collides with that slope
         */
        public void onCollideCeilSlope(FlxObject slope, FlxObject obj)
        {
            // set the object's touching flag
            obj.collideTop = true;

            // adjust the object's velocity
            obj.velocity.Y = 0;

            // reposition the object
            obj.y = _slopePoint.Y;
            if (obj.y > slope.y + _tileHeight)
            {
                obj.y = slope.y + _tileHeight;
            }
        }

        /**
         * Solves collision against a left-sided floor slope
         * 
         * @param slope the slope to check against
         * @param obj the object that collides with the slope
         */
        public void solveCollisionSlopeFloorLeft(FlxObject slope, FlxObject obj)
        {
            // calculate the corner point of the object
            _objPoint.X = FlxU.floor(obj.x + obj.width + _snapping);
            _objPoint.Y = FlxU.floor(obj.y + obj.height);

            /* calculate position of the point on the slope that the object might
            * overlap
            * this would be one side of the object projected onto the slope's
            * surface
            */
            _slopePoint.X = _objPoint.X;
            _slopePoint.Y = (slope.y + _tileHeight) - (_slopePoint.X - slope.x);

            // fix the slope point to the slope tile
            fixSlopePoint(slope);

            // check if the object is inside the slope
            if (_objPoint.X > slope.x + _snapping
                    && _objPoint.X < slope.x + _tileWidth + obj.width + _snapping
                    && _objPoint.Y >= _slopePoint.Y
                    && _objPoint.Y <= slope.y + _tileHeight)
            {
                // call the collide function for the floor slope
                onCollideFloorSlope(slope, obj);
            }
        }

        /**
         * Solves collision against a right-sided floor slope
         * 
         * @param slope
         *            the slope to check against
         * @param obj
         *            the object that collides with the slope
         */
        public void solveCollisionSlopeFloorRight(FlxObject slope, FlxObject obj)
        {
            // calculate the corner point of the object
            _objPoint.X = FlxU.floor(obj.x - _snapping);
            _objPoint.Y = FlxU.floor(obj.y + obj.height);

            /* calculate position of the point on the slope that the object might
             * overlap
             * this would be one side of the object projected onto the slope's
             * surface 
             */
            _slopePoint.X = _objPoint.X;
            _slopePoint.Y = (slope.y + _tileHeight)
                    - (slope.x - _slopePoint.X + _tileWidth);

            // fix the slope point to the slope tile
            fixSlopePoint(slope);

            // check if the object is inside the slope
            if (_objPoint.X > slope.x - obj.width - _snapping
                    && _objPoint.X < slope.x + _tileWidth + _snapping
                    && _objPoint.Y >= _slopePoint.Y
                    && _objPoint.Y <= slope.y + _tileHeight)
            {
                // call the collide function for the floor slope
                onCollideFloorSlope(slope, obj);
            }
        }

        /**
         * Solves collision against a left-sided ceiling slope
         * @param slope the slope to check against
         * @param obj the object that collides with the slope
         */
        public void solveCollisionSlopeCeilLeft(FlxObject slope, FlxObject obj)
        {
            // calculate the corner point of the object
            _objPoint.X = FlxU.floor(obj.x + obj.width + _snapping);
            _objPoint.Y = FlxU.ceil(obj.y);

            // calculate position of the point on the slope that the object might
            // overlap
            // this would be one side of the object projected onto the slope's
            // surface
            _slopePoint.X = _objPoint.X;
            _slopePoint.Y = (slope.y) + (_slopePoint.X - slope.x);

            // fix the slope point to the slope tile
            fixSlopePoint(slope);

            // check if the object is inside the slope
            if (_objPoint.X > slope.x + _snapping
                    && _objPoint.X < slope.x + _tileWidth + obj.width + _snapping
                    && _objPoint.Y <= _slopePoint.Y && _objPoint.Y >= slope.y)
            {
                // call the collide function for the floor slope
                onCollideCeilSlope(slope, obj);
            }
        }

        /**
         * solves collision against a right-sided ceiling slope
         * 
         * @param slope
         *            the slope to check against
         * @param obj
         *            the object that collides with the slope
         */
        public void solveCollisionSlopeCeilRight(FlxObject slope, FlxObject obj)
        {
            // calculate the corner point of the object
            _objPoint.X = FlxU.floor(obj.x - _snapping);
            _objPoint.Y = FlxU.ceil(obj.y);

            // calculate position of the point on the slope that the object might
            // overlap
            // this would be one side of the object projected onto the slope's
            // surface
            _slopePoint.X = _objPoint.X;
            _slopePoint.Y = (slope.y) + (slope.x - _slopePoint.X + _tileWidth);

            // fix the slope point to the slope tile
            fixSlopePoint(slope);

            // check if the object is inside the slope
            if (_objPoint.X > slope.x - obj.width - _snapping
                    && _objPoint.X < slope.x + _tileWidth + _snapping
                    && _objPoint.Y <= _slopePoint.Y && _objPoint.Y >= slope.y)
            {
                // call the collide function for the floor slope
                onCollideCeilSlope(slope, obj);
            }
        }

    }
}
