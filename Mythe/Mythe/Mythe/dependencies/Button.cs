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
    public delegate void ClickEvent(Object sender);

    class Button : Sprite
    {
        public event ClickEvent Click;

        protected Rectangle hitRect;
        protected MouseState newMouseState, oldMouseState;

        bool playing = false;

        public Button(Texture2D texture, Vector2 position, Rectangle hitRect):base(texture, position)
        {
            this.hitRect = hitRect;
            this.hitRect.X = (int)position.X;
            this.hitRect.Y = (int)position.Y;
        }

        public override void Update(GameTime gameTime)
        {
            newMouseState = Mouse.GetState();

            if (newMouseState.LeftButton == ButtonState.Pressed && currentFrame == 1)
            {
                if (hitRect.Contains(new Point(newMouseState.X, newMouseState.Y)))
                {
                    playing = true;
                }
            }
            if (playing)
                base.Update(gameTime);

            oldMouseState = newMouseState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Debug.debugMode)
                spriteBatch.Draw(TextureObjects.RectTex(hitRect, Color.Blue * 0.6f), hitRect, Color.White);

            base.Draw(spriteBatch);
        }
    }
}
