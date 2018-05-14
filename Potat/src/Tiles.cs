using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Potat.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Potat.Tiles {
    /// <summary>
    /// Basic tile
    /// </summary>
    public abstract class Tile : IRenderable {
        public bool solid;
        public bool colliding;
        public Rectangle bounds;

        public Tile(bool solid, Rectangle? bounds = null) {
            this.solid = solid;
            this.bounds = bounds ?? new Rectangle(0, 0, 32, 32);
        }

        public abstract void Render(Rectangle renderRect, SpriteBatch spriteBatch, GameTime gameTime);
        public abstract void Tick(GameTime gameTime);
    }

    /// <summary>
    /// Tile with one single texture
    /// </summary>
    public abstract class TexturedTile : Tile {
        private readonly static Random rnd = new Random();
        /// <summary>
        /// Tile's texture
        /// </summary>
        public Texture2D texture;
        /// <summary>
        /// If the texture should be randomly rotated when placed on the map
        /// </summary>
        public bool rotate;
        /// <summary>
        /// Rotation multiplier
        /// </summary>
        public int rotation;

        public TexturedTile(Texture2D texture, bool rotate = false, bool solid = false): base(solid) {
            this.texture = texture;
            this.rotate = rotate;

            rotation = rotate ? rnd.Next(3) : 0;
        }

        public override void Render(Rectangle renderRect, SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.Draw(texture, renderRect, null, Color.White, 0f, Vector2.Zero, (SpriteEffects)rotation, 0);
            //spriteBatch.Draw(texture, renderRect, Color.White);
        }

        public override void Tick(GameTime gameTime) { }
    }
    /// <summary>
    /// Tile with multiple textures. Ex: doors, clocks, lights
    /// </summary>
    public abstract class MultiTexturedTile : Tile {
        /// <summary>
        /// Tile's Textures (line of textures)
        /// </summary>
        public Texture2D textures;
        /// <summary>
        /// Current selected texture
        /// </summary>
        public uint texture = 0;

        public uint frames {
            get {
                return (uint)(textures.Width / bounds.Width);
            }
        }

        public MultiTexturedTile(Texture2D textures, bool solid = false) : base(solid) {
            this.textures = textures;
        }

        public override void Render(Rectangle renderRect, SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.Draw(textures, renderRect, new Rectangle(bounds.Width * (int)texture, 0, bounds.Width, bounds.Height), Color.White);
        }
    }

    /// <summary>
    /// Tile with multiple textures that are played frame by frame
    /// </summary>
    public abstract class AnimatedTile : MultiTexturedTile {
        /// <summary>
        /// Rate at which to change the frame
        /// </summary>
        int fps;
        /// <summary>
        /// Last time a frame was drawn
        /// </summary>
        double lastframe = 0;

        /// <summary>
        /// Create an animated tile
        /// </summary>
        /// <param name="textures">Sprite sheet</param>
        /// <param name="fps">Frames per second</param>
        public AnimatedTile(Texture2D textures, int fps, bool solid = false) : base(textures, solid) {
            this.textures = textures;
            this.fps = fps;
        }

        public override void Tick(GameTime gameTime) {
            if (lastframe + 1 / ((double)fps / 1000) < gameTime.TotalGameTime.TotalMilliseconds) {
                if (texture == frames - 1)
                    texture = 0;
                else
                    texture += 1;


                lastframe = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }
    }

    public class WoodTile : TexturedTile {
        public WoodTile(Game game) : base(game.Content.Load<Texture2D>("tiles/wood"), false) {

        }
    }

    public class StoneTile : TexturedTile {
        public StoneTile(Game game) : base(game.Content.Load<Texture2D>("tiles/stone"), true) {

        }
    }

    public class WaterTile : AnimatedTile {
        public WaterTile(Game game) : base(game.Content.Load<Texture2D>("tiles/water"), 2) {

        }
    }

    public class BrickTile: TexturedTile {
        public BrickTile(Game game) : base(game.Content.Load<Texture2D>("tiles/brick"), false, true) {

        }
    }
}
