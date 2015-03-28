using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mythe.resources;
using Mythe.dependencies;

namespace Mythe.content.level
{
    class Door : Sprite
    {
        public int destIndex;
        public Rectangle bounds;
        public Vector2 destPos;

        public bool needButton;

        public Door(Rectangle rect, int destIndex, Vector2 destPos, bool needButton = true)
            : base(TextureObjects.RectTex(rect, Color.OrangeRed * 0.7f), new Vector2(rect.X, rect.Y))
        {
            this.bounds = rect;
            this.needButton = needButton;

            this.destPos = destPos;
            this.destIndex = destIndex;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Debug.debugMode)
                base.Draw(spriteBatch);
        }
    }
}
