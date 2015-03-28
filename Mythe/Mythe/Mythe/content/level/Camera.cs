using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mythe.resources;

namespace Mythe.content.level
{
    public class Camera
    {
        public Matrix transform;
        public Vector2 position;
        public float Left, Right, Up, Down;

        public Camera()
        {
            
        }

        public void Update(Vector2 pos)
        {
            this.position = pos; // new Vector2(center.X - Globals.SCREEN_WIDTH / 2, center.Y - Globals.SCREEN_HEIGHT / 2);
            transform = Matrix.CreateScale(new Vector3(1f, 1f, 0)) *
                Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0));

            Left = position.X;
            Right = position.X + Globals.SCREEN_WIDTH;
            Up = position.Y;
            Down = position.Y + Globals.SCREEN_HEIGHT;
        }
    }
}
