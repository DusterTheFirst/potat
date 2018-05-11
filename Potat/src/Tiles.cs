using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Potat.Extensions;
using System;

namespace Potat.Tiles {
    /// <summary>
    /// Tile with one single texture
    /// </summary>
    public abstract class TexturedTile : IRenderable {
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

        public TexturedTile(Texture2D texture, bool rotate = false) {
            this.texture = texture;
            this.rotate = rotate;

            rotation = rotate ? rnd.Next(3) : 0;
        }

        public void Render(Rectangle renderRect, SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.Draw(texture, renderRect, null, Color.White, 0f, Vector2.Zero, (SpriteEffects)rotation, 0);
            //spriteBatch.Draw(texture, renderRect, Color.White);
        }

        public void Tick(GameTime gameTime) {}
    }
    /// <summary>
    /// Tile with multiple textures. Ex: doors, clocks, lights
    /// </summary>
    public abstract class MultiTexturedTile : IRenderable {
        /// <summary>
        /// Tile's Textures
        /// </summary>
        public Texture2D[] textures;
        /// <summary>
        /// Current selected texture
        /// </summary>
        public uint CurrentTexture {
            get {
                return _texture;
            }
            set {
                _texture = Math.Min(value, (uint)textures.Length);
            }
        }
        private uint _texture = 0;

        public MultiTexturedTile(Texture2D[] textures) {
            this.textures = textures;
        }

        public void Render(Rectangle renderRect, SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.Draw(textures[CurrentTexture], renderRect, Color.White);
        }

        public abstract void Tick(GameTime gameTime);
    }

    /// <summary>
    /// Tile with multiple textures that are played frame by frame
    /// </summary>
    public abstract class AnimatedTile: MultiTexturedTile {
        /// <summary>
        /// Rate at which to change the frame
        /// </summary>
        int fps;
        /// <summary>
        /// Last time a frame was drawn
        /// </summary>
        double lastframe;

        public AnimatedTile(Texture2D[] textures, int fps): base(textures) {
            this.textures = textures;
            this.fps = fps;
        }

        public override void Tick(GameTime gameTime) {
            if (gameTime.TotalGameTime.Milliseconds + 1/((decimal)fps/1000) < gameTime.TotalGameTime.Milliseconds) {
                if (CurrentTexture == textures.Length)
                    CurrentTexture = 0;
                else
                    CurrentTexture += 1;
            }

            lastframe = gameTime.TotalGameTime.TotalMilliseconds;
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

    // TODO: ACTAULLY ANIMATE
    public class WaterTile : AnimatedTile {
        public WaterTile(Game game) : base(game.Content.Load<Texture2D>("tiles/water").Split(32, 32), 1) {

        }
    }
}
