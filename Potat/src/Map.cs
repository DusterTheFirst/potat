using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Potat {
    /// <summary>
    /// A renderable object
    /// </summary>
    public interface IRenderable {
        /// <summary>
        /// Render the object.
        /// Only ever called if visible
        /// </summary>
        /// <param name="renderRect">The calculated position of the object on the screen</param>
        /// <param name="spriteBatch">The sprite batch to render to</param>
        /// <param name="gameTime">The time since the last frame (not the last render call)</param>
        void Render(Rectangle renderRect, SpriteBatch spriteBatch, GameTime gameTime);

        /// <summary>
        /// Tick the object.
        /// Called throughout life unconditionally
        /// </summary>
        /// <param name="gameTime">Time since the last tick</param>
        void Tick(GameTime gameTime);
    }

    public class Map: IRenderable {
        public Rectangle bounds;

        public IRenderable[,] tiles;

        public Map(Rectangle bounds, IRenderable[,] tiles) {
            this.bounds = bounds;
            this.tiles = tiles;
        }

        public void Render(Rectangle renderRect, SpriteBatch spriteBatch, GameTime gameTime) {
            for (int ytile = 0; ytile < tiles.GetLength(0); ytile++)
                for (int xtile = 0; xtile < tiles.GetLength(1); xtile++) {
                    tiles[ytile, xtile].Render(new Rectangle(renderRect.X + xtile * 64, renderRect.Y + ytile * 64, 64, 64), spriteBatch, gameTime);
                }
        }

        public void Tick(GameTime gameTime) {
            foreach (IRenderable tile in tiles) {
                tile.Tick(gameTime);
            }
        }
    }
}
