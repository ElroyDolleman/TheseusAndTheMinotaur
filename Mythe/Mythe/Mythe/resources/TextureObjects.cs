using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mythe.resources
{
    class TextureObjects
    {
        public static Texture2D RectTex(Rectangle rect, Color color)
        {
            if (rect.Width <= 0)
                rect.Width = 1;
            if (rect.Height <= 0)
                rect.Height = 1;

            Texture2D tex = new Texture2D(Globals.graphicsDevice, rect.Width, rect.Height);
            Color[] data = new Color[rect.Width * rect.Height];

            for (int i = 0; i < data.Length; ++i)
                data[i] = color;

            tex.SetData(data);

            return tex;
        }

        public static void DrawLine(SpriteBatch spriteBatch, Color color, Vector2 point1, Vector2 point2, float thick = 1)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            Texture2D tex = new Texture2D(Globals.graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            tex.SetData(new[] { Color.White });

            spriteBatch.Draw(tex, point1, null, color, angle, Vector2.Zero, new Vector2(length, thick), SpriteEffects.None, 0);
        }
    }
}
