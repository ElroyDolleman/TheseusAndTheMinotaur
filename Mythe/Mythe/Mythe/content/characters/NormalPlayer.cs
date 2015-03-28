using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Mythe.resources;
using Mythe.dependencies;
using Mythe.content.characters.weapons;

namespace Mythe.content.characters
{
    class NormalPlayer : Player
    {
        private List<Texture2D> textures = new List<Texture2D>();

        public NormalPlayer(Vector2 position)
            : base(Globals.Content.Load<Texture2D>(Assets.PLAYER_IDLE_RIGHT), position)
        {
            left = Keys.A;
            right = Keys.D;
            jump = Keys.W;
            attack = Keys.S;

            velocity = new Vector2(260, -500);

            gravity = 22f;
            maxFallSpeed = -velocity.Y * 2;
            interval = 16f;

            bounds = new Rectangle(0, 0, 110, 180);
            swordBounds = new Rectangle(-1800, 0, 24, 120);

            power = 10;
            health = 16;

            // textures
            textures.Add(Globals.Content.Load<Texture2D>(Assets.PLAYER_IDLE_RIGHT));
            textures.Add(Globals.Content.Load<Texture2D>(Assets.PLAYER_IDLE_LEFT)); //
            textures.Add(Globals.Content.Load<Texture2D>(Assets.PLAYER_RUN_RIGHT));
            textures.Add(Globals.Content.Load<Texture2D>(Assets.PLAYER_RUN_LEFT)); //
            textures.Add(Globals.Content.Load<Texture2D>(Assets.PLAYER_JUMP_RIGHT));
            textures.Add(Globals.Content.Load<Texture2D>(Assets.PLAYER_JUMP_LEFT)); //
            textures.Add(Globals.Content.Load<Texture2D>(Assets.PLAYER_THROW_RIGHT));
            textures.Add(Globals.Content.Load<Texture2D>(Assets.PLAYER_THROW_LEFT)); //
            textures.Add(Globals.Content.Load<Texture2D>(Assets.PLAYER_ATTACK_RIGHT)); 
            textures.Add(Globals.Content.Load<Texture2D>(Assets.PLAYER_ATTACK_LEFT)); //
            textures.Add(Globals.Content.Load<Texture2D>(Assets.PLAYER_HIT_RIGHT));
            textures.Add(Globals.Content.Load<Texture2D>(Assets.PLAYER_HIT_LEFT)); //

            // hookshot
            hookshot = new HookShot(position, new Vector2(8, 76), 26, 18, 6, 490);
            hookshot.textures.Add(Globals.Content.Load<Texture2D>(Assets.HOOKSHOT_RIGHT));
            hookshot.textures.Add(Globals.Content.Load<Texture2D>(Assets.HOOKSHOT_LEFT));

            hookshot.srcRect = new Rectangle(0, 0, 490, 90);

            hookshot.origin = new Vector2(432, 18);
            hookshot.interval = 48f;
            hookshot.totalFrames = 30;

            hookshot.degreeOffset = 90 + 180;

            ChangeAnimation(Animation.idleRight);
        }

        public override void Update(GameTime gameTime)
        {
            if (newKeyState.IsKeyUp(Keys.X) || !Debug.debugMode)
                base.Update(gameTime);

            if (currentFrame == totalFrames && (currentAnimation == Animation.jumpRight || currentAnimation == Animation.jumpLeft))
                currentFrame = totalFrames;
            else
                base.AnimationUpdate(gameTime);

            if (newKeyState.IsKeyDown(Keys.Z) && Debug.debugMode)
                fallSpeed = -80;
            if (newKeyState.IsKeyDown(Keys.X) && Debug.debugMode)
                fallSpeed = 0;
        }

        protected override void UpdateHookShot(GameTime gameTime)
        {
            if (flipped)
            {
                hookshot.direction = -1;
                hookshot.origin = new Vector2(432, 18);
                hookshot.offset = new Vector2(2, 74);
                hookshot.degreeOffset = 270;
            }
            else
            {
                hookshot.direction = 1;
                hookshot.origin = new Vector2(48, 18);
                hookshot.offset = new Vector2(8, 78);
                hookshot.degreeOffset = 90;
            }

            base.UpdateHookShot(gameTime);
        }

        protected override void UpdateAttack()
        {
            if (currentState == State.attack && currentFrame == 18)
            {
                if (!flipped)
                    swordBounds.X = bounds.X + bounds.Width + 70;
                else
                    swordBounds.X = bounds.X - 110;

                swordBounds.Y = bounds.Y;
            }

            base.UpdateAttack();
        }

