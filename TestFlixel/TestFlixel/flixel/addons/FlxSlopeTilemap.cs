using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    public class FlxSlopeTilemap : FlxTilemap
    {
        // slope related variables
        protected int _snapping = 0;
        protected Vector2 _slopePoint = new Vector2();
        protected Vector2 _objPoint = new Vector2();

        public static int SLOPE_FLOOR_LEFT = 0;
        public static int SLOPE_FLOOR_RIGHT = 1;
        public static int SLOPE_CEIL_LEFT = 2;
        public static int SLOPE_CEIL_RIGHT = 3;

        protected int[] slopeFloorLeft = new int[] { };
        protected int[] slopeFloorRight = new int[] { };
        protected int[] slopeCeilLeft = new int[] { };
        protected int[] slopeCeilRight = new int[] { };

        private FlxObject _block;

        public FlxSlopeTilemap()
        {

            _block = new FlxObject();
            _block.width = _block.height = 0;
            _block.@fixed = true;
        }


        public bool checkArrays(int tileIndex)
        {
            int i = 0;
            for (i = 0; i < slopeFloorLeft.Length; i++)
            {
                if (slopeFloorLeft[i] == tileIndex)
                    return true;
            }
            for (i = 0; i < slopeFloorRight.Length; i++)
            {
                if (slopeFloorRight[i] == tileIndex)
                    return true;
            }
            for (i = 0; i < slopeCeilLeft.Length; i++)
            {
                if (slopeCeilLeft[i] == tileIndex)
                    return true;
            }
            for (i = 0; i < slopeCeilRight.Length; i++)
            {
                if (slopeCeilRight[i] == tileIndex)
                    return true;
            }

            return false;
        }

       
      

    }
}
