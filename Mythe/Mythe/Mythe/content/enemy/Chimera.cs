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
    public enum ChimeraAnimation
    {
        idle,
        run,
        attack,
        hit,
        stun,
        die
    }

    public enum ChimeraState
    {
        Normal,
        Aggressive,
        Attack,
        Stun,
        Hit,
        Die
    }

    class Chimera : Enemy
    {
        private List<Texture2D> _textures = new List<Texture2D>();
        private Rectangle attackBounds;

        public ChimeraState currentState;

        public ChimeraAnimation currentAnimation = ChimeraAnimation.idle;
        public ChimeraAnimation previousAnimation = ChimeraAnimation.run;

        private float _timer;

        private int previousDirection;

        public Chimera(Vector2 position, int _health)
            : base(Globals.Content.Load<Texture2D>(Assets.CHIMERA_IDLE), position)
        {
            bounds = new Rectangle((int)position.X, (int)position.Y, 220, 190);
            attackBounds = new Rectangle(0, 0, 74, 190);

            velocity = new Vector2(200, 0);
            interval = 20f;
            gravity = 24f;

            health = _health;

            direction = -1;
            previousDirection = direction;

            _textures.Add(Globals.Content.Load<Texture2D>(Assets.CHIMERA_IDLE));
            _textures.Add(Globals.Content.Load<Texture2D>(Assets.CHIMERA_RUN));
            _textures.Add(Globals.Content.Load<Texture2D>(Assets.CHIMERA_ATTACK));
            _textures.Add(Globals.Content.Load<Texture2D>(Assets.CHIMERA_HIT));
            _textures.Add(Globals.Content.Load<Texture2D>(Assets.CHIMERA_STUN));
            _textures.Add(Globals.Content.Load<Texture2D>(Assets.CHIMERA_DIE));

            currentState = ChimeraState.Normal;
            _timer = 3000f;
            AnimationUpdate();
        }

        public override void Update(GameTime gameTime)
        {
            if (IsInScreen() && currentState != ChimeraState.Die)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                PlayerCollision();

                if (currentState == ChimeraState.Normal)
                {
                    _timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (_timer < 0f)
                    {
                        currentAnimation = ChimeraAnimation.run;

                        if (SideCollision(elapsed))
                            direction = -direction;
                        CheckFall(elapsed);
                        MovementUpdate(elapsed);
                    }
                    if (_timer < -2000f)
                    {
                        currentAnimation = ChimeraAnimation.idle;
                        _timer = 3000f;
                    }
                    Debug.text = "" + CheckPlayerDistance() + "   " + (player.bounds.Bottom > bounds.Top) + "   " + (player.bounds.Top < bounds.Bottom);

                    if (CheckPlayerDistance() < 600 && (player.bounds.Bottom > bounds.Top && player.bounds.Top < bounds.Bottom))
                        currentState = ChimeraState.Aggressive;
                }
                if (currentState == ChimeraState.Aggressive)
                {
                    if (position.X < player.position.X)
                        direction = 1;
                    else
                        direction = -1;

                    currentAnimation = ChimeraAnimation.run;
                    SideCollision(elapsed);
                    MovementUpdate(elapsed);

                    if (CheckPlayerDistance() > 600 || player.bounds.Bottom < bounds.Top || player.bounds.Top > bounds.Bottom)
                    {
                        currentState = ChimeraState.Normal;
                        currentAnimation = ChimeraAnimation.idle;
                        _timer = 3000f;
                    }
                    if (CheckPlayerDistance() < 160)
                    {
                        currentState = ChimeraState.Attack;
                    }
                }
                if (currentState == ChimeraState.Attack)
                {
                    if (currentAnimation != ChimeraAnimation.attack)
                    {
                        currentAnimation = ChimeraAnimation.attack;
                        currentFrame = 1;
                    }

                    if (direction == 1)
                        attackBounds.X = bounds.X + bounds.Width;
                    else
                        attackBounds.X = bounds.X - attackBounds.Width;
                    attackBounds.Y = bounds.Y;

                    if (currentFrame == totalFrames)
                        currentState = ChimeraState.Aggressive;
                }
                if (currentState == ChimeraState.Hit)
                {
                    _timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    currentAnimation = ChimeraAnimation.hit;

                    if (position.X < player.position.X)
                        direction = -1;
                    else
                        direction = 1;

                    if (!SideCollision(elapsed))
                        position.X += (velocity.X / 2 * elapsed) * direction;

                    if (currentFrame == totalFrames)
                    {
                        currentAnimation = ChimeraAnimation.idle;
                        currentState = ChimeraState.Aggressive;
                        direction = -direction;
                    }
                }
                if (currentState == ChimeraState.Stun)
                {
                    _timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    currentAnimation = ChimeraAnimation.stun;

                    if (_timer < 0)
                        currentState = ChimeraState.Aggressive;
                }
                
                FallingUpdate(elapsed);
                DownCollision(elapsed);
                AnimationUpdate();
                StunUpdate();
                PlayerHit();

                if (health <= 0)
                {
                    currentAnimation = ChimeraAnimation.die;
                    currentFrame = 1;
                    currentState = ChimeraState.Die;
                    invulnerable = true;
                    AnimationUpdate();
                }

                base.Update(gameTime);
            }
            else if (currentState == ChimeraState.Die)
            {
                if (currentFrame == totalFrames)
                    dead = true;

                base.Update(gameTime);
            }
        }

        protected override void PlayerHit()
        {
            if (player.bounds.Intersects(attackBounds) && currentState == ChimeraState.Attack && currentFrame > 24)
                player.GetHit(2);

            base.PlayerHit();
        }

        protected override void PlayerCollision()
        {
            if (bounds.Intersects(player.swordBounds) && currentState != ChimeraState.Hit)
            {
                currentState = ChimeraState.Hit;
                currentFrame = 1;
                base.PlayerCollision();
            }
        }

        private void StunUpdate()
        {
            if (bounds.Contains(new Point((int)player.hookshot.shootPos.X, (int)player.hookshot.shootPos.Y)) && player.hookshot.currentFrame > 1 && currentState != ChimeraState.Stun)
            {
                _timer = 2000f;
                currentState = ChimeraState.Stun;
            }
        }

        private void CheckFall(float elapsed)
        {
            if (!DownCollision(elapsed, new Rectangle(bounds.X + (bounds.Width * direction), bounds.Y, bounds.Width, bounds.Height)))
                direction = -direction;
        }

        public override void MovementUpdate(float elapsed)
        {
            base.MovementUpdate(elapsed);
        }

        public void AnimationUpdate()
        {
            if (currentAnimation != previousAnimation || direction != previousDirection)
            {
                switch (currentAnimation)
                {
                    case ChimeraAnimation.idle:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 275, 225), new Vector2(32, 26), 38, 7);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 275, 225), new Vector2(24, 26), 38, 7);
                        }
                        texture = _textures[0]; // IDLE
                        break;
                    case ChimeraAnimation.run:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 285, 225), new Vector2(36, 25), 27, 6);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 285, 225), new Vector2(26, 25), 27, 6);
                        }
                        texture = _textures[1]; // RUN
                        break;
                    case ChimeraAnimation.attack:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 335, 260), new Vector2(40, 62), 37, 7);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 335, 260), new Vector2(80, 62), 37, 7);
                        }
                        texture = _textures[2]; // ATTACK
                        break;
                    case ChimeraAnimation.hit:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 350, 255), new Vector2(80, 56), 10, 4);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 350, 255), new Vector2(40, 56), 10, 4);
                        }
                        texture = _textures[3]; // HIT
                        break;
                    case ChimeraAnimation.stun:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 275, 215), new Vector2(30, 18), 32, 6);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 275, 215), new Vector2(16, 18), 32, 6);
                        }
                        texture = _textures[4]; // STUN
                        break;
                    case ChimeraAnimation.die:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 325, 280), new Vector2(26, 78), 59, 8);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 325, 280), new Vector2(60, 78), 59, 8);
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
            {
                spriteBatch.Draw(TextureObjects.RectTex(bounds, Color.Red * 0.6f), bounds, Color.White);

                if (currentState == ChimeraState.Attack)
                    spriteBatch.Draw(TextureObjects.RectTex(attackBounds, Color.Gray * 0.6f), attackBounds, Color.White);
            }
        }
    }
}