        protected override void ChangeAnimation(Animation animation)
        {
            if (flipped)
                animation += 1;

            if (texture != textures[(int)animation])
            {
                switch (animation)
                {
                    case Animation.idleRight:
                        totalFrames = 36;
                        currentFrame = 1;
                        frameRowsX = 6;
                        frameRowsY = 6;
                        interval = 18f;

                        srcRect = new Rectangle(0, 0, 140, 180);
                        offset = new Vector2((srcRect.Width / 2 - bounds.Width / 2) - 6, (srcRect.Height - bounds.Height) - 10);
                        break;
                    case Animation.idleLeft:
                        totalFrames = 36;
                        currentFrame = 1;
                        frameRowsX = 6;
                        frameRowsY = 6;
                        interval = 18f;

                        srcRect = new Rectangle(0, 0, 160, 175);
                        offset = new Vector2((srcRect.Width / 2 - bounds.Width / 2) + 40, (srcRect.Height - bounds.Height) - 8);
                        break;
                    case Animation.runRight:                    
                        totalFrames = 24;
                        frameRowsX = 5;
                        interval = 18f;

                        if (animation == Animation.runRight && animation == Animation.runLeft)
                            currentFrame = 1;

                        srcRect = new Rectangle(0, 0, 170, 181);
                        offset = new Vector2((srcRect.Width / 2 - bounds.Width / 2) + 18, (srcRect.Height - bounds.Height) - 10);
                        break;
                    case Animation.runLeft:
                        totalFrames = 24;
                        frameRowsX = 5;
                        interval = 18f;

                        if (animation == Animation.runRight && animation == Animation.runLeft)
                            currentFrame = 1;

                        srcRect = new Rectangle(0, 0, 200, 180);
                        offset = new Vector2((srcRect.Width / 2 - bounds.Width / 2) + 14, (srcRect.Height - bounds.Height) - 6);
                        break;
                    case Animation.jumpRight:
                        totalFrames = 36;
                        frameRowsX = 7;
                        frameRowsY = 6;
                        interval = 18f;

                        if (animation == Animation.jumpRight && animation == Animation.jumpLeft)
                            currentFrame = 1;

                        srcRect = new Rectangle(0, 0, 130, 200);
                        offset = new Vector2(srcRect.Width / 2 - bounds.Width / 2, (srcRect.Height - bounds.Height) - 10);
                        break;
                    case Animation.jumpLeft:
                        totalFrames = 36;
                        frameRowsX = 7;
                        frameRowsY = 6;
                        interval = 18f;

                        if (animation == Animation.jumpRight && animation == Animation.jumpLeft)
                            currentFrame = 1;

                        srcRect = new Rectangle(0, 0, 170, 195);
                        offset = new Vector2((srcRect.Width / 2 - bounds.Width / 2) + 30, (srcRect.Height - bounds.Height) - 10);
                        break;
                    case Animation.throwRight:
                        totalFrames = 30;
                        frameRowsX = 6;
                        frameRowsY = 5;
                        currentFrame = 1;
                        interval = 18f;

                        srcRect = new Rectangle(0, 0, 90, 175);
                        offset = new Vector2((srcRect.Width / 2 - bounds.Width / 2), (srcRect.Height - bounds.Height) - 10);
                        break;
                    case Animation.throwLeft:
                        totalFrames = 30;
                        frameRowsX = 6;
                        frameRowsY = 5;
                        currentFrame = 1;
                        interval = 18f;

                        srcRect = new Rectangle(0, 0, 90, 175);
                        offset = new Vector2((srcRect.Width / 2 - bounds.Width / 2), (srcRect.Height - bounds.Height) - 10);
                        break;
                    case Animation.attackRight:
                        totalFrames = 26;
                        frameRowsX = 6;
                        frameRowsY = 5;
                        currentFrame = 1;
                        interval = 18f;

                        srcRect = new Rectangle(0, 0, 200, 195);
                        offset = new Vector2((srcRect.Width / 2 - bounds.Width / 2) - 30, (srcRect.Height - bounds.Height) - 8);
                        break;
                    case Animation.attackLeft:
                        totalFrames = 28;
                        frameRowsX = 6;
                        frameRowsY = 5;
                        currentFrame = 1;
                        interval = 18f;

                        srcRect = new Rectangle(0, 0, 200, 180);
                        offset = new Vector2((srcRect.Width / 2 - bounds.Width / 2) + 46, (srcRect.Height - bounds.Height) - 6);
                        break;
                    case Animation.hitRight:
                        totalFrames = 10;
                        frameRowsX = 4;
                        frameRowsY = 3;
                        currentFrame = 1;
                        interval = 18f;

                        srcRect = new Rectangle(0, 0, 117, 173);
                        offset = new Vector2((srcRect.Width / 2 - bounds.Width / 2) + 2, (srcRect.Height - bounds.Height) - 10);
                        break;
                    case Animation.hitLeft:
                        totalFrames = 10;
                        frameRowsX = 4;
                        frameRowsY = 3;
                        currentFrame = 1;
                        interval = 18f;

                        srcRect = new Rectangle(0, 0, 170, 175);
                        offset = new Vector2((srcRect.Width / 2 - bounds.Width / 2) + 36, (srcRect.Height - bounds.Height) - 10);
                        break;
                } // end switch
            } // end if

            texture = textures[(int)animation];
            currentAnimation = animation;
        }
    }
}
