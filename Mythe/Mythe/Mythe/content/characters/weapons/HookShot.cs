using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Mythe.dependencies;
using Mythe.resources;

namespace Mythe.content.characters.weapons
{
    class HookShot
    {
        public List<Texture2D> textures = new List<Texture2D>();

        public Vector2 offset;
        public Vector2 position = new Vector2();
        public Vector2 shootPos = new Vector2(0, 0);
        public Vector2 origin;

        public Rectangle srcRect = new Rectangle(0, 0, 490, 90);

        public int currentFrame = 1;
        public int frameRowsX = 6, frameRowsY = 5;
        public int beginWidth = 0, frameWidth;
        public int speed;
        public int throwFrames, beginFrames, totalFrames;

        public float interval;
        private float timer = 100f;

        public float rotation = 0;
        public float degreeOffset = 0;
        public int direction = -1;

        private MouseState newMouseState, oldMouseState;

        // test
        private Texture2D cross;

        public HookShot(Vector2 position, Vector2 offset, int speed, int throwFrames, int beginFrames, int frameWidth)
        {
            this.position = position;
            this.offset = offset;
            this.frameWidth = frameWidth;

            this.speed = speed;

            this.throwFrames = throwFrames;
            this.beginFrames = beginFrames;

            this.frameRowsX = 6;
            this.frameRowsY = 5;

            this.cross = Globals.Content.Load<Texture2D>(Assets.CROSS);
        }

        public void UpdateHook(GameTime gameTime, Vector2 pos)
        {
            position.X = pos.X + offset.X;
            position.Y = pos.Y + offset.Y;

            newMouseState = Mouse.GetState();

            if (newMouseState.LeftButton == ButtonState.Pressed && currentFrame == 1) // first time clicking
                PlayAnimation(gameTime);
            else if (currentFrame > 1) // after already clicked
                PlayAnimation(gameTime);

            if (currentFrame > beginFrames + 2 && currentFrame < 14 && newMouseState.LeftButton == ButtonState.Released) // when released while throwing
                Backwards();

            if (currentFrame == 1) // when the player isn't trhowing
            {
                // look at mouse
                rotation = (float)Math.Atan2(position.X - Globals.ScreenPosition.X - newMouseState.X, -(position.Y - Globals.ScreenPosition.Y - newMouseState.Y));
                rotation += degreeOffset * (float)(Math.PI / 180); // TODO mouseX + cameraX

                // rotation in degrees
                float degrees = rotation * (float)(180 / Math.PI);

                // rotation limit
                float limitOffset = degreeOffset;
                if (direction == -1) limitOffset -= 180;

                if (degrees > 270 - limitOffset && degrees < 360 - limitOffset)
                    rotation = (360 - limitOffset) * (float)(Math.PI / 180);
                if (degrees > 180 - limitOffset && degrees <= 270 - limitOffset)
                    rotation = (180 - limitOffset) * (float)(Math.PI / 180);
            }

            oldMouseState = newMouseState;
        }

        public void Backwards()
        {
            currentFrame = totalFrames-1 - currentFrame;
        }

        private void PlayAnimation(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                float angle = rotation * (float)(180 / Math.PI);
                if (currentFrame < beginFrames + throwFrames / 2)
                {
                    shootPos.X = position.X + (float)Math.Cos(angle * (Math.PI / 180)) * (currentFrame * (speed * direction));
                    shootPos.Y = position.Y + (float)Math.Sin(angle * (Math.PI / 180)) * (currentFrame * (speed * direction));
                }
                else
                {
                    shootPos.X = position.X + (float)Math.Cos(angle * (Math.PI / 180)) * ((totalFrames - currentFrame) * (speed * direction));
                    shootPos.Y = position.Y + (float)Math.Sin(angle * (Math.PI / 180)) * ((totalFrames - currentFrame) * (speed * direction));
                }

                timer = 0;
            }

            if (frameRowsX + frameRowsY == 0)
                srcRect.X = frameWidth * (currentFrame - 1);
            else
            {
                int rowY = (int)Math.Floor((float)(currentFrame - 1) / frameRowsX);

                srcRect.Y = rowY * srcRect.Height;
                srcRect.X = (currentFrame - 1 - (frameRowsX * rowY)) * frameWidth;
            }

            if (currentFrame > 29)
                currentFrame = 1;
        }

        public void Draw(SpriteBatch spriteBatch, int layer)
        {
            spriteBatch.Draw(textures[layer], position, srcRect, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0.0f);

            if (Debug.debugMode)
            {
                if (layer == 1)
                    spriteBatch.Draw(TextureObjects.RectTex(srcRect, Color.Purple * 0.4f), position, null, Color.White, rotation, origin, 1, SpriteEffects.None, 0);
                else
                    spriteBatch.Draw(TextureObjects.RectTex(srcRect, Color.DarkGreen * 0.4f), position, null, Color.White, rotation, origin, 1, SpriteEffects.None, 0);

                spriteBatch.Draw(cross, shootPos, null, Color.White, rotation, new Vector2(cross.Width / 2, cross.Height / 2), 1, SpriteEffects.None, 0);
                spriteBatch.Draw(cross, position, null, Color.Blue, 0f, new Vector2(cross.Width / 2, cross.Height / 2), 1, SpriteEffects.None, 0);
            }
        }
    }
}
