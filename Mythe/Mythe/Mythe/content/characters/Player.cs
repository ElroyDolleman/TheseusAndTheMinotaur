using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Mythe.dependencies;
using Mythe.resources;
using Mythe.content.characters.weapons;
using Mythe.content.enemy;

namespace Mythe.content.characters
{
    public enum State
    {
        onGround,
        inAir,
        shootHook,
        attack,
        hit
    }

    public enum Animation
    {
        idleRight,
        idleLeft,
        runRight,
        runLeft,
        jumpRight,
        jumpLeft,
        throwRight,
        throwLeft,
        attackRight,
        attackLeft,
        hitRight,
        hitLeft,
    }

    class Player : Sprite
    {
        // state
        public State currentState = State.inAir;

        // vector
        public Vector2 velocity;
        public Vector2 offset = Vector2.Zero;

        // hit test
        public Rectangle bounds;
        public Rectangle swordBounds;

        // falling
        public float gravity;
        public float maxFallSpeed;
        public float fallSpeed = 0;

        // keyboard
        protected KeyboardState newKeyState, oldKeyState;
        protected Keys left, right, jump, attack; 

        // hookshot
        public HookShot hookshot;

        // animation
        protected Animation currentAnimation;
        protected bool flipped;

        // flicker
        protected bool hit = false;
        private bool _flicker = false;
        private float _flickerTimer = 0f;
        protected float hitTimer;
        protected float hitInterval = 1000f;

        // stats
        public int power;
        public int health;

        public Player(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            bounds = new Rectangle(0, 0, texture.Width, texture.Height);
            hitTimer = hitInterval+1f;
        }

        public override void Update(GameTime gameTime)
        {
            newKeyState = Keyboard.GetState();
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if ((int)currentState < 2)
            {
                if (!hit)
                {
                    UpdateMovement(elapsed);
                    Updatejump(elapsed);
                }
                UpdateFalling(elapsed);
            }

            bounds.X = (int)position.X;
            bounds.Y = (int)position.Y;

            UpdateAttack();
            UpdateHit(gameTime);

            if (currentState != State.shootHook)
                UpdateCollision(elapsed);

            if (currentState != State.inAir)
                UpdateHookShot(gameTime);

            oldKeyState = newKeyState;
        }

        protected virtual void UpdateAttack()
        {
            if (newKeyState.IsKeyDown(attack) && oldKeyState.IsKeyUp(attack) && currentState == State.onGround)
            {
                currentState = State.attack;

                ChangeAnimation(Animation.attackRight);
            }
            if (currentState == State.attack && currentFrame == totalFrames)
            {
                currentState = State.inAir;
                swordBounds.X = -1800;
            }
        }

        public void GetHit(int damage = 1)
        {
            if (hitTimer > hitInterval)
            {
                ChangeAnimation(Animation.hitRight);

                health -= damage;
                hit = true;
                hitTimer = 0f;
            }
        }

        protected virtual void UpdateHit(GameTime gameTime)
        {
            if (hitTimer < hitInterval)
            {
                if (currentFrame == totalFrames && hit)
                    hit = false;
                else if (!hit)
                {
                    _flickerTimer += gameTime.ElapsedGameTime.Milliseconds;

                    if (_flickerTimer > interval)
                    {
                        _flicker = !_flicker;
                        _flickerTimer = 0f;
                    }

                    hitTimer += gameTime.ElapsedGameTime.Milliseconds;

                    if (hitTimer > hitInterval)
                        _flicker = false;
                }
            }
        }

        protected virtual void UpdateMovement(float elapsed)
        {
            if (newKeyState.IsKeyDown(left))
            {
                position.X -= (float)Math.Round((decimal)velocity.X * (decimal)elapsed);

                flipped = true;
                if (currentState == State.onGround)
                    ChangeAnimation(Animation.runRight);
            }
            else if (newKeyState.IsKeyDown(right))
            {
                position.X += (float)Math.Round((decimal)velocity.X * (decimal)elapsed);

                flipped = false;
                if (currentState == State.onGround)
                    ChangeAnimation(Animation.runRight);
            }
            else if (currentState == State.onGround)
                ChangeAnimation(Animation.idleRight);
        }

        protected virtual void Updatejump(float elapsed)
        {
            if (newKeyState.IsKeyDown(jump) && oldKeyState.IsKeyUp(jump) && currentState == State.onGround)
            {
                fallSpeed = velocity.Y;
                currentState = State.inAir;
                ChangeAnimation(Animation.jumpRight);
            }
        }

