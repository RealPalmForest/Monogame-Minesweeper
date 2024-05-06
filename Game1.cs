using Bomb_Finder.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bomb_Finder
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1040;
            _graphics.PreferredBackBufferHeight = 592;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            Globals.MouseState = Mouse.GetState();
            Globals.WindowBounds = GraphicsDevice.PresentationParameters.Bounds;
            Globals.Content = Content;
            Globals.SpriteBatch = _spriteBatch;
            Globals.GameManager = new GameManager();
            Globals.UIManager = new UIManager();

            Globals.GameManager.StartGame();

            Globals.DebugRed = Content.Load<Texture2D>("DebugRed");
            Globals.GameFont = Content.Load<SpriteFont>("UI/Font");
        }

        protected override void Update(GameTime gameTime)
        {
            Globals.OldMouseState = Globals.MouseState;
            Globals.MouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            Globals.GameManager.Update();
            Globals.UIManager.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            Globals.GameManager.Draw();
            Globals.UIManager.Draw();

            _spriteBatch.End();
        }
    }
}
