using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Potat.Entities;

namespace Potat {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Scene map;

        public Game() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            Window.AllowAltF4 = true;
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
            

            Window.Title = "Potat";

            base.Initialize();
        }
        
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            map = new Scene(this);
        }
        
        protected override void UnloadContent() {
        }
        
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            map.Tick(this, gameTime);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            map.Render(spriteBatch, this, gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
