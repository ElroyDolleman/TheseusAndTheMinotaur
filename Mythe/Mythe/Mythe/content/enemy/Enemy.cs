using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mythe.resources;
using Mythe.dependencies;

namespace Mythe.content.enemy
{
    class Enemy : Sprite
    {
        public Vector2 offset;
        public Vector2 velocity;
        public Rectangle bounds;

        public float gravity;
        public float fallSpeed = 0;
        public int direction = 1;

        public int health;
        public bool dead = false;
        public bool invulnerable = false;

        public content.characters.Player player;

        public Enemy(Texture2D texture, Vector2 position) : base(texture, position)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            bounds.X = (int)position.X;
            bounds.Y = (int)position.Y;

            base.Update(gameTime);

            if (dead)
                Globals.enemys.Remove(this);
        }

        protected bool IsInScreen()
        {
            return (position.X > Globals.ScreenPosition.X - bounds.Width - 60 && position.X < Globals.ScreenPosition.X + Globals.SCREEN_WIDTH + 60 &&
                    position.Y > Globals.ScreenPosition.Y - bounds.Height - 60 && position.Y < Globals.ScreenPosition.Y + Globals.SCREEN_HEIGHT + 60);
        }

        protected float CheckPlayerDistance()
        {
            Vector2 playPos = new Vector2(player.position.X + player.bounds.Width / 2, player.position.Y + player.bounds.Height);
            Vector2 thisPos = new Vector2(position.X + bounds.Width / 2, position.Y + bounds.Height);

            return Vector2.Distance(playPos, thisPos);
        }

        protected virtual void PlayerCollision()
        {
            health -= player.power;
        }

        protected virtual void PlayerHit()
        {
            if (player.bounds.Intersects(bounds) && !invulnerable)
                player.GetHit();
        }

        public virtual void MovementUpdate(float elapsed)
        {
            position.X += (velocity.X * elapsed) * direction;
        }

        public virtual void FallingUpdate(float elapsed)
        {
            fallSpeed += gravity;
            position.Y += fallSpeed * elapsed;
        }

        public virtual bool SideCollision(float elapsed)
        {
            foreach (Rectangle rect in Globals.Collision)
            {
                if (bounds.isOnLeft(rect, velocity.X * elapsed))
                {
                    bounds.X = rect.Left - bounds.Width;
                    position.X = bounds.X;
                    return true; // -1
                }
                if (bounds.isOnRight(rect, velocity.X * elapsed))
                {
                    bounds.X = rect.Right;
                    position.X = bounds.X;
                    return true; // 1
                }
            }
            return false;
        }

        public virtual bool DownCollision(float elapsed, Rectangle hitTest = default(Rectangle))
        {
            if (hitTest == default(Rectangle))
                hitTest = bounds;

            foreach (Rectangle rect in Globals.Collision)
            {
                if (hitTest.isOnTopOf(rect, fallSpeed * elapsed))
                {
                    if (hitTest == bounds)
                    {
                        position.Y = rect.Top - bounds.Height;
                        fallSpeed = 0;
                    }
                    return true;
                }
            }
            foreach (Rectangle rect in Globals.Platforms)
            {
                if (hitTest.isOnTopOf(rect, fallSpeed * elapsed))
                {
                    if (hitTest == bounds)
                    {
                        position.Y = rect.Top - bounds.Height;
                        fallSpeed = 0;
                    }
                    return true;
                }
            }
            return false;
        }

        protected void ChangeAnimation(Rectangle _srcRect, Vector2 _offset, int _totalFrames, int _frameRowsX)
        {
            currentFrame = 1;
            srcRect = _srcRect;
            offset = _offset;
            totalFrames = _totalFrames;
            frameRowsX = _frameRowsX;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 texPos = new Vector2(position.X - offset.X, position.Y - offset.Y);
            spriteBatch.Draw(texture, texPos, srcRect, Color.White, rotation, origin, scale, spriteEffect, layerDepth);
        }
    }
}
