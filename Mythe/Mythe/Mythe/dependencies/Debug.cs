using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Mythe.resources;

namespace Mythe.dependencies
{
    class Debug
    {
        public static bool debugMode = false;

        public static string text = "Hello World";
        public static SpriteFont font = Globals.Content.Load<SpriteFont>("font\\SpriteFont1");

        public static void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (debugMode)
            {
                Rectangle rect = new Rectangle((int)position.X, (int)position.Y, Globals.SCREEN_WIDTH, 32);

                spriteBatch.Draw(TextureObjects.RectTex(rect, Color.White), rect, Color.White);
                spriteBatch.DrawString(font, text, position, Color.Black);
            }
        }

        public static Rectangle mouseRect;
        public static Vector2 clickPos;
        public static void CreateMouseRect(SpriteBatch spriteBatch, MouseState oldMouseState)
        {
            MouseState newMouseState = Mouse.GetState();
            mouseRect = new Rectangle((int)clickPos.X, (int)clickPos.Y, newMouseState.X - (int)clickPos.X, newMouseState.Y - (int)clickPos.Y);

            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                clickPos = new Vector2(newMouseState.X, newMouseState.Y);
            }
            else if(newMouseState.LeftButton == ButtonState.Pressed)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(TextureObjects.RectTex(mouseRect, Color.White * 0.4f), mouseRect, Color.White);
                spriteBatch.End();
            }
            else if (newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                Console.WriteLine("(new Rectangle(" + (Globals.ScreenPosition.X + mouseRect.X) + ", " + (Globals.ScreenPosition.Y + mouseRect.Y) + ", " + mouseRect.Width + ", " + mouseRect.Height + "));");
            }
        }
    }
}
