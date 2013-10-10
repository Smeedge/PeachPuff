﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Project1
{
    class ButterflyField
    {
        private Project1 game;
        private Model butterfly;

        // bones for the wings
        private ModelBone leftwingBase;
        private ModelBone rightwingBase;
        private Matrix leftBaseBind;
        private Matrix rightBaseBind;


        private Random random = new Random();


        private LinkedList<Butterfly> butterflys = new LinkedList<Butterfly>();

        public LinkedList<Butterfly> Butterflys { get { return butterflys; } }


        public class Butterfly
        {
            public Model butterflyModel;
            public Vector3 position;
            public Vector3 velocity;
            public float size;
            public float angle = -.5f;
            public float rate = 3f;
            public bool angleChange = false;
        }

        public ButterflyField(Project1 game)
        {
            this.game = game;
        }

        public void positionButterflys()
        {
            while (butterflys.Count < 5)
            {
                Butterfly butter_fly = new Butterfly();
                butter_fly.butterflyModel = butterfly;
                butter_fly.position = RandomVector(-80, 80);
                butter_fly.velocity = RandomVector(-40, 40);
                butter_fly.size = 10;

                if (butter_fly.position.Length() < 100)
                    continue;
                butterflys.AddLast(butter_fly);
            }
        }

        public Vector3 RandomVector(float min, float max)
        {
            return new Vector3((float)(min + (random.NextDouble() * (max - min))),
                (float)(min + (random.NextDouble() * (max - min))), 0);
        }

        /// <summary>
        /// This function is called to load content into this component
        /// of our game.
        /// </summary>
        /// <param name="content">The content manager to load from.</param>
        public void LoadContent(ContentManager content)
        {
            butterfly = content.Load<Model>("Butterfly");
            leftwingBase = butterfly.Bones["WingLeft"];
            rightwingBase = butterfly.Bones["WingRight"];
            leftBaseBind = leftwingBase.Transform;
            rightBaseBind = rightwingBase.Transform;

            positionButterflys();
        }

        /// <summary>
        /// This function is called to update this component of our game
        /// to the current game time.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Butterfly butter_fly in butterflys)
            {
                butter_fly.position += butter_fly.velocity * delta;

                // this code animates the butterfly
                if (butter_fly.angle >= -.5f && butter_fly.angleChange == false)
                    butter_fly.angle += (float)(Math.PI * butter_fly.rate * delta);
                else
                    butter_fly.angle -= (float)(Math.PI * butter_fly.rate * delta);
                if (butter_fly.angle > Math.PI / 4)
                {
                    butter_fly.angle = (float)Math.PI / 4;
                    butter_fly.angleChange = true;
                }
                else if (butter_fly.angle < -.5f)
                {
                    butter_fly.angle = -.5f;
                    butter_fly.angleChange = false;
                }
                leftwingBase.Transform = Matrix.CreateRotationZ(butter_fly.angle) * leftBaseBind;
                rightwingBase.Transform = Matrix.CreateRotationZ(-butter_fly.angle) * rightBaseBind;
            }
        }


        /// <summary>
        /// This function is called to draw this game component.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="gameTime"></param>
        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            foreach (Butterfly butter_fly in butterflys)
            {
                DrawModel(graphics, butterfly, Matrix.CreateScale(10) * Matrix.CreateTranslation(butter_fly.position));
            }
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

        private void RemoveButterfly(Vector3 position)
        {
            for (LinkedListNode<Butterfly> butterflyNode = butterflys.First; butterflyNode != null; )
            {
                LinkedListNode<Butterfly> nextNode = butterflyNode.Next;
                Butterfly butter_fly = butterflyNode.Value;

                if (position == butter_fly.position)
                {
                    // The butterfly has been eaten. Remove it and return true.
                    Butterfly newButterfly = new Butterfly();
                    newButterfly.size = butter_fly.size;
                    newButterfly.butterflyModel = butter_fly.butterflyModel;
                    newButterfly.velocity = RandomVector(-40, 40);
                    newButterfly.position = RandomVector(-80, 80);

                    // Delete the original butterfly
                    butterflys.Remove(butterflyNode);
                    // Add the new butterfly
                    butterflys.AddLast(newButterfly);
                }
                butterflyNode = nextNode;
            }

        }

     

        public bool TestSphereForCollision(BoundingSphere sphere)
        {
            for (LinkedListNode<Butterfly> butterflyNode = butterflys.First; butterflyNode != null; )
            {
                LinkedListNode<Butterfly> nextNode = butterflyNode.Next;
                Butterfly butter_fly = butterflyNode.Value;

                // Obtain a bounding sphere for the butterfly. 
                BoundingSphere bs = butterfly.Meshes[0].BoundingSphere;
                bs = bs.Transform(butterfly.Bones[0].Transform);

                // Move this to world coordinates. 
                bs.Radius *= butter_fly.size;
                bs.Center += butter_fly.position;

                if (sphere.Intersects(bs))
                {
                    System.Diagnostics.Trace.WriteLine("It collided!\n");
                    RemoveButterfly(butter_fly.position);
                    return true;
                }

                butterflyNode = nextNode;
            }

            return false;
        }

    }
}
