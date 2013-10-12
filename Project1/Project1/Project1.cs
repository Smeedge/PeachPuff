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

        private KeyboardState lastKeyboardState;

        // our game screens
        Project1GameScreen project1Screen = null;
        SplashGameScreen splashScreen = null;
        EndGameScreen endScreen = null;
        // The game screen we are playing
        GameScreen screen = null;



        /// <summary>
        /// A reference to the audio engine we use
        /// </summary>
        public AudioEngine audioEngine;

        /// <summary>
        /// The loaded audio wave bank
        /// </summary>
        public WaveBank waveBank;

        /// <summary>
        /// The loaded audio sound bank
        /// </summary>
        public SoundBank soundBank;


        private Camera camera;
        public Camera Camera { get { return camera; } }

        public enum GameScreens { Splash, Project1, End };

        public GraphicsDeviceManager Graphics { get { return graphics; } }

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

                case GameScreens.End:
                    screen = endScreen;
                    break;
            }

            screen.Activate();
        }

        public Project1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            project1Screen = new Project1GameScreen(this);
            splashScreen = new SplashGameScreen(this);
            endScreen = new EndGameScreen(this, project1Screen);
            screen = splashScreen;
            camera = new Camera(graphics);
            camera.UseChaseCamera = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            lastKeyboardState = Keyboard.GetState();
            camera.Initialize();
            project1Screen.Initialize();
            splashScreen.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            project1Screen.LoadContent();
            splashScreen.LoadContent();
            endScreen.LoadContent();
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

            KeyboardState keyBoardState = Keyboard.GetState();

            lastKeyboardState = keyBoardState; 
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
            spriteBatch.Begin();
            screen.DrawSprites(gameTime, spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
