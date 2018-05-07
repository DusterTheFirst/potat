using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Potat.Entities {
    class Player : Entity {

        public Player(Vector2 position, Texture2D texture, Scene map) : base(position, texture, map) {
            
        }

        public override void Render(SpriteBatch spriteBatch, Game game, GameTime gameTime) {
            throw new NotImplementedException();
        }

        public override void Tick(Game game, GameTime gameTime) {
            throw new NotImplementedException();
        }
    }
}
