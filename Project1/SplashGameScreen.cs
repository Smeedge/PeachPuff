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
    class SplashGameScreen: GameScreen
    {
        private Texture2D splash;
        private double time;

        public SplashGameScreen(Project1 game): base(game)
        {
        }

        public override void LoadContent()
        {
           // splash = Game.Content.Load<Texture2D>("sky");
        }

        public override void Activate()
        {
            base.Activate();
            time = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            time += gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int width = Game.GraphicsDevice.Viewport.Width;
            int height = Game.GraphicsDevice.Viewport.Height;

            int imgWid = (int)((16.0 / 9.0) * height);
            int tooWide = imgWid - width;

            Rectangle rect = new Rectangle(-tooWide / 2, 0, imgWid, height);
           // spriteBatch.Draw(splash, rect, Color.White);
        }
    }
}
