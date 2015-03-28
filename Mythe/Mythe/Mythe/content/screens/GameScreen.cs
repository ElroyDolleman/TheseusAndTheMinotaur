using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Mythe.content.characters;
using Mythe.content.enemy;
using Mythe.content.level;
using Mythe.dependencies;
using Mythe.resources;

namespace Mythe.content.screens
{
    // TODO:
    //  Minitour
    //  Startscreen (swf) & (Button class)
    //  BGM & SoundEffects
    //  Player health & Healing room
    //  Locked doors

    class GameScreen : Screen
    {
        public static bool LoadLevel = false;
        bool gameover = false;

        Texture2D pauseScreen = Globals.Content.Load<Texture2D>(Assets.PAUSE_SCREEN);
        bool pause = false;

        NormalPlayer player;
        Level level;
        
        Camera camera;

        Vector2 backpos = new Vector2(0,0);

        public GameScreen()
        {
            player = new NormalPlayer(new Vector2(96, 270));
            level = new Level(new Vector2(0, 0));

            player.offset = new Vector2(-16, -8);
            player.bounds = new Rectangle(0, 0, 64, 150);

            camera = new Camera();
            camera.Update(player.position);
            EnemyUpdate();

            UI.healthbar.srcRect = new Rectangle(0, 0, 518, 30);
            UI.healthbar.frameRowsX = 4;
            UI.healthbar.totalFrames = 16;
        }

        KeyboardState newKeyState, oldKeyState;

        public override void Update(GameTime gameTime)
        {
            newKeyState = Keyboard.GetState();
            if (!pause)
            {
                if (!gameover)
                {
                    level.Update(gameTime);

                    foreach (Enemy enemy in Globals.enemys)
                    {
                        enemy.Update(gameTime);

                        if (enemy.dead)
                            break;
                    }

                    if (!LoadLevel)
                    {
                        if (player.currentState == State.onGround)
                            level.DoorUpdate(player.bounds);

                        if (!LoadLevel)
                            UpdatePlayer(gameTime);
                        else
                            FadeUpdate(gameTime);
                    }
                    else
                        FadeUpdate(gameTime);
                }

                if (player.health <= 0)
                {
                    UI.healthbar.currentFrame = 1;
                    gameover = true;
                    player.UpdateDead();
                    ScreenManager.SwitchScreen(new StartScreen());
                }
                else
                    UI.healthbar.currentFrame = player.health;

                UI.healthbar.UpdateFrameOnly();

                if (newKeyState.IsKeyDown(Keys.Escape) && oldKeyState.IsKeyUp(Keys.Escape))
                {
                    pause = true;
                }
            }
            else
            {
                if (newKeyState.IsKeyDown(Keys.Escape) && oldKeyState.IsKeyUp(Keys.Escape))
                {
                    pause = false;
                }
            }
            oldKeyState = newKeyState;
        }

        Rectangle ScreenRect;
        Texture2D FadeRectTex;
        public static Door currentDoor;
        float _fadeTimer = 0;
        float interval = 0.034f;

        public void FadeUpdate(GameTime gameTime)
        {
            ScreenRect = new Rectangle((int)camera.position.X, (int)camera.position.Y, Globals.SCREEN_WIDTH, Globals.SCREEN_HEIGHT);
            _fadeTimer += interval;
            FadeRectTex = TextureObjects.RectTex(ScreenRect, Color.Black * _fadeTimer);

            if (_fadeTimer >= 1)// && currentDoor.destIndex != level.index)
            {
                player.position = currentDoor.destPos;
                level.CreateLevel(currentDoor.destIndex);

                EnemyUpdate();

                UpdatePlayer(gameTime);
                ScreenRect = new Rectangle((int)camera.position.X, (int)camera.position.Y, Globals.SCREEN_WIDTH, Globals.SCREEN_HEIGHT);

                Globals.Respawn = player.position;

                _fadeTimer = 1;
                interval = -interval;
            }
            else if (_fadeTimer <= 0 && currentDoor.destIndex == level.index)
            {
                _fadeTimer = 0;
                interval = -interval;
                LoadLevel = false;
            }
        }

        public void EnemyUpdate()
        {
            foreach (Enemy enemy in Globals.enemys)
            {
                enemy.player = player;
            }
        }

        public void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);
            CameraUpdate();

            if (player.position.Y > level.Height)
            {
                player.position = Globals.Respawn;
                player.Update(gameTime);
            }
        }

        public void CameraUpdate()
        {
            Vector2 pos = new Vector2(player.position.X + player.bounds.Width / 2 - Globals.SCREEN_WIDTH / 2, player.position.Y + player.bounds.Height / 2 - Globals.SCREEN_HEIGHT / 2);

            if (pos.X < 0)
                pos.X = 0;
            if (pos.X > level.Width - Globals.SCREEN_WIDTH)
                pos.X = level.Width - Globals.SCREEN_WIDTH;
            if (pos.Y < 0)
                pos.Y = 0;
            if (pos.Y > level.Height - Globals.SCREEN_HEIGHT)
                pos.Y = level.Height - Globals.SCREEN_HEIGHT;

            camera.Update(pos);
            Globals.ScreenPosition = pos;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);

            level.DrawBackground(spriteBatch, camera.position);
            player.Draw(spriteBatch);

            foreach (Enemy enemy in Globals.enemys)
                enemy.Draw(spriteBatch);

            level.DrawForeground(spriteBatch, camera.position);
            UI.DrawHealthbar(spriteBatch, camera.position);

            // fade in effect
            if (LoadLevel)
                spriteBatch.Draw(FadeRectTex, ScreenRect, Color.White);

            if (pause)
                spriteBatch.Draw(pauseScreen, camera.position, Color.White);

            // debug mode
            if (Debug.debugMode)
            {
                // vertical line
                TextureObjects.DrawLine(spriteBatch, Color.White * 0.5f, new Vector2(camera.position.X + Globals.SCREEN_WIDTH / 2, camera.position.Y),
                                                                             new Vector2(camera.position.X + Globals.SCREEN_WIDTH / 2, camera.position.Y + Globals.SCREEN_HEIGHT), 3);
                // horizontal line
                TextureObjects.DrawLine(spriteBatch, Color.White * 0.5f, new Vector2(camera.position.X, camera.position.Y + Globals.SCREEN_HEIGHT / 2),
                                                                             new Vector2(camera.position.X + Globals.SCREEN_WIDTH, camera.position.Y + Globals.SCREEN_HEIGHT / 2), 3);

                Debug.Draw(spriteBatch, camera.position);
            }

            spriteBatch.End();
        }
    }
}
