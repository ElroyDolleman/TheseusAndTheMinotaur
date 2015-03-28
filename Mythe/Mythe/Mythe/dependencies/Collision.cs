using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mythe.dependencies
{
    static class Collision
    {
        public static bool isOnTopOf(this Rectangle r1, Rectangle r2, float offset = 0)
        {
            return (r1.Bottom >= r2.Top && r1.Bottom <= r2.Top + offset && r1.Right > r2.Left && r1.Left < r2.Right);
        }

        public static bool isUnder(this Rectangle r1, Rectangle r2, float offset = 0)
        {
            return (r1.Top <= r2.Bottom && r1.Top >= r2.Top + offset && r1.Right > r2.Left && r1.Left < r2.Right);
        }

        public static bool isOnLeft(this Rectangle r1, Rectangle r2, float offset = 0)
        {
            return (r1.Right >= r2.Left && r1.Right <= r2.Left + offset && r1.Bottom > r2.Top && r1.Top < r2.Bottom);
        }

        public static bool isOnRight(this Rectangle r1, Rectangle r2, float offset = 0)
        {
            return (r1.Left <= r2.Right && r1.Left >= r2.Right - offset && r1.Bottom > r2.Top && r1.Top < r2.Bottom);
        }
    }
}
