using Bomb_Finder.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomb_Finder
{
    public enum GameState
    {
        Options,
        Game,
        GameOver
    }

    public static class Globals
    {
        public static Texture2D DebugRed;
        public static SpriteFont GameFont;

        public static MouseState MouseState;
        public static MouseState OldMouseState;

        public static GameState GameState = GameState.Game;

        public static Rectangle WindowBounds;

        public static SpriteBatch SpriteBatch;
        public static ContentManager Content;

        public static GameManager GameManager;
        public static UIManager UIManager;

        public static Stopwatch GameStopwatch = new Stopwatch();
        public static Random Random { get; } = new Random();

        public static Vector2 GetOriginOfTexture(Texture2D texture)
        {
            return new Vector2(texture.Width, texture.Height) / 2;
        }

        public static Rectangle GetSourceRectangleOfTexture(Texture2D texture)
        {
            return new Rectangle(0, 0, texture.Width, texture.Height);
        }
    }
}
