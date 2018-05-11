using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Potat.Extensions;
using Potat.Tiles;

namespace Potat {
    public class Scene {
        SpriteFont OpenSans;

        // OBJECTS
        Rectangle camera;
        Rectangle player;
        float playerSpeed = 100;
        Map map;

        // RENDER POSITIONS
        Vector2 MapPosition => new Vector2(map.bounds.X - camera.X, map.bounds.Y - camera.Y);
        Vector2 PlayerPosition => new Vector2(player.X - camera.X, player.Y - camera.Y);

        bool debug;

        Keys[] prekeys = new Keys[0];

        public Scene(Game game) {
            OpenSans = game.Content.Load<SpriteFont>("fonts/OpenSans");

            camera = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);

            player = new Rectangle(0, 0, 100, 100);

            map = new Map(new Rectangle(0, 0, 1000, 1000), new IRenderable[,]{
                {new StoneTile(game), new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game)},
                {new StoneTile(game), new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game)},
                {new StoneTile(game), new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game)},
                {new StoneTile(game), new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game)},
                {new StoneTile(game), new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game)},
                {new StoneTile(game), new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game)},
                {new StoneTile(game), new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game),  new WoodTile(game)},
                {new WaterTile(game), new WaterTile(game), new WaterTile(game), new WaterTile(game), new WaterTile(game), new WaterTile(game), new WaterTile(game)}
            });
        }

        public void Tick(Game game, GameTime gameTime) {
            var kstate = Keyboard.GetState();

            // Keep camera size == to viewport size
            camera.Width = game.GraphicsDevice.Viewport.Width;
            camera.Height = game.GraphicsDevice.Viewport.Height;

            // MOVE PLAYER IF KEY DOWN
            if (kstate.IsKeyDown(Keys.LeftShift))
                playerSpeed = 500;
            else
                playerSpeed = 100;

            if (kstate.IsKeyDown(Keys.W))
                player.Y -= (int)(playerSpeed * gameTime.ElapsedGameTime.TotalSeconds);

            if (kstate.IsKeyDown(Keys.S))
                player.Y += (int)(playerSpeed * gameTime.ElapsedGameTime.TotalSeconds);

            if (kstate.IsKeyDown(Keys.A))
                player.X -= (int)(playerSpeed * gameTime.ElapsedGameTime.TotalSeconds);

            if (kstate.IsKeyDown(Keys.D))
                player.X += (int)(playerSpeed * gameTime.ElapsedGameTime.TotalSeconds);

            if (kstate.IsKeyDown(Keys.F3) && Array.IndexOf(prekeys, Keys.F3) == -1) {
                debug = !debug;
            }

            prekeys = kstate.GetPressedKeys();

            // KEEP PLAYER IN MAP5
            if (player.Bottom > map.bounds.Height) {
                player.Y = map.bounds.Height - player.Height;
            }
            if (player.Top < 0) {
                player.Y = 0;
            }

            if (player.Right > map.bounds.Width) {
                player.X = map.bounds.Width - player.Width;
            }
            if (player.Left < 0) {
                player.X = 0;
            }

            // CENTER CAMERA ON PLAYER
            camera.X = (player.X + (player.Width / 2)) - (camera.Width / 2);

            camera.Y = (player.Y + (player.Height / 2)) - (camera.Height / 2);

            // KEEP CAMERA ON MAP, NOT BLANK SPACE
            if (camera.Width > map.bounds.Width) {
                camera.X = (map.bounds.Width / 2) - (camera.Width / 2);
            } else {
                camera.X = Math.Max(camera.X, 0);
                camera.X = Math.Min(camera.X, map.bounds.Width - camera.Width);
            }

            if (camera.Height > map.bounds.Height) {
                camera.Y = (map.bounds.Height / 2) - (camera.Height / 2);
            } else {
                camera.Y = Math.Max(camera.Y, 0);
                camera.Y = Math.Min(camera.Y, map.bounds.Height - camera.Height);
            }

            // TICK MAP
            map.Tick(gameTime);
        }

        public void Render(SpriteBatch spriteBatch, Game game, GameTime gameTime) {
            Texture2D rect = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });

            // Draw map base
            spriteBatch.Draw(rect, new Rectangle((int)MapPosition.X, (int)MapPosition.Y, map.bounds.Width, map.bounds.Height), Color.White);

            // Draw map tiles
            bool black = true;
            bool preblack;
            for (int x = 0; x < map.bounds.Width; x += 200) {
                preblack = black;
                for (int y = 0; y < map.bounds.Height; y += 200) {
                    spriteBatch.Draw(rect, new Rectangle((int)MapPosition.X + x, (int)MapPosition.Y + y, 200, 200), black ? Color.Black : Color.White);
                    black = !black;
                }
                if (preblack == black)
                    black = !black;
            }

            // Draw map
            map.Render(new Rectangle((int)MapPosition.X, (int)MapPosition.Y, map.bounds.Width, map.bounds.Height), spriteBatch, gameTime);

            // Draw player
            spriteBatch.Draw(rect, new Rectangle((int)PlayerPosition.X, (int)PlayerPosition.Y, player.Width, player.Height), Color.Green);
            if (debug) {
                spriteBatch.DrawDebug(OpenSans, new[] {
                    $"The player is at ({player.X}, {player.Y}) on the map with a speed of {playerSpeed}",
                    $"The map is {map.bounds.Width} by {map.bounds.Height} and offset <{MapPosition.X}, {MapPosition.Y}>",
                    $"The camera can see from ({camera.X}, {camera.Y}) to ({camera.X + camera.Width}, {camera.Y + camera.Height}), a {camera.Width}x{camera.Height} area",
                    $"The user has {String.Join(", ", Keyboard.GetState().GetPressedKeys().Select(x => x.ToString()))} pressed down",
                    // TODO
                    //$"The map has {null} tiles, {null} visible and {null} offscreen: {null} of which solid tiles, {null} transparent, and {null} doorways",
                    //$"There are {null} entities in the map, {null} of which are moving, and {null} of witch with active pathfinding",
                    //$"There are {null} collisions happening in the map"
                    "F3 for less details"
                });
            } else {
                spriteBatch.DrawDebug(OpenSans, new[] {
                    $"({player.X}, {player.Y})",
                    "F3 for more details"
                });
            }
        }
    }
}
