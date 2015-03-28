using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mythe.content.level;
using Mythe.content.enemy;

using Mythe.dependencies;

namespace Mythe.resources
{
    class Globals
    {
        public static ContentManager Content;
        public static GraphicsDevice graphicsDevice;

        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;
        public static Vector2 ScreenPosition;
        public static Vector2 Respawn;

        public static List<Rectangle> Collision;
        public static List<Rectangle> Platforms;
        public static List<Rectangle> Grabpoints;
        public static List<Door> Doors;
        public static List<LockedDoor> LockedDoors;
        public static List<Enemy> enemys;

        public static List<Sprite> foreground;
        public static List<Sprite> background;
        public static List<Paralax> paralax;

        public static List<Sprite> keys;
        public static int keyAmount = 1;
    }
}
