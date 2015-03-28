using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mythe.dependencies
{
    static class MathClass
    {
        public static float GetAngle(Vector2 position)
        {
            return (float)Math.Atan2((double)position.X, (double)position.Y);
        }
    }
}
