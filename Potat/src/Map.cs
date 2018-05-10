using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Potat.Extensions;

namespace Potat {
    public class Scene {
        SpriteFont OpenSans;

        // OBJECTS
        Rectangle camera;
        Rectangle player;
        float playerSpeed = 100;
        Rectangle map;

        // RENDER POSITIONS
        Vector2 MapPosition => new Vector2(map.X - camera.X, map.Y - camera.Y);
        Vector2 PlayerPosition => new Vector2(player.X - camera.X, player.Y - camera.Y);

        public Scene(Game game) {
            OpenSans = game.Content.Load<SpriteFont>("fonts/OpenSans");

            camera = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);

            player = new Rectangle(0, 0, 100, 100);

            map = new Rectangle(0, 0, 10000, 5000);
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

            // KEEP PLAYER IN MAP5
            if (player.Bottom > map.Height) {
                player.Y = map.Height - player.Height;
            }
            if (player.Top < 0) {
                player.Y = 0;
            }

            if (player.Right > map.Width) {
                player.X = map.Width - player.Width;
            }
            if (player.Left < 0) {
                player.X = 0;
            }

            // CENTER CAMERA ON PLAYER
            camera.X = (player.X + (player.Width / 2)) - (camera.Width / 2);

            camera.Y = (player.Y + (player.Height / 2)) - (camera.Height / 2);

            // KEEP CAMERA ON MAP, NOT BLANK SPACE
            if (camera.Width > map.Width) {
                camera.X = (map.Width / 2) - (camera.Width / 2);
            } else {
                camera.X = Math.Max(camera.X, 0);
                camera.X = Math.Min(camera.X, map.Width - camera.Width);
            }

            if (camera.Height > map.Height) {
                camera.Y = (map.Height / 2) - (camera.Height / 2);
            } else {
                camera.Y = Math.Max(camera.Y, 0);
                camera.Y = Math.Min(camera.Y, map.Height - camera.Height);
            }


        }

        public void Render(SpriteBatch spriteBatch, Game game, GameTime gameTime) {
            Texture2D rect = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });

            // Draw map base
            spriteBatch.Draw(rect, new Rectangle((int)MapPosition.X, (int)MapPosition.Y, map.Width, map.Height), Color.White);

            // Draw map tiles
            bool black = true;
            bool preblack;
            for (int x = 0; x < map.Width; x += 200) {
                preblack = black;
                for (int y = 0; y < map.Height; y += 200) {
                    spriteBatch.Draw(rect, new Rectangle((int)MapPosition.X + x, (int)MapPosition.Y + y, 200, 200), black ? Color.Black : Color.White);
                    black = !black;
                }
                if (preblack == black)
                    black = !black;
            }

            // Draw player
            spriteBatch.Draw(rect, new Rectangle((int)PlayerPosition.X, (int)PlayerPosition.Y, player.Width, player.Height), Color.Green);
			spriteBatch.DrawDebug(OpenSans, new[] {
                $"The player is at ({player.X}, {player.Y}) on the map with a speed of {playerSpeed}",
                $"The map is {map.Width} by {map.Height} and offset <{MapPosition.X}, {MapPosition.Y}>",
                $"The camera can see from ({camera.X}, {camera.Y}) to ({camera.X + camera.Width}, {camera.Y + camera.Height}), a {camera.Width}x{camera.Height} area",
                $"The user has {String.Join(" ,", Keyboard.GetState().GetPressedKeys().Select(x => x.ToString()))} pressed down",
                // TODO
                //$"The map has {null} tiles, {null} visible and {null} offscreen: {null} of which solid tiles, {null} transparent, and {null} doorways",
                //$"There are {null} entities in the map, {null} of which are moving, and {null} of witch with active pathfinding",
                //$"There are {null} collisions happening in the map"
            });
        }
    }

    //public abstract class Tile {
    //    public Vector2 position;
    //    public Texture2D texture;
    //    public Rectangle bounds;
    //    public bool solid;
    //    public float layer;

    //    public Tile(Vector2 position, Texture2D texture, bool solid = true, float layer = 0) {
    //        this.position = position;
    //        this.texture = texture;
    //        this.bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

    //    }
    //}
}
