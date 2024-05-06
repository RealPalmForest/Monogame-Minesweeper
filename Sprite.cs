using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomb_Finder
{
    public class Sprite
    {
        public bool ShowBoundsRect { get; set; } = false;

        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }

        protected Rectangle Bounds { get; private set; }
        protected Vector2 Origin { get; private set; }
        protected Rectangle TextureSourceRectangle { get; private set; }

        public Sprite(Texture2D texture, Vector2 position, Rectangle sourceRect) 
        {
            Position = position;
            Texture = texture;
            TextureSourceRectangle = sourceRect;

            UpdateOriginAndBounds();
        }
        public Sprite(Texture2D texture, Vector2 position)
        {
            Position = position;
            Texture = texture;
            TextureSourceRectangle = Globals.GetSourceRectangleOfTexture(texture);

            UpdateOriginAndBounds();
        }


        public Rectangle GetBounds() { return Bounds; }


        private void UpdateOriginAndBounds()
        {
            Origin = new Vector2(TextureSourceRectangle.Width, TextureSourceRectangle.Height) / 2;
            Bounds = new Rectangle((Position - Origin).ToPoint(), new Point(TextureSourceRectangle.Width, TextureSourceRectangle.Height));
        }

        public void SetTextureSourceRect(Rectangle sourceRect)
        {
            TextureSourceRectangle = sourceRect;
            UpdateOriginAndBounds();
        }


        public virtual void Draw()
        {
            Globals.SpriteBatch.Draw(Texture, Position, TextureSourceRectangle, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);

            if(ShowBoundsRect)
            {
                Globals.SpriteBatch.Draw(Globals.DebugRed, Bounds, Color.White);
            }
        }
    }
}
