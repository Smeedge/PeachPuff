﻿using System;
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
    class Bat
    {
        private Project1 game;   // the game
        private Model batModel; // bats model

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
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {

            // TODO: Add your update logic here

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {

            // TODO: Add your drawing code here
            DrawModel(graphics, batModel, Matrix.Identity);
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
