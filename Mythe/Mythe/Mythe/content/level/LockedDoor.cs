using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mythe.dependencies;
using Mythe.resources;

namespace Mythe.content.level
{
    class LockedDoor : Sprite
    {
        public Rectangle bounds;
        public Vector2 destPos;

        public int levelIndex;
        public int destIndex;
        public bool unlocked = false;

        public LockedDoor(Texture2D _texture,Vector2 destPos, int _levelIndex, int _destIndex)
            : base(_texture, new Vector2())
        {
            bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            
        }

        public override void Update(GameTime gameTime)
        {
            if (currentFrame < totalFrames && currentFrame > 1)
                base.Update(gameTime);

            else if (currentFrame == totalFrames)
                unlocked = true;
        }

        public void Unlock()
        {
            currentFrame++;
        }
    }
}
