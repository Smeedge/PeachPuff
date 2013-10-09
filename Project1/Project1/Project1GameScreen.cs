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
    public class Project1GameScreen : GameScreen
    {
        private Bat bat;
        private ButterflyField butterflyField;
        private KeyboardState lastKeyboardState;

        public Project1GameScreen(Project1 game): base(game)
        {
            bat = new Bat(Game);
            butterflyField = new ButterflyField(Game);
        }

        public override void Initialize()
        {
            base.Initialize();
            lastKeyboardState = Keyboard.GetState();
        }

        public override void LoadContent() 
        {
            bat.LoadContent(Game.Content);
            butterflyField.LoadContent(Game.Content);
            //scoreFont = Content.Load<SpriteFont>("scorefont");
        }
        public override void Activate()
        {
            base.Activate();

            lastKeyboardState = Keyboard.GetState();
        }
        public override void Deactivate()
        {
        }
        public override void Update(GameTime gameTime) 
        {
            KeyboardState keyBoardState = Keyboard.GetState();

            // moves the bat
            if (keyBoardState.IsKeyDown(Keys.Space))
            {
                bat.Thrust = 1;
            }
            else
            {
                bat.Thrust = 0;
            }

            // turn bat left and right
            if (keyBoardState.IsKeyDown(Keys.Left))
            {
                bat.TurnRate = -1;
            }
            else if (keyBoardState.IsKeyDown(Keys.Right))
            {
                bat.TurnRate = 1;
            }
            else
            {
                bat.TurnRate = 0;
            }

            // elevate the bat
            if (keyBoardState.IsKeyDown(Keys.Up))
            {
                bat.PitchRate = 1;
            }
            else if (keyBoardState.IsKeyDown(Keys.Down))
            {
                bat.PitchRate = -1;
            }
            
           

            lastKeyboardState = keyBoardState;
            bat.Update(gameTime);
            butterflyField.Update(gameTime);
            Game.Camera.DesiredEye = Vector3.Transform(new Vector3(0, 30, -30), bat.Transform);
            Game.Camera.Center = bat.Position;
            Game.Camera.Up = bat.Transform.Up;
            Game.Camera.Update(gameTime);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime) 
        {
            Game.GraphicsDevice.Clear(Color.White);
            bat.Draw(Game.Graphics, gameTime);
            butterflyField.Draw(Game.Graphics, gameTime);
        }
        public override void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
           // spriteBatch.DrawString(scoreFont, "hi", new Vector2(10, 10), Color.White);
        }
    }
}
