using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Potat {
    //public class Scene {
    //    List<Tile> tiles;

    //    Rectangle bounds;
    //    double speed;
    //    Vector2 offset;

    //    Player player;

    //    List<string> state;

    //    SpriteFont OpenSans;

    //    public Scene(Game game) {
    //        tiles = new List<Tile>();

    //        bounds = new Rectangle(0, 0, 1000, 1000);
    //        speed = 100;

    //        player = new Player(Vector2.Zero, null, this);

    //        state = new List<string>();

    //        offset = Vector2.Zero;

    //        OpenSans = game.Content.Load<SpriteFont>("fonts/OpenSans");
    //    }

    //    public void Tick(Game game, GameTime gameTime) {
    //        state = new List<string>();

    //        var kstate = Keyboard.GetState();

    //        if (kstate.IsKeyDown(Keys.W))
    //            offset.Y -= (int)(speed * gameTime.ElapsedGameTime.TotalSeconds);

    //        if (kstate.IsKeyDown(Keys.S))
    //            offset.Y += (int)(speed * gameTime.ElapsedGameTime.TotalSeconds);

    //        if (kstate.IsKeyDown(Keys.A))
    //            offset.X -= (int)(speed * gameTime.ElapsedGameTime.TotalSeconds);

    //        if (kstate.IsKeyDown(Keys.D))
    //            offset.X += (int)(speed * gameTime.ElapsedGameTime.TotalSeconds);

    //        TODO: STOP IF AT EDGE
    //        if (bounds.X > 0) {
    //            state.Add("Far Right");
    //            bounds.X = 0;
    //        }
    //        if (bounds.Y > 0) {
    //            state.Add("Low");
    //            bounds.Y = 0;
    //        }

    //        if (bounds.X + bounds.Width < game.GraphicsDevice.Viewport.Width) {
    //            state.Add("Far Left");
    //            bounds.X = game.graphics.PreferredBackBufferWidth - bounds.Width;
    //        }
    //        if (bounds.Y + bounds.Height < game.GraphicsDevice.Viewport.Height) {
    //            state.Add("High");
    //            bounds.Y = game.graphics.PreferredBackBufferHeight - bounds.Height;
    //        }

    //        TODO: MOVE CHARICTER IF AT EDGE

    //        TODO: CENTER IF TOO SMALL
    //         If too thin to cover full screen
    //        if (bounds.Width < game.GraphicsDevice.Viewport.Width) {
    //            bounds.X = (game.GraphicsDevice.Viewport.Width / 2) - (bounds.Width / 2);
    //        }
    //        If too short to cover full screen
    //        if (bounds.Height < game.GraphicsDevice.Viewport.Height) {
    //            bounds.Y = (game.GraphicsDevice.Viewport.Height / 2) - (bounds.Height / 2);
    //        }
    //        TODO: PLAYER
    //    }

    //    public void Render(SpriteBatch spriteBatch, Game game, GameTime gameTime) {
    //        Texture2D rect = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
    //        rect.SetData(new[] { Color.White });

    //        spriteBatch.Draw(rect, bounds, Color.White);


    //        Player


    //        DEGUG
    //        #region DEBUG
    //        spriteBatch.DrawDebug($"Top Left: ({bounds.X}, {bounds.Y}) Bottom Right: ({bounds.Width + bounds.X}, {bounds.Height + bounds.Y}) Screen Size: ({game.GraphicsDevice.Viewport.Width}, {game.GraphicsDevice.Viewport.Height})", OpenSans);
    //        spriteBatch.DrawDebug($"Offset: ({offset.X}, {offset.Y})", OpenSans);
    //        spriteBatch.DrawDebug($"Too: {String.Join(", ", state)}", OpenSans);
    //        spriteBatch.EndDebug();
    //        #endregion
    //    }

    //}
    public class Scene {
        SpriteFont OpenSans;

        Rectangle camera;
        Rectangle player;
        float playerSpeed = 100;
        Rectangle map;

        public Scene(Game game) {
            OpenSans = game.Content.Load<SpriteFont>("fonts/OpenSans");

            camera = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);

            player = new Rectangle(0, 0, 100, 100);

            map = new Rectangle(0, 0, 1000, 100);
        }

        public void Tick(Game game, GameTime gameTime) {
            var kstate = Keyboard.GetState();

            // Keep camera size == to viewport size
            camera.Width = 500;//game.GraphicsDevice.Viewport.Width;
            camera.Height = 500;//game.GraphicsDevice.Viewport.Height;

            // MOVE PLAYER IF KEY DOWN
            if (kstate.IsKeyDown(Keys.LeftShift))
                playerSpeed = 200;
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

            // CENTER CAMERA ON PLAYER
            camera.X = (player.X + (player.Width/2)) - (camera.Width / 2);

            camera.Y = (player.Y + (player.Height/2)) - (camera.Height / 2);

            // KEEP CAMERA ON MAP, NOT BLANK SPACE
            if (camera.Width > map.Width) {
                camera.X = (map.Width / 2) - (camera.Width / 2);
            }  else {
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

            spriteBatch.Draw(rect, map, Color.White);
            spriteBatch.Draw(rect, camera, Color.Red);
            spriteBatch.Draw(rect, player, Color.Green);

            spriteBatch.DrawDebug(OpenSans, new[] {
                $"The player is at ({player.X}, {player.Y}) on the map with a speed of {playerSpeed}",
                $"The map is {map.Width} by {map.Height}",
                $"The camera can see from ({camera.X}, {camera.Y}) to ({camera.X + camera.Width}, {camera.Y + camera.Height}), a {camera.Width}x{camera.Height} area"
            });
        }
    }

    public static class Extensions {
        public static void DrawDebug(this SpriteBatch spriteBatch, SpriteFont font, string[] strings) {
            Texture2D rect = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });

            for (int i = 0; i < strings.Length; i++) {

                String debug = strings[i];

                Vector2 measure = font.MeasureString(debug);

                spriteBatch.Draw(rect, new Rectangle(0, i * ((int)measure.Y), (int)measure.X, (int)measure.Y), Color.Black * 0.75f);
                spriteBatch.DrawString(font, debug, new Vector2(0, i * ((int)measure.Y)), Color.White);
            }
        }
    }

    public abstract class Tile {
        public Vector2 position;
        public Texture2D texture;
        public Rectangle bounds;
        public bool solid;
        public float layer;

        public Tile(Vector2 position, Texture2D texture, bool solid = true, float layer = 0) {
            this.position = position;
            this.texture = texture;
            this.bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

        }
    }
}
