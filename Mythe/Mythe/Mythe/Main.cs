using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mythe.resources;
using Mythe.dependencies;
using Mythe.content.screens;

namespace Mythe // latest update 4/25/2013 2:10 // latest back up 4/25/2013 2:10
{
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ScreenManager screenManager;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = Globals.SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = Globals.SCREEN_HEIGHT;

            Globals.Content = this.Content;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Globals.graphicsDevice = GraphicsDevice;

            screenManager = new ScreenManager(new StartScreen());

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        //KeyboardState keyState;
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            screenManager.Update(gameTime);

            // debug switch
            //KeyboardState state = Keyboard.GetState();
            //if (state.IsKeyDown(Keys.Space) && keyState.IsKeyUp(Keys.Space))
                //Debug.debugMode = !Debug.debugMode;
            //keyState = state;

            base.Update(gameTime);
        }

        MouseState mouseState;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            screenManager.Draw(spriteBatch);

            if (Debug.debugMode)
            {
                Debug.CreateMouseRect(spriteBatch, mouseState);
                mouseState = Mouse.GetState();
            }

            base.Draw(gameTime);
        }
    }
}
