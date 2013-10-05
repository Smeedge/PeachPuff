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
    public class Bat
    {
        private Project1 game;   // the game
        private Model batModel; // bats model

        // the bones of the bats wings
        private int leftwing1;
        private int leftwing2;
        private int rightwing1;
        private int rightwing2;
        private int head;

        /// <summary>
        /// Bat's wing angle when wings flap
        /// </summary>
        private float wingAngle = .2f;

        Vector3 translateLeftWings;
        Vector3 translateRightWings;

        private bool flap = false;

        private Vector3 location;

        public bool Flap { get { return flap; } set { flap = value; } }

        public Vector3 Location {get {return location;}}

        /********************************stuff********************/

        /// <summary>
        /// Current position
        /// </summary>
        private Vector3 position = Vector3.Zero;

        /// <summary>
        /// Compass heading (radians, 0 is Z direction)
        /// </summary>
        private float azimuth = 0;

        /// <summary>
        /// Climb angle (radians, 0 is level)
        /// </summary>
        private float elevation = 0;

        /// <summary>
        /// How fast we are going (cm/sec)
        /// </summary>
        private float speed = 0;

        /// <summary>
        /// Thrust in cm/sec^2
        /// </summary>
        private float thrust = 0;

        /// <summary>
        ///  Decelleration due to drag
        /// </summary>
        private const float Drag = 1;

        /// <summary>
        /// Maximum thrust (cm/sec^2)
        /// </summary>
        private const float MaxThrust = 100;

        /// <summary>
        /// The current ship thrust
        /// </summary>
        public float Thrust { get { return thrust; } set { thrust = value; } }


         public Bat(Project1 game)
        {
            this.game = game;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
         public void LoadContent(ContentManager content)
        {
             batModel = content.Load<Model>("Bat-rigid");
             leftwing1 = batModel.Bones.IndexOf(batModel.Bones["LeftWing1"]);
             leftwing2 = batModel.Bones.IndexOf(batModel.Bones["LeftWing2"]);
             rightwing1 = batModel.Bones.IndexOf(batModel.Bones["RightWing1"]);
             rightwing2 = batModel.Bones.IndexOf(batModel.Bones["RightWing2"]);
             head = batModel.Bones.IndexOf(batModel.Bones["Head"]);

        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            // flaps the bats wings
            if (flap)
            {
                translateLeftWings = new Vector3(0, -1, 1);
                translateRightWings = new Vector3(3, 0, 3);
                wingAngle = 0.2f;
            }
            else
            {
                translateRightWings = translateLeftWings = new Vector3(0, 0, 0);
                wingAngle = 0;
            }

            float acceleration = thrust * MaxThrust - Drag * speed;
            speed += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Matrix transform = Matrix.CreateRotationX(elevation) *
                Matrix.CreateRotationY(azimuth);

            Vector3 directedThrust = Vector3.TransformNormal(new Vector3(0, 0, 1), transform);
            position += directedThrust * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            Matrix transform = Matrix.CreateRotationX(elevation) *
                Matrix.CreateRotationY(azimuth) *
                Matrix.CreateTranslation(position);

            DrawModel(graphics, batModel, transform);
        }


       private void DrawModel(GraphicsDeviceManager graphics, Model model, Matrix world)
       {
           Matrix[] transforms = new Matrix[model.Bones.Count];
           model.CopyAbsoluteBoneTransformsTo(transforms);
           transforms[rightwing1] = Matrix.CreateRotationY(-wingAngle) * transforms[rightwing1];
           transforms[rightwing2] = Matrix.CreateRotationY(-wingAngle*2) * transforms[rightwing2] * Matrix.CreateTranslation(translateRightWings);
           transforms[leftwing1] = Matrix.CreateRotationY(wingAngle) * transforms[leftwing1];
           transforms[leftwing2] = Matrix.CreateRotationY(wingAngle*2) * transforms[leftwing2] * Matrix.CreateTranslation(translateLeftWings);
           transforms[head] = Matrix.CreateRotationX(wingAngle) * transforms[head];
           foreach (ModelMesh mesh in model.Meshes)
           {
               foreach (BasicEffect effect in mesh.Effects)
               {
                   effect.EnableDefaultLighting();
                   effect.World = transforms[mesh.ParentBone.Index] * world;
                   effect.View = Matrix.CreateLookAt(new Vector3(30, 30, 30),
                                                     new Vector3(0, 0, 0),
                                                     new Vector3(0, 1, 0));
                   effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(35),
                       graphics.GraphicsDevice.Viewport.AspectRatio, 10, 10000);
               }
               mesh.Draw();
           }
       }
    }
}
