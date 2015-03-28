using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mythe.dependencies;
using Mythe.resources;
using Mythe.content.level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mythe.content.enemy
{
    public enum SwineBossAnimation
    {
        idle,
        run,
        attack,
        hit,
        stun,
        die,
        dead
    }

    public enum SwineBossState
    {
        Normal,
        Aggressive,
        Attack,
        Counter,
        Scared,
        Hit,
        Die
    }

    class SwineBoss : Enemy
    {
        private List<Texture2D> _textures = new List<Texture2D>();

        private int closedDoors = 0;
        private int previousDirection;
        private Rectangle attackBounds;

        private SwineBossAnimation currentAnimation, previousAnimation;
        private SwineBossState currentState;

        public SwineBoss(Vector2 position, int _health)
            : base(Globals.Content.Load<Texture2D>(Assets.SWINE_BOSS_IDLE), position)
        {
            health = _health;
            bounds = new Rectangle(0, 0, 220, 180);
            attackBounds = new Rectangle(0, 0, 64, 180);

            velocity = new Vector2(320, 0);
            interval = 40f;
            gravity = 32f;

            bounds.X = (int)position.X;
            bounds.Y = (int)position.Y;

            direction = 1;
            previousDirection = direction;

            _textures.Add(texture);
            _textures.Add(Globals.Content.Load<Texture2D>(Assets.SWINE_BOSS_RUN));
            _textures.Add(Globals.Content.Load<Texture2D>(Assets.SWINE_BOSS_ATTACK));
            _textures.Add(Globals.Content.Load<Texture2D>(Assets.SWINE_BOSS_HIT));
            _textures.Add(Globals.Content.Load<Texture2D>(Assets.SWINE_BOSS_DIE));
            _textures.Add(Globals.Content.Load<Texture2D>(Assets.SWINE_BOSS_DEAD));

            currentAnimation = SwineBossAnimation.idle;
            previousAnimation = SwineBossAnimation.run;
            AnimationUpdate();

            foreach (Door door in Globals.Doors)
            {
                Globals.Collision.Add(door.bounds);
                closedDoors++;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsInScreen() && currentState != SwineBossState.Die)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                PlayerCollision();
                PlayerHit();

                if (currentState == SwineBossState.Normal)
                {
                    currentAnimation = SwineBossAnimation.idle;

                    if (player.position.Y + player.bounds.Height > bounds.Y)
                    {
                        currentAnimation = SwineBossAnimation.run;
                        currentState = SwineBossState.Aggressive;
                    }
                }
                if (currentState == SwineBossState.Aggressive)
                {
                    if (position.X < player.position.X)
                        direction = 1;
                    else
                        direction = -1;

                    MovementUpdate(elapsed);

                    bounds.X = (int)position.X - direction;
                    bounds.Y = (int)position.Y;

                    SideCollision(elapsed);

                    if (CheckPlayerDistance() < 170 && position.Y < player.position.Y)
                    {
                        currentAnimation = SwineBossAnimation.attack;
                        currentState = SwineBossState.Attack;
                    }
                    if (player.position.Y + player.bounds.Height < bounds.Y)
                        currentState = SwineBossState.Normal;
                }
                else if (currentState == SwineBossState.Attack) // TODO direct counter attack (run away after attack)
                {
                    if (currentFrame == totalFrames)
                    {
                        currentAnimation = SwineBossAnimation.run;
                        currentState = SwineBossState.Aggressive;
                    }

                    if (direction == 1)
                        attackBounds.X = bounds.X + bounds.Width;
                    else
                        attackBounds.X = bounds.X - attackBounds.Width;
                    attackBounds.Y = bounds.Y;
                }
                if (currentState == SwineBossState.Counter)
                {
                    if (currentFrame == totalFrames)
                    {
                        currentAnimation = SwineBossAnimation.run;
                        currentState = SwineBossState.Scared;
                    }

                    if (direction == 1)
                        attackBounds.X = bounds.X + bounds.Width;
                    else
                        attackBounds.X = bounds.X - attackBounds.Width;
                    attackBounds.Y = bounds.Y;
                }
                if (currentState == SwineBossState.Scared)
                {
                    if (position.X < player.position.X)
                        direction = -1;
                    else
                        direction = 1;

                    if (SideCollision(elapsed))
                    {
                        currentState = SwineBossState.Aggressive;
                        currentAnimation = SwineBossAnimation.run;
                    }
                    else
                        MovementUpdate(elapsed);
                }
                if (currentState == SwineBossState.Hit)
                {
                    if (position.X < player.position.X)
                        direction = 1;
                    else
                        direction = -1;

                    if (currentFrame == totalFrames)
                    {
                        currentState = SwineBossState.Counter;
                        currentAnimation = SwineBossAnimation.attack;
                    }
                }

                if (health <= 0)
                {
                    currentState = SwineBossState.Die;
                    currentAnimation = SwineBossAnimation.die;
                    AnimationUpdate();
                    invulnerable = true;
                    interval = 38f;
                }
                if (health <= 50 && health > 20 && velocity.X != 400)
                {
                    velocity.X = 400;
                    interval -= 4f;
                }
                else if (health <= 20 && velocity.X != 440)
                {
                    velocity.X = 440;
                    interval -= 4f;
                }

                FallingUpdate(elapsed);
                DownCollision(elapsed);
                AnimationUpdate();

                base.Update(gameTime);
            }
            else if (currentState == SwineBossState.Die)
            {
                if (currentAnimation != SwineBossAnimation.dead)
                {
                    if (currentFrame == totalFrames)
                    {
                        currentAnimation = SwineBossAnimation.dead;
                        Globals.Collision.RemoveAt(Globals.Collision.Count - 1);
                    } // TODO (dying) if dead door unlock and key cutscene
                }

                AnimationUpdate();
                base.Update(gameTime);
            }
            Debug.text = " Health:{" + health + "}";
        }

        protected override void PlayerCollision()
        {
            if (bounds.Intersects(player.swordBounds) && currentState != SwineBossState.Hit)
            {
                currentState = SwineBossState.Hit;
                currentAnimation = SwineBossAnimation.hit;
                currentFrame = 1;

                base.PlayerCollision();
            }
        }

        protected override void PlayerHit()
        {
            if (player.bounds.Intersects(attackBounds) && (currentState == SwineBossState.Attack || currentState == SwineBossState.Counter) && currentFrame > 23)
                player.GetHit(3);

            base.PlayerHit();
        }

        public void AnimationUpdate()
        {
            if (currentAnimation != previousAnimation || direction != previousDirection)
            {
                switch (currentAnimation)
                {
                    case SwineBossAnimation.idle:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 340, 250), new Vector2(90, 58), 79, 9);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 340, 250), new Vector2(20, 58), 79, 9);
                        }
                        texture = _textures[0]; // IDLE
                        break;
                    case SwineBossAnimation.run:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 340, 250), new Vector2(90, 56), 25, 5);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 340, 250), new Vector2(30, 56), 25, 5);
                        }
                        texture = _textures[1]; // RUN
                        break;
                    case SwineBossAnimation.attack:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 365, 260), new Vector2(80, 66), 33, 6);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 365, 260), new Vector2(68, 66), 33, 6);
                        }
                        texture = _textures[2]; // ATTACK
                        if (currentState == SwineBossState.Counter)
                            currentFrame = 23;
                        break;
                    case SwineBossAnimation.hit:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 340, 250), new Vector2(84, 52), 10, 4);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 340, 250), new Vector2(32, 52), 10, 4);
                        }
                        texture = _textures[3]; // HIT
                        break;
                    case SwineBossAnimation.die:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 575, 240), new Vector2(340, 44), 42, 7);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 575, 240), new Vector2(32, 44), 42, 7);
                        }
                        texture = _textures[4]; // DIE
                        break;
                    case SwineBossAnimation.dead:
                        if (direction == 1)
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                            ChangeAnimation(new Rectangle(0, 0, 210, 250), new Vector2(174, 55), 42, 7);
                        }
                        else
                        {
                            spriteEffect = SpriteEffects.None;
                            ChangeAnimation(new Rectangle(0, 0, 210, 250), new Vector2(-172, 55), 42, 7);
                        }
                        texture = _textures[5]; // DEAD
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

                if (currentState == SwineBossState.Attack || currentState == SwineBossState.Counter)
                    spriteBatch.Draw(TextureObjects.RectTex(attackBounds, Color.Gray * 0.6f), attackBounds, Color.White);
            }
        }
    }
}
