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
        private Bee bee;
        private Field field;
        private ButterflyField butterflyField;
        private CoinField coinField;
        private KeyboardState lastKeyboardState;
        private SpriteFont scoreFont;

        public int score = 0;
        private int time = 0;
        private float timeM = 0;

        


        public Project1GameScreen(Project1 game)
            : base(game)
        {
            bat = new Bat(Game);
            butterflyField = new ButterflyField(Game);
            coinField = new CoinField(Game);
            bee = new Bee(Game, bat);
            field = new Field(Game);
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
            coinField.LoadContent(Game.Content);
            bee.LoadContent(Game.Content);
            field.LoadContent(Game.Content);
            scoreFont = Game.Content.Load<SpriteFont>("font1");
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
                bat.Thrust = 10;
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

            time = gameTime.TotalGameTime.Seconds;
            timeM = gameTime.TotalGameTime.Milliseconds / 100;


            lastKeyboardState = keyBoardState;
            bat.Update(gameTime);
            butterflyField.Update(gameTime);
            coinField.Update(gameTime);
            bee.Update(gameTime);
            field.Update(gameTime);
            Game.Camera.DesiredEye = new Vector3(bat.Position.X, 250, bat.Position.Z);
            Game.Camera.Center = bat.Position;
            Game.Camera.Up = bat.Transform.Backward;
            Game.Camera.Update(gameTime);


            Matrix[] transforms = new Matrix[bat.Model.Bones.Count];
            bat.Model.CopyAbsoluteBoneTransformsTo(transforms);
            Matrix batTransform = bat.Transform;

            foreach (ModelMesh mesh in bat.Model.Meshes)
            {
                BoundingSphere bs = mesh.BoundingSphere;
                bs = bs.Transform(transforms[mesh.ParentBone.Index] * batTransform);
                bool collided = butterflyField.TestSphereForCollision(bs);
                bool CoinCollide = coinField.TestSphereForCollision(bs);
            
                if (bee.TestSphereForCollision(bs))
                    Game.SetScreen(Project1.GameScreens.End); //DETECT WHETHER BEE COLLIDED WITH BAT, IF SO, DISPLAY ENDGAME SCREEN
               
            }


            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.ForestGreen);
            field.Draw(Game.Graphics, gameTime);
            bat.Draw(Game.Graphics, gameTime);
            butterflyField.Draw(Game.Graphics, gameTime);
            coinField.Draw(Game.Graphics, gameTime);
            bee.Draw(Game.Graphics, gameTime);
            
        }
        public override void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(scoreFont, "Score: " + score.ToString(), new Vector2(10, 10), Color.Red);
            spriteBatch.DrawString(scoreFont, "Time: " + time.ToString() + "." + timeM.ToString(), new Vector2(10, 40), Color.Yellow);
        }
    }
}
