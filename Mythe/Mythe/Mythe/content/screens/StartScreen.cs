using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mythe.resources;
using Mythe.dependencies;

namespace Mythe.content.screens
{
    class StartScreen : Screen
    {
        KeyboardState newKeyState, oldKeyState;

        Video video;
        VideoPlayer player;
        Texture2D videoTexture;

        Button startButton;

        public StartScreen()
        {
            background = Globals.Content.Load<Texture2D>(Assets.START_SCREEN);

            video = Globals.Content.Load<Video>("wmv_loginscreen");
            player = new VideoPlayer();

            startButton = new Button(Globals.Content.Load<Texture2D>(Assets.START_BUTTON), new Vector2(1280 / 2 - 110, 720 / 2 - 110), new Rectangle(0,0,220,220));
            startButton.srcRect = new Rectangle(0, 0, 220, 220);
            startButton.frameRowsX = 5;
            startButton.totalFrames = 20;
            startButton.interval = 20f;
        }

        public override void Update(GameTime gameTime)
        {
            newKeyState = Keyboard.GetState();

            if (startButton.currentFrame == startButton.totalFrames)
                ScreenManager.SwitchScreen(new GameScreen());

            if (player.State == MediaState.Stopped)
            {
                player.IsLooped = true;
                player.Play(video);
            }

            startButton.Update(gameTime);

            base.Update(gameTime);

            oldKeyState = newKeyState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Globals.graphicsDevice.Clear(Color.CornflowerBlue);

            // Only call GetTexture if a video is playing or paused
            if (player.State != MediaState.Stopped)
                videoTexture = player.GetTexture();

            // Drawing to the rectangle will stretch the 
            // video to fill the screen
            Rectangle screen = new Rectangle(Globals.graphicsDevice.Viewport.X,
                Globals.graphicsDevice.Viewport.Y,
                Globals.graphicsDevice.Viewport.Width,
                Globals.graphicsDevice.Viewport.Height);

            // Draw the video, if we have a texture to draw.
            if (videoTexture != null)
                spriteBatch.Draw(videoTexture, screen, Color.White);

            startButton.Draw(spriteBatch);

            //base.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
