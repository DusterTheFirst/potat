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
    }
}
