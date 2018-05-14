using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Potat.Extensions;
using Potat.Tiles;
using System;

namespace Potat {
    public class Map : IRenderable {
        public Rectangle bounds;
        public Rectangle tileBounds;

        public Tile[,] tiles;

        public Map(Rectangle bounds, Rectangle tileBounds, Tile[,] tiles) {
            this.bounds = bounds;
            this.tileBounds = tileBounds;
            this.tiles = tiles;
        }

        public void Render(Rectangle renderRect, SpriteBatch spriteBatch, GameTime gameTime) {
            for (int ytile = 0; ytile < tiles.GetLength(0); ytile++)
                for (int xtile = 0; xtile < tiles.GetLength(1); xtile++) {
                    Rectangle tileRenderRect = new Rectangle(renderRect.X + (xtile * tileBounds.Width), renderRect.Y + (ytile * tileBounds.Height), tileBounds.Width, tileBounds.Height);
                    Tile tile = tiles[ytile, xtile];

                    tile.Render(tileRenderRect, spriteBatch, gameTime);

                    if (tile.solid)
                        spriteBatch.DrawRectangle(tileRenderRect, Color.Pink, 4);

                    if (tile.colliding)
                        spriteBatch.FillRectangle(tileRenderRect, Color.Yellow);
                }
        }

        public Point GetTileIndex(Point position) {
            int posX = (int)Math.Floor((double)position.X / tileBounds.Width);
            int posY = (int)Math.Floor((double)position.Y / tileBounds.Height);

            return new Point(posX, posY);
        }
        public Tile GetTile(Point position) {
            Point index = GetTileIndex(position);
            
            if (index.X == -1 || index.Y == -1) {
                return null;
            }

            return tiles[index.Y, index.X];
        }

        public Tile[] GetTiles(Rectangle bounds) {
            Point topleft = GetTileIndex(bounds.Location);
            Point bottomright = GetTileIndex(bounds.Location + bounds.Size);

            foreach (Tile tile in tiles)
                tile.colliding = false;

            for (int xindex = topleft.X; xindex <= bottomright.X; xindex++)
                for (int yindex = topleft.Y; yindex <= bottomright.Y; yindex++) {
                    if (xindex < tiles.GetLength(1) && yindex < tiles.GetLength(0))
                        tiles[yindex, xindex].colliding = true;
                    Console.WriteLine(new Point(xindex));
                }

            //Console.WriteLine($"tl: {topleft}, br: {topleft + new Point(xcount, ycount)}, count: {xcount}, {ycount}");
            //Console.WriteLine(bounds.Location);

            //Tile[] tiles = new Tile[xcount * ycount];

            return null;
        }

        public void Tick(GameTime gameTime) {
            foreach (Tile tile in tiles) {
                tile.Tick(gameTime);
            }
        }
    }
}
