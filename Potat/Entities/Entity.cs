using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Potat.Entities {
    abstract class Entity {
        public Texture2D texture;
        public Vector2 position;
        private Scene map;

        public Entity(Vector2 position, Texture2D texture, Scene map) {
            this.position = position;
            this.texture = texture;
            this.map = map;
        }

        abstract public void Tick(Game game, GameTime gameTime);
        abstract public void Render(SpriteBatch spriteBatch, Game game, GameTime gameTime);
    }
}
