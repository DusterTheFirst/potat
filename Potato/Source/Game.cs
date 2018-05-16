using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Gui;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Content;
using MonoGame.Extended.Tiled.Graphics;

namespace Potato {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Camera2D camera;
        private TiledMap map;
        private TiledMapRenderer mapRenderer;

        public Game() {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            base.Initialize();

            map = Content.Load<TiledMap>("maps/island/island");
            mapRenderer = new TiledMapRenderer(GraphicsDevice);

            camera = new Camera2D(new WindowViewportAdapter(Window, GraphicsDevice));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            var keyboardState = Keyboard.GetState();
            const float movementSpeed = 0.1f;

            //TODO: ADD PLAYER
            //TODO: POSITION CAMERA AROUND PLAYER
            //TODO: STOP CAMERA FROM GOING OVER THE MAP'S EDGES
            if (keyboardState.IsKeyDown(Keys.W))
                camera.Move(new Vector2(0, -movementSpeed) * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            if (keyboardState.IsKeyDown(Keys.S))
                camera.Move(new Vector2(0, movementSpeed) * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            if (keyboardState.IsKeyDown(Keys.A))
                camera.Move(new Vector2(-movementSpeed, 0) * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            if (keyboardState.IsKeyDown(Keys.D))
                camera.Move(new Vector2(movementSpeed, 0) * (float)gameTime.ElapsedGameTime.TotalMilliseconds);

            camera.Zoom = 2;

            mapRenderer.Update(map, gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Pink);
            
            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);

            spriteBatch.DrawRectangle(new RectangleF(0, 0, 100, 100), Color.Red);

            mapRenderer.Draw(map, camera.GetViewMatrix());

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
