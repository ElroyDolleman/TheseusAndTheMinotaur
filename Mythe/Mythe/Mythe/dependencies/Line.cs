using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mythe.dependencies
{
    class Line
    {
        public Vector2 point1, point2;
        public Texture2D texture;

        public Line(Texture2D texture)
        {
            this.texture = texture;
            this.texture.SetData(new[] { Color.White });
        }

        public void Update(Vector2 point1, Vector2 point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }

        public void Draw(SpriteBatch spriteBatch, float width, Color color)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
