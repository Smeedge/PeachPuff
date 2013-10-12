﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Project1
{
    class CoinField
    {
        private Project1 game;
        private Model coinModel;

        // Represents one coin
        public class Coin
        {
            public Model CoinModel;
            public Vector3 position;
            public float size;
            public float angle = -.5f;
            public float rate = 3f;
            public bool angleChange = false;
        }



        private Random random = new Random();


        private LinkedList<Coin> coins = new LinkedList<Coin>();

        public LinkedList<Coin> Coins { get { return coins; } }


        public CoinField(Project1 game)
        {
            this.game = game;
        }

        public void positionCoins()
        {
            while (Coins.Count < 5)
            {
                Coin coinObj = new Coin();
                coinObj.CoinModel = coinModel;
                coinObj.position = RandomVector(-80, 80);
                coinObj.size = 10;

                if (coinObj.position.Length() < 100)
                    continue;
                Coins.AddLast(coinObj);
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
            coinModel = content.Load<Model>("Coin");
            positionCoins();
        }

        /// <summary>
        /// This function is called to update this component of our game
        /// to the current game time.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Coin coin in Coins)
            {
                Matrix.CreateRotationZ(coin.angle);
            }
        }


        /// <summary>
        /// This function is called to draw this game component.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="gameTime"></param>
        public void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            foreach (Coin butter_fly in Coins)
            {
                DrawModel(graphics, coinModel, Matrix.CreateScale(10) * Matrix.CreateTranslation(butter_fly.position));
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

        private void RemoveCoin(Vector3 position)
        {
            for (LinkedListNode<Coin> CoinNode = Coins.First; CoinNode != null; )
            {
                LinkedListNode<Coin> nextNode = CoinNode.Next;
                Coin butter_fly = CoinNode.Value;

                if (position == butter_fly.position)
                {
                    // The Coin has been eaten. Remove it and return true.
                    Coin newCoin = new Coin();
                    newCoin.size = butter_fly.size;
                    newCoin.CoinModel = butter_fly.CoinModel;
                    newCoin.position = RandomVector(-80, 80);

                    // Delete the original Coin
                    Coins.Remove(CoinNode);
                    // Add the new Coin
                    Coins.AddLast(newCoin);
                }
                CoinNode = nextNode;
            }

        }



        public bool TestSphereForCollision(BoundingSphere sphere)
        {
            for (LinkedListNode<Coin> CoinNode = Coins.First; CoinNode != null; )
            {
                LinkedListNode<Coin> nextNode = CoinNode.Next;
                Coin butter_fly = CoinNode.Value;

                // Obtain a bounding sphere for the Coin. 
                BoundingSphere bs = coinModel.Meshes[0].BoundingSphere;
                bs = bs.Transform(coinModel.Bones[0].Transform);

                // Move this to world coordinates. 
                bs.Radius *= butter_fly.size;
                bs.Center += butter_fly.position;

                if (sphere.Intersects(bs))
                {
                    System.Diagnostics.Trace.WriteLine("It collided!\n");
                    RemoveCoin(butter_fly.position);
                    return true;
                }

                CoinNode = nextNode;
            }

            return false;
        }

    }
}
