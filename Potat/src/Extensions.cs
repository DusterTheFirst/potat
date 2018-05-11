using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Potat.Extensions {
    public static class Extensions {
        /// <summary>
        /// Draw debug messages on the screen
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch to render to</param>
        /// <param name="font">The font to render</param>
        /// <param name="strings">An array of strings to render</param>
        public static void DrawDebug(this SpriteBatch spriteBatch, SpriteFont font, string[] strings) {
            Texture2D rect = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });

            Dictionary<string, Vector2> messages = new Dictionary<string, Vector2>();

            // Gather measurements
            for (int i = 0; i < strings.Length; i++) {
                messages.Add(strings[i], font.MeasureString(strings[i]));
            }

            // Draw backdrop
            int length = (int)messages.Max(x => x.Value.X) + 2;
            int height = (int)messages.Select(x => x.Value.Y).Aggregate((a, b) => b + a);

            spriteBatch.Draw(rect, new Rectangle(0, 0, length, height), Color.Black * 0.75f);

            // Draw strings
            int offset = 0;
            foreach (KeyValuePair<string, Vector2> message in messages) {
                spriteBatch.DrawString(font, message.Key, new Vector2(0, offset * ((int)message.Value.Y)), Color.White);
                offset++;
            }
        }

        /// <summary>
        /// Splits a texture into an array of smaller textures of the specified size.
        /// </summary>
        /// <param name="original">The texture to be split into smaller textures</param>
        /// <param name="partWidth">The width of each of the smaller textures that will be contained in the returned array.</param>
        /// <param name="partHeight">The height of each of the smaller textures that will be contained in the returned array.</param>
        public static Texture2D[] Split(this Texture2D original, int partWidth, int partHeight) {
            int yCount = original.Height / partHeight + (partHeight % original.Height == 0 ? 0 : 1);//The number of textures in each horizontal row
            int xCount = original.Height / partHeight + (partHeight % original.Height == 0 ? 0 : 1);//The number of textures in each vertical column
            Texture2D[] r = new Texture2D[xCount * yCount];//Number of parts = (area of original) / (area of each part).
            int dataPerPart = partWidth * partHeight;//Number of pixels in each of the split parts

            //Get the pixel data from the original texture:
            Color[] originalData = new Color[original.Width * original.Height];
            original.GetData<Color>(originalData);

            int index = 0;
            for (int y = 0; y < yCount * partHeight; y += partHeight)
                for (int x = 0; x < xCount * partWidth; x += partWidth) {
                    //The texture at coordinate {x, y} from the top-left of the original texture
                    Texture2D part = new Texture2D(original.GraphicsDevice, partWidth, partHeight);
                    //The data for part
                    Color[] partData = new Color[dataPerPart];

                    //Fill the part data with colors from the original texture
                    for (int py = 0; py < partHeight; py++)
                        for (int px = 0; px < partWidth; px++) {
                            int partIndex = px + py * partWidth;
                            //If a part goes outside of the source texture, then fill the overlapping part with Color.Transparent
                            if (y + py >= original.Height || x + px >= original.Width)
                                partData[partIndex] = Color.Transparent;
                            else
                                partData[partIndex] = originalData[(x + px) + (y + py) * original.Width];
                        }

                    //Fill the part with the extracted data
                    part.SetData<Color>(partData);
                    //Stick the part in the return array:                    
                    r[index++] = part;
                }
            //Return the array of parts.
            return r;
        }
    }
}
