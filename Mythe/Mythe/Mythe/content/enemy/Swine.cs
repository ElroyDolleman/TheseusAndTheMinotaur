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
    public enum SwineAnimation
    {
        idle,
        run,
        attack,
        hit,
        stun,
        die
    }

    public enum SwineState
    {
        Friendly,
        Aggressive,
        Attack,
        Scared,
        Rest,
        Stun,
        Hit,
        Die
    }

    class Swine : Enemy
    {
        private List<Texture2D> _textures = new List<Texture2D>();

        public SwineState currentState;

        public SwineAnimation currentAnimation = SwineAnimation.idle;
        public SwineAnimation previousAnimation = SwineAnimation.run;

        private float _timer;

        private int previousDirection;

        public Swine(Vector2 position, int _health)
            : base(Globals.Content.Load<Texture2D>(Assets.SWINE_RUN), position)
        {
            bounds = new Rectangle((int)position.X, (int)position.Y, 120, 104);
            
            velocity = new Vector2(300, 0);
            interval = 40f;
            gravity = 18f;

            health = _health;

            direction = -1;
            previousDirection = direction;

            _textures.Add(Globals.Content.Load<Texture2D>(Assets.SWINE_IDLE));
            _textures.Add(Globals.Content.Load<Texture2D>(Assets.SWINE_RUN));
            _textures.Add(Globals.Content.Load<Texture2D>(Assets.SWINE_ATTACK));
            _textures.Add(Globals.Content.Load<Texture2D>(Assets.SWINE_HIT));
            _textures.Add(Globals.Content.Load<Texture2D>(Assets.SWINE_STUN));
            _textures.Add(Globals.Content.Load<Texture2D>(Assets.SWINE_DIE));

            currentState = SwineState.Friendly;
            _timer = 3000f;
            AnimationUpdate();
        }

        public override void Update(GameTime gameTime)
        {
            if (IsInScreen() && currentState != SwineState.Die)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                bool correctState = currentState != SwineState.Scared && currentAnimation != SwineAnimation.idle && currentAnimation != SwineAnimation.stun && currentState != SwineState.Attack;

                FallingUpdate(elapsed);
                DownCollision(elapsed);
                if (SideCollision(elapsed) && correctState)
                    direction = -direction;
                PlayerCollision();
                StunUpdate();
                PlayerHit();

                if (currentState == SwineState.Friendly)
                {
                    _timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (_timer < 0f)
                    {
                        currentAnimation = SwineAnimation.run;
                        MovementUpdate(elapsed);
                    }
                    if (_timer < -2000f)
                    {
                        currentAnimation = SwineAnimation.idle;
                        _timer = 3000f;
                    }
                }
                if (currentState == SwineState.Aggressive)
                {
                    if (position.X < player.position.X)
                        direction = 1;
                    else
                        direction = -1;

                    Vector2 thisPos = new Vector2(position.X + bounds.Width /2, position.Y);
                    Vector2 playPos = new Vector2(player.position.X + player.bounds.Width /2, player.position.Y);

                    if (Vector2.Distance(thisPos, playPos) < 120)
                        currentState = SwineState.Attack;
                    else
                    {
                        currentAnimation = SwineAnimation.run;
                        MovementUpdate(elapsed);
                    }
                    if (position.Y < player.position.Y - bounds.Height || position.Y > player.position.Y + player.bounds.Height)
                    {
                        currentState = SwineState.Friendly;
                        currentAnimation = SwineAnimation.idle;
                        _timer = 3000f;
                    }
                }
                if (currentState == SwineState.Attack)
                {
                    currentAnimation = SwineAnimation.attack;
                    Tackle(elapsed);
                }
                if (currentState == SwineState.Stun)
                {
                    _timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    currentAnimation = SwineAnimation.stun;

                    if (_timer < 0)
                        currentState = SwineState.Aggressive;
                }
                if (currentState == SwineState.Hit)
                {
                    currentAnimation = SwineAnimation.hit;

                    if (position.X < player.position.X)
                        direction = -1;
                    else
                        direction = 1;

                    if (!SideCollision(elapsed))
                        position.X += (velocity.X / 2 * elapsed) * direction;

                    if (currentFrame == totalFrames && currentAnimation == SwineAnimation.hit)
                    {
                        currentAnimation = SwineAnimation.idle;
                        currentState = SwineState.Scared;
                        _timer = 800f;
                    }
                }
                if (currentState == SwineState.Scared)
                {
                    if (position.X < player.position.X)
                        direction = -1;
                    else
                        direction = 1;

                    currentAnimation = SwineAnimation.run;
                    _timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (_timer < 0)
                        currentState = SwineState.Aggressive;
                    else
                        MovementUpdate(elapsed);
                }
                if (health <= 0)
                {
                    currentState = SwineState.Die;
                    currentAnimation = SwineAnimation.die;
                    AnimationUpdate();
                    invulnerable = true;
                }

                AnimationUpdate();

                base.Update(gameTime);
            }

            if (currentState == SwineState.Die)
            {
                if (currentFrame == totalFrames)
                    dead = true;

                base.Update(gameTime);
            }
        }

        public override void MovementUpdate(float elapsed)
        {
            base.MovementUpdate(elapsed);

            if (!DownCollision(elapsed, new Rectangle(bounds.X + (bounds.Width * direction), bounds.Y, bounds.Width, bounds.Height)) && currentState == SwineState.Friendly)
                direction = -direction;
        }

        private void StunUpdate()
        {
            if (bounds.Contains(new Point((int)player.hookshot.shootPos.X, (int)player.hookshot.shootPos.Y)) && player.hookshot.currentFrame > 1 && currentState != SwineState.Stun)
            {
                _timer = 2000f;
                currentState = SwineState.Stun;
            }
        }

        protected override void PlayerCollision()
        {
            if (bounds.Intersects(player.swordBounds) && currentState != SwineState.Hit)
            {
                currentState = SwineState.Hit;

                base.PlayerCollision();
            }
        }

        public void Tackle(float elapsed)
        {
            if (currentFrame > 14 && currentFrame < 22)
                position.X += (float)Math.Round((velocity.X / 2 * elapsed) * direction);

            if (currentFrame == 14)
                fallSpeed = -200f;

            if (currentFrame == totalFrames)
            {
                currentState = SwineState.Aggressive;
                _timer = 1600f;
            }
        }

        public void AnimationUpdate()
        {
            if (currentAnimation != previousAnimation || direction != previousDirection)
            {
                switch (currentAnimation)
                {
                    case SwineAnimation.idle:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 140, 125), new Vector2(10, 15), 38, 7);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 140, 125), new Vector2(10, 15), 38, 7);
                        }
                        texture = _textures[0]; // IDLE
                        break;
                    case SwineAnimation.run:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 205, 141), new Vector2(76, 25), 13, 4);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 205, 141), new Vector2(10, 25), 13, 4);
                        }
                        texture = _textures[1]; // RUN
                        break;
                    case SwineAnimation.attack:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 165, 150), new Vector2(0, 42), 38, 7);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 165, 150), new Vector2(40, 42), 38, 7);
                        }
                        texture = _textures[2]; // ATTACK
                        break;
                    case SwineAnimation.hit:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 140, 125), new Vector2(10, 10), 10, 4);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 140, 125), new Vector2(10, 10), 10, 4);
                        }
                        texture = _textures[3]; // HIT
                        break;
                    case SwineAnimation.stun:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 135, 120), new Vector2(10, 10), 20, 5);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 135, 120), new Vector2(0, 10), 20, 5);
                        }
                        texture = _textures[4]; // STUN
                        break;
                    case SwineAnimation.die:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 370, 180), new Vector2(0, 58), 55, 8);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 370, 180), new Vector2(236, 58), 55, 8);
                        }
                        texture = _textures[5]; // DIE
                        break;
                }
            }

            previousDirection = direction;
            previousAnimation = currentAnimation;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (Debug.debugMode)
                spriteBatch.Draw(TextureObjects.RectTex(bounds, Color.Red * 0.6f), bounds, Color.White);
        }
    }
}
