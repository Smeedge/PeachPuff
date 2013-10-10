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
        #region Fields

        private Project1 game;   // the game
        private Model batModel; // bats model

        // animates the bat
        private ModelBone leftwingBase;
        private ModelBone rightwingBase;
        private Matrix leftBaseBind;
        private Matrix rightBaseBind;

        private float angle = -.5f;
        private float rate = 1f;
        private bool angleChange = false;


        /// <summary>
        /// Bool if wings are flapping
        /// </summary>
        private bool flap = false;

        /// <summary>
        /// Bat orientation as a quaternion
        /// </summary>
        private Quaternion orientation = Quaternion.Identity;

        /// <summary>
        /// Current position
        /// </summary>
        private Vector3 position = Vector3.Zero;

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
        private const float MaxThrust = 40;

        /// <summary>
        /// The current turning rate in radians per second
        /// Effectively the azimuth change rate
        /// </summary>
        private float turnRate = 0;

        /// <summary>
        /// The maximum turning rate
        /// </summary>
        private const float MaxTurnRate = (float)Math.PI;

        /// <summary>
        /// elevation rate
        /// </summary>
        private float pitchRate = 0;

        /// <summary>
        /// max elevation rate
        /// </summary>
        private const float MaxPitchRate = (float)(Math.PI / 2);

        #endregion

        #region Properties

        public Model Model { get { return batModel; } }


        /// <summary>
        /// The current bat thrust
        /// </summary>
        public float Thrust { get { return thrust; } set { thrust = value; } }

        /// <summary>
        /// Bat flapping its wings
        /// </summary>
        public bool Flap { get { return flap; } set { flap = value; } }

        /// <summary>
        /// Current position of the bat
        /// </summary>
        public Vector3 Position { get { return position; } }

        /// <summary>
        /// Turning rate in radians per second
        /// </summary>
        public float TurnRate { get { return turnRate; } set { turnRate = value; } }

        /// <summary>
        /// Elevation rate
        /// </summary>
        public float PitchRate { get { return pitchRate; } set { pitchRate = value; } }


        /// <summary>
        /// The current bat transformation
        /// </summary>
        public Matrix Transform
        {
            get
            {
                return Matrix.CreateFromQuaternion(orientation) *
                        Matrix.CreateTranslation(position);
            }
        }

        #endregion

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
             leftwingBase = batModel.Bones["LeftWing1"];
             rightwingBase = batModel.Bones["RightWing1"];
             leftBaseBind = leftwingBase.Transform;
             rightBaseBind = rightwingBase.Transform;
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            double delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // this code animates the bat on its own
            if (angle >= -.5f && angleChange == false)
                angle += (float)(Math.PI * rate * delta);
            else
                angle -= (float)(Math.PI * rate * delta);
            if (angle > Math.PI / 4)
            {
                angle = (float)Math.PI / 4;
                angleChange = true;
            }
            else if (angle < -.5f)
            {
                angle = -.5f;
                angleChange = false;
            }
            leftwingBase.Transform = Matrix.CreateRotationY(angle) * leftBaseBind;
            rightwingBase.Transform = Matrix.CreateRotationY(-angle) * rightBaseBind;

            //
            // Orientation updates
            //

            orientation *= Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), -turnRate * MaxTurnRate * (float)delta);
            orientation *= Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), pitchRate * MaxPitchRate * (float)delta);
            orientation.Normalize();


            float acceleration = thrust * MaxThrust - Drag * speed;
            speed += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Matrix transform = Matrix.CreateFromQuaternion(orientation);

            Vector3 directedThrust = Vector3.TransformNormal(new Vector3(0, 0, 1), transform);
            position += directedThrust * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            DrawModel(graphics, batModel, Transform);
        }


       private void DrawModel(GraphicsDeviceManager graphics, Model model, Matrix world)
       {
           Matrix[] transforms = new Matrix[model.Bones.Count];
           model.CopyAbsoluteBoneTransformsTo(transforms);

           foreach (ModelMesh mesh in model.Meshes)
           {
               foreach (BasicEffect effect in mesh.Effects)
               {
                   effect.EnableDefaultLighting();
                   effect.World = transforms[mesh.ParentBone.Index] * world;
                   effect.View = game.Camera.View;
                   effect.Projection = game.Camera.Projection;
               }
               mesh.Draw();
           }
       }
    }
}
