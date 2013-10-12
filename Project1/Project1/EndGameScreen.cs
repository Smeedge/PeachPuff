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
    public class EndGameScreen: GameScreen
    {
        private SpriteFont font1;
        private Project1GameScreen mainGameScreen;
        public EndGameScreen(Project1 game, Project1GameScreen screen): base(game)
        {
            mainGameScreen = screen;
        }

        public override void LoadContent()
        {
            font1 = Game.Content.Load<SpriteFont>("font1");
        }

        public override void Activate()
        {
            base.Activate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState keyBoardState = Keyboard.GetState();

            if (keyBoardState.IsKeyDown(Keys.Enter))
            {
                Game.SetScreen(Project1.GameScreens.Project1);
            }

            if (keyBoardState.IsKeyDown(Keys.LeftControl) && keyBoardState.IsKeyDown(Keys.Q))
            {
                Game.Exit();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
        }

        public override void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int width = Game.GraphicsDevice.Viewport.Width;
            int height = Game.GraphicsDevice.Viewport.Height;

            int imgWid = (int)((16.0 / 9.0) * height);
            int tooWide = imgWid - width;

            Rectangle rect = new Rectangle(-tooWide / 2, 0, imgWid, height);
            
            spriteBatch.DrawString(font1, "GAME OVER!/n/n/n Your Score: " + mainGameScreen.score, new Vector2(width/4, height/4), Color.Red);
        }
    }
}
