using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Project1
{
    class Bee
    {
        private Project1 game;   // the game
        private Model beeModel; // bees model
        private Bat bat;

        private int wing1;
        private int wing2;
        private int wing3;
        private int wing4;

        private float wingAngle;
        private float wingFlapSpeed = 0.01f;

        public float currentSpeed = 0;
        public float damping = 0;
        public float scale = 20;
        public Vector3 direction;
        public Vector3 location;
        Matrix locMatrix;

        public Bee(Project1 game, Bat bat)
        {
            this.game = game;
            this.bat = bat;
            location = Vector3.Zero;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            beeModel = content.Load<Model>("Bee");
            wing1 = beeModel.Bones.IndexOf(beeModel.Bones["RightWing1"]);
            wing2 = beeModel.Bones.IndexOf(beeModel.Bones["LeftWing1"]);
            wing3 = beeModel.Bones.IndexOf(beeModel.Bones["RightWing2"]);
            wing4 = beeModel.Bones.IndexOf(beeModel.Bones["LeftWing2"]);
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            /* MOVEMENT (FOLLOW CODE) */ 
            direction = Vector3.Forward;    //bat.Location - location;
            direction.Normalize();
            location += direction * currentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            double angle = Math.Atan2(direction.Y, direction.X);
            locMatrix = Matrix.CreateTranslation(location); // * rotation code

            /* WING FLAP CODE  */
            wingAngle += (float)(0.20 * gameTime.ElapsedGameTime.TotalSeconds / wingFlapSpeed);
            if (wingAngle > 0.20f)
            {
                wingAngle = 0.20f;
                wingFlapSpeed = -wingFlapSpeed;
            }
            if (wingAngle < -0.20f)
            {
                wingAngle = -0.20f;
                wingFlapSpeed = -wingFlapSpeed;
            }

            


        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {

            // TODO: Add your drawing code here
            DrawModel(graphics, beeModel, Matrix.Identity);
        }


        private void DrawModel(GraphicsDeviceManager graphics, Model model, Matrix world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];

            model.CopyAbsoluteBoneTransformsTo(transforms);
            transforms[wing1] = Matrix.CreateRotationY(wingAngle) * transforms[wing1];
            transforms[wing2] = Matrix.CreateRotationY(-wingAngle) * transforms[wing2];
            transforms[wing3] = Matrix.CreateRotationY(wingAngle) * transforms[wing3];
            transforms[wing4] = Matrix.CreateRotationY(-wingAngle) * transforms[wing4];

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    effect.View = Matrix.CreateLookAt(new Vector3(0, 30, 0),
                                                      new Vector3(0, 0, 0),
                                                      new Vector3(1, 0, 0));
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(35),
                        graphics.GraphicsDevice.Viewport.AspectRatio, 10, 10000);
                }
                mesh.Draw();
            }
        }
    }
}
