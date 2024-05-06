using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomb_Finder
{
    public class Tile : Sprite
    {
        public bool isBomb { get; set; } = false;
        public bool isFlagged { get; private set; } = false;
        public bool isCovered { get; private set; } = true;

        public int surroundingBombs { get; set; } = 0;

        public string Text { get; private set; } = "";
        public bool ShowText { get; set; } = false;
        private Vector2 TextOrigin = Vector2.Zero;
        public Color TextColor { get; set; } = Color.White;

        public Point TilePosition { get; set; }

        public Tile(Texture2D texutre, Vector2 position, bool isBomb) : base(texutre, position) { this.isBomb = isBomb; }
        public Tile(Texture2D texutre, Vector2 position, Rectangle sourceRect, bool isBomb) : base(texutre, position, sourceRect) { this.isBomb = isBomb; }


        public void UpdateTexture()
        {
            List<Point> list = new List<Point>();

            // Exclude points with x = 5
            list = list.Where(p => p.X != 5).ToList();


            if (isCovered)
            {
                if(isFlagged)
                {
                    SetTextureSourceRect(new Rectangle(32, 0, 32, 32));
                    return;
                }

                SetTextureSourceRect(new Rectangle(0, 0, 32, 32));
            }
            else
            {
                SetTextureSourceRect(new Rectangle(64, 0, 32, 32));
            }
        }

        public void SetText(string text, Color color)
        {
            Text = text;
            TextColor = color;
            GetTextOrigin(text);
        }
        public void SetText(string text)
        {
            Text = text;
            GetTextOrigin(text);
        }

        private void GetTextOrigin(string text)
        {
            TextOrigin = Globals.GameFont.MeasureString(text) / 2;
        }

        public void Open()
        {
            if(isBomb)
            {
                // Lose
                return;
            }

            isCovered = false;

            UpdateTexture();

            switch (surroundingBombs)
            {
                
                case 1:
                    TextColor = Color.Blue;
                    break;
                case 2:
                    TextColor = Color.Green;
                    break;
                case 3:
                    TextColor = Color.DarkRed;
                    break;
                case 4:
                    TextColor = Color.DarkBlue;
                    break;
                case 5:
                    TextColor = new Color(Color.Maroon.R - 60, Color.Maroon.G - 60, Color.Maroon.B - 60);
                    break;
                case 6:
                    TextColor = Color.Teal;
                    break;
                case 7:
                    TextColor = Color.Black;
                    break;
                case 8:
                    TextColor = new Color(Color.DarkGray.R - 90, Color.DarkGray.G - 90, Color.DarkGray.B - 90);
                    break;
                
                default:
                    TextColor = Color.White;
                    break;
            }

            SetText(surroundingBombs.ToString());

            if(surroundingBombs > 0)
                ShowText = true;
        }

       
        public void ToggleFlag()
        {
            isFlagged = !isFlagged;
            UpdateTexture();
        }

        public void CountSurroundingBombs()
        {

        }


        public override void Draw()
        {
            base.Draw();

            //if(isBomb)
            //   Globals.SpriteBatch.Draw(Globals.DebugRed, Bounds, new Color(Color.White, 0.01f));

            if (ShowText)
                Globals.SpriteBatch.DrawString(Globals.GameFont, Text, new Vector2(Position.X, Position.Y + 1.5f), TextColor, 0f, TextOrigin, 0.65f, SpriteEffects.None, 0f);
        }
    }
}
