using System;
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
        public class Butterfly
        {
            public Model butterflyModel;
            public Vector3 position;
            public Vector3 velocity;
        }


        private Project1 game;
        private Model butterfly;

        private Random random = new Random();


        private LinkedList<Butterfly> butterflys = new LinkedList<Butterfly>();

        public LinkedList<Butterfly> Butterflys { get { return butterflys; } }

        public ButterflyField(Project1 game)
        {
            this.game = game;
        }



        /// <summary>
        /// This function is called to load content into this component
        /// of our game.
        /// </summary>
        /// <param name="content">The content manager to load from.</param>
        public void LoadContent(ContentManager content)
        {
            butterfly = content.Load<Model>("Butterfly");
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
                    //effect.View = game.Camera.View;
                    //effect.Projection = game.Camera.Projection;
                }
                mesh.Draw();
            }
        }

        private Vector3 RandomVector(float min, float max)
        {
            return new Vector3((float)(min + (random.NextDouble() * (max - min))),
                (float)(min + (random.NextDouble() * (max - min))),
                (float)(min + (random.NextDouble() * (max - min))));
        }


        public void positionButterflys()
        {
            while (butterflys.Count < 5)
            {
                Butterfly butter_fly = new Butterfly();
                butter_fly.butterflyModel = butterfly;
                butter_fly.position = RandomVector(-80, 80);
                butter_fly.velocity = RandomVector(-40, 40);
                if (butter_fly.position.Length() < 100)
                    continue;
                butterflys.AddLast(butter_fly);
            }
        }
    }
}
