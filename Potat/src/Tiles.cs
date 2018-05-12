using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Potat.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void Tick(GameTime gameTime) { }
    }
    /// <summary>
    /// Tile with multiple textures. Ex: doors, clocks, lights
    /// </summary>
    public abstract class MultiTexturedTile : IRenderable {
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
                return (uint)textures.Width / 32;
            }
        }

        public MultiTexturedTile(Texture2D textures) {
            this.textures = textures;
        }

        public void Render(Rectangle renderRect, SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.Draw(textures, renderRect, new Rectangle(32 * (int)texture, 0, 32, 32), Color.White);
        }

        public abstract void Tick(GameTime gameTime);
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
        /// <param name="textures">32px by a factor of 32px sprite sheet</param>
        /// <param name="fps">Frames per second</param>
        public AnimatedTile(Texture2D textures, int fps) : base(textures) {
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

    /// <summary>
    /// Tile with a flowing animation
    /// </summary>
    public abstract class FlowingAnimatedTile : IRenderable {
        /// <summary>
        /// Rate at which to change the frame
        /// </summary>
        int fps;
        /// <summary>
        /// Last time a frame was drawn
        /// </summary>
        double lastframe = 0;
        /// <summary>
        /// Tile's Textures (line of textures)
        /// </summary>
        public Texture2D textures;
        /// <summary>
        /// Y offset of the animation
        /// </summary>
        public int offset;

        public FlowingAnimatedTile(Texture2D textures, int fps) {
            this.textures = textures;
            this.fps = fps;
        }

        public void Render(Rectangle renderRect, SpriteBatch spriteBatch, GameTime gameTime) {
            spriteBatch.Draw(textures, renderRect, new Rectangle(0, offset, 32, 32), Color.White);

            if (offset + 32 > textures.Height) {
                int bottom = offset + 32;
                int missing = bottom - textures.Height;
                int exists = 32 - missing;

                spriteBatch.Draw(textures, new Rectangle(renderRect.X, renderRect.Y + exists*2, 64, missing*2), new Rectangle(0, 0, 32, missing), Color.White);
            }
        }

        public void Tick(GameTime gameTime) {
            if (lastframe + 1 / ((double)fps / 1000) < gameTime.TotalGameTime.TotalMilliseconds) {
                if (offset == textures.Height - 1)
                    offset = 0;
                else
                    offset += 1;


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

    // TODO: ACTAULLY ANIMATE
    public class WaterTile : FlowingAnimatedTile {
        public WaterTile(Game game) : base(game.Content.Load<Texture2D>("tiles/water"), 20) {

        }
    }
}