        protected virtual void UpdateFalling(float elapsed)
        {
            if (fallSpeed < maxFallSpeed)
                fallSpeed += gravity;
            else
                fallSpeed = maxFallSpeed;

            position.Y += fallSpeed * elapsed;
        }

        protected virtual void UpdateCollision(float elapsed)
        {
            foreach (Rectangle rect in Globals.Collision)
            {
                if (bounds.isOnLeft(rect, velocity.X * elapsed))
                {
                    position.X = rect.Left - bounds.Width;
                    bounds.X = rect.Left - bounds.Width;
                }
                if (bounds.isOnRight(rect, velocity.X * elapsed))
                {
                    position.X = rect.Right;
                    bounds.X = rect.Right;
                }
            }

            foreach (Rectangle rect in Globals.Collision)
            {
                if (bounds.isOnTopOf(rect, fallSpeed * elapsed))
                {
                    position.Y = rect.Top - bounds.Height;
                    fallSpeed = 0;

                    if (currentState != State.attack)
                        currentState = State.onGround;
                }
                if (bounds.isUnder(rect, fallSpeed * elapsed))
                {
                    position.Y = rect.Bottom;
                    if (fallSpeed < 0) fallSpeed = 0;
                }
            }

            foreach (Rectangle rect in Globals.Platforms)
            {
                if (bounds.isOnTopOf(rect, fallSpeed * elapsed))
                {
                    position.Y = rect.Top - bounds.Height;
                    fallSpeed = 0;

                    if (currentState != State.attack)
                        currentState = State.onGround;
                }
            }

            if (fallSpeed != 0 && currentState != State.attack)
            {
                currentState = State.inAir;

                if (!hit)
                    ChangeAnimation(Animation.jumpRight);
            }
        }

        protected virtual void UpdateHookShot(GameTime gameTime)
        {
            int previousFrame = hookshot.currentFrame;

            hookshot.UpdateHook(gameTime, position);

            Rectangle grabRect = new Rectangle((int)hookshot.shootPos.X - hookshot.speed/2, (int)hookshot.shootPos.Y-hookshot.speed/2, hookshot.speed, hookshot.speed);

            foreach(Rectangle rect in Globals.Grabpoints)
            {
                if (rect.Contains(grabRect) && hookshot.currentFrame > hookshot.beginFrames + 2 && previousFrame != hookshot.currentFrame)
                {
                    if (hookshot.currentFrame < 14)
                        hookshot.Backwards();

                    float angle = hookshot.rotation * (float)(180 / Math.PI);
                    position.X += (float)Math.Cos(angle * (Math.PI / 180)) * (hookshot.speed * hookshot.direction);
                    position.Y += (float)Math.Sin(angle * (Math.PI / 180)) * (hookshot.speed * hookshot.direction);

                    hookshot.position = position + hookshot.offset;
                }
            }

            if (currentState != State.shootHook && hookshot.currentFrame > 1)
            {
                ChangeAnimation(Animation.throwRight);
                currentState = State.shootHook;
            }

            if (hookshot.currentFrame == 1 && currentState == State.shootHook)
            {
                currentState = State.inAir;
                position.X = (float)Math.Round(position.X);
                position.Y = (float)Math.Round(position.Y);
            }
        }

        protected void AnimationUpdate(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected virtual void ChangeAnimation(Animation animation)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (flipped && (currentAnimation == Animation.throwRight || currentAnimation == Animation.throwLeft))
                hookshot.Draw(spriteBatch, 1);

            Vector2 texPos = new Vector2(position.X - offset.X, position.Y - offset.Y);

            if (!_flicker)
                spriteBatch.Draw(texture, texPos, srcRect, Color.White, rotation, origin, scale, spriteEffect, layerDepth);

            if (!flipped && (currentAnimation == Animation.throwRight || currentAnimation == Animation.throwLeft))
                hookshot.Draw(spriteBatch, 0);

            if (Debug.debugMode)
            {
                spriteBatch.Draw(TextureObjects.RectTex(bounds, Color.Blue * 0.6f), bounds, Color.White);
                spriteBatch.Draw(TextureObjects.RectTex(srcRect, Color.Black * 0.14f), texPos, Color.White);
                spriteBatch.Draw(TextureObjects.RectTex(swordBounds, Color.Red * 0.6f), swordBounds, Color.White);
            }
        }

        public virtual void UpdateDead()
        {

        }
    }
}
