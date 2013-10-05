using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Project1
{
    class GameScreen
    {
        private Project1 game;

        public Project1 Game { get { return game; } }

        public GameScreen() {}
        public GameScreen(Project1 game)
        {
            this.game = game;
        }

        public virtual void Initialize()
        {

        }

        public virtual void LoadContent() {}
        public virtual void Activate() {}
        public virtual void Deactivate() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
        public virtual void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch) { }

        

    }
}
