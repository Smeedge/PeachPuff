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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Project1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Bat bat;
        private Bee bee;
        private Field field;

        // our game screens
        Project1GameScreen project1Screen = null;
        SplashGameScreen splashScreen = null;

        // The game screen we are playing
        GameScreen screen = null;

        public enum GameScreens { Splash, Project1 };

        public void SetScreen(GameScreens newScreen)
        {
            screen.Deactivate();

            switch (newScreen)
            {
                case GameScreens.Splash:
                    screen = splashScreen;
                    break;
                
                case GameScreens.Project1:
                    screen = project1Screen;
                    break;
            }

            screen.Activate();
        }

        public Project1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            bat = new Bat(this);
            bee = new Bee(this, bat);
            field = new Field(this, bat);
            project1Screen = new Project1GameScreen(this);
            splashScreen = new SplashGameScreen(this);

            screen = splashScreen;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            project1Screen.Initialize();
            splashScreen.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bat.LoadContent(Content);
            field.LoadContent(Content);
            project1Screen.LoadContent();
            splashScreen.LoadContent();

            screen.Activate();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            screen.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.PeachPuff);

            screen.Draw(gameTime);
            //bat.Draw(graphics, gameTime);
            field.Draw(graphics, gameTime);
            screen.DrawSprites(gameTime, spriteBatch);
            base.Draw(gameTime);
        }

    }
}
