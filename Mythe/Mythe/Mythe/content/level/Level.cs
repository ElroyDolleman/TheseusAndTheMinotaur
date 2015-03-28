using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Mythe.resources;
using Mythe.dependencies;
using Mythe.content.screens;

namespace Mythe.content.level
{
    class Level
    {
        public Vector2 position;

        public float Width, Height;
        public int index = 0;

        public Song Bgm;

        public Level(Vector2 position)
        {
            this.position = position;

            Globals.LockedDoors = new List<LockedDoor>();
            //Globals.LockedDoors.Add(new LockedDoor(Globals.Content.Load<Texture2D>(""), new Vector2(), 2, 5));

            CreateLevel(1);
        }

        public void CreateLevel(int level)
        {
            Rooms.SelectRoom(level);
            Width = Rooms.Width;
            Height = Rooms.Height;

            if (level < 6 && (index >= 6 || index == 0))
            {
                Bgm = Globals.Content.Load<Song>(Assets.MUSIC_NORMAL_BGM);
                MediaPlayer.Play(Bgm);
                MediaPlayer.IsRepeating = true;
                
            }
            else if (level >= 6 && index < 6)
            {
                Bgm = Globals.Content.Load<Song>(Assets.MUSIC_HEALROOM_BGM);
                MediaPlayer.Play(Bgm);
                MediaPlayer.IsRepeating = true;
            }

            index = level;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Sprite back in Globals.background)
                if (back.totalFrames > 1)
                    back.Update(gameTime);
        }

        public void DoorUpdate(Rectangle bounds)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            foreach (Door door in Globals.Doors)
            {
                if (bounds.Left > door.bounds.Left && bounds.Right < door.bounds.Right && bounds.Intersects(door.bounds) && (keyboardState.IsKeyDown(Keys.W) || !door.needButton))
                {
                    GameScreen.LoadLevel = true;
                    GameScreen.currentDoor = door;
                }
            }

            foreach (LockedDoor door in Globals.LockedDoors)
            {
                if (bounds.Left > door.bounds.Left && bounds.Right < door.bounds.Right && bounds.Intersects(door.bounds) && (keyboardState.IsKeyDown(Keys.W)))
                {
                    // TODO next level
                }
            }
        }

        Texture2D paralax = Globals.Content.Load<Texture2D>(Assets.PARALAX_1);

        public void DrawBackground(SpriteBatch spriteBatch, Vector2 camera)
        {
            foreach (Paralax paralax in Globals.paralax)
                paralax.Draw(spriteBatch, camera);

            foreach(Sprite sprite in Globals.background)
                spriteBatch.Draw(sprite.texture, sprite.position, sprite.srcRect, Color.White);            

            foreach (Door door in Globals.Doors)
                door.Draw(spriteBatch);

            foreach (LockedDoor door in Globals.LockedDoors)
                if (door.levelIndex == index)
                    door.Draw(spriteBatch);

            if (Debug.debugMode)
            {
                for (int i = 0; i < Globals.Collision.Count; i++)
                    spriteBatch.Draw(TextureObjects.RectTex(Globals.Collision[i], Color.Black), Globals.Collision[i], Color.White);
                for (int i = 0; i < Globals.Platforms.Count; i++)
                {
                    spriteBatch.Draw(TextureObjects.RectTex(Globals.Platforms[i], Color.Black), Globals.Platforms[i], Color.White);
                    spriteBatch.DrawString(Debug.font, "" + (i + 1), new Vector2(Globals.Platforms[i].X, Globals.Platforms[i].Y), Color.White);
                }
                for (int i = 0; i < Globals.Grabpoints.Count; i++)
                {
                    spriteBatch.Draw(TextureObjects.RectTex(Globals.Grabpoints[i], Color.Red * 0.8f), Globals.Grabpoints[i], Color.White);
                    spriteBatch.DrawString(Debug.font, "" + (i + 1), new Vector2(Globals.Grabpoints[i].X, Globals.Grabpoints[i].Y), Color.White);
                }
            }
        }

        public void DrawForeground(SpriteBatch spriteBatch, Vector2 camera)
        {
            if (!Debug.debugMode)
            {
                foreach (Sprite sprite in Globals.foreground)
                {
                    spriteBatch.Draw(sprite.texture, sprite.position, Color.White);
                }

                spriteBatch.Draw(Rooms.frame, camera, Color.White);
            }
        }
    }
}
