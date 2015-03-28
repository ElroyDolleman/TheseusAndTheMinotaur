using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mythe.dependencies
{
    class Sprite
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 origin;
        public Rectangle srcRect;
        public SpriteEffects spriteEffect;

        public float rotation;
        public float scale;
        public float layerDepth;

        public int currentFrame = 1, totalFrames;

        public int frameRowsX, frameRowsY;
        protected int interspace = 0;
        public float timer = 0;
        public float interval;

        public Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;

            origin = new Vector2(0, 0);
            srcRect = new Rectangle(0, 0, texture.Width, texture.Height);

            spriteEffect = SpriteEffects.None;

            rotation = 0f;
            scale = 1f;
            layerDepth = 0f;
            totalFrames = 1;
            interval = 200f;
        }

        public virtual void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
                timer = 0f;
            }

            if (currentFrame > totalFrames)
                currentFrame = 1;

            if (frameRowsX + frameRowsY == 0)
                srcRect.X = (srcRect.Width + interspace) * (currentFrame - 1);
            else
            {
                int rowY = (int)Math.Floor((float)(currentFrame-1) / frameRowsX);

                srcRect.Y = rowY * (srcRect.Height + interspace);
                srcRect.X = (currentFrame-1 - (frameRowsX * rowY)) * (srcRect.Width + interspace);
            }
        }

        public void UpdateFrameOnly()
        {
            if (frameRowsX + frameRowsY == 0)
                srcRect.X = (srcRect.Width + interspace) * (currentFrame - 1);
            else
            {
                int rowY = (int)Math.Floor((float)(currentFrame - 1) / frameRowsX);

                srcRect.Y = rowY * (srcRect.Height + interspace);
                srcRect.X = (currentFrame - 1 - (frameRowsX * rowY)) * (srcRect.Width + interspace);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, srcRect, Color.White, rotation, origin, scale, spriteEffect, layerDepth);
        }
    }
}
