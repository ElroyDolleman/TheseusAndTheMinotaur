using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mythe.resources;
using Mythe.dependencies;

namespace Mythe.content.screens
{
    class UI
    {
        public static Sprite healthbar = new Sprite(Globals.Content.Load<Texture2D>(Assets.HEALTHBAR), new Vector2(382, 651));

        public static void DrawHealthbar(SpriteBatch spriteBatch, Vector2 camera)
        {
            spriteBatch.Draw(healthbar.texture, new Vector2(camera.X + healthbar.position.X, camera.Y + healthbar.position.Y), healthbar.srcRect, Color.White);
        }
    }
}
