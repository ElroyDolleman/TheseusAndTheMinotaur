using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mythe.content.level
{
    class Paralax
    {
        public Texture2D texture;
        float speedX, speedY;

        public Paralax(Texture2D _texture, float _speedX = 0f, float _speedY = 0f)
        {
            texture = _texture;
            speedX = _speedX;
            speedY = _speedY;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, new Vector2(position.X * speedX, position.Y * speedY), Color.White);
        }
    }
}
