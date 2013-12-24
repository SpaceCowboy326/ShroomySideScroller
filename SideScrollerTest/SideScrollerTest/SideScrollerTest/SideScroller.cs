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

namespace SideScroller
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SideScroller : Microsoft.Xna.Framework.Game
    {
        public static Vector2 SCREEN_RES;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TitleScreen titleScreen;
        GameScreen gameScreen;
        Screen currentScreen;


        public SideScroller()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            SCREEN_RES = new Vector2(1280, 720);
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            

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

            //initialize the screens/ load contents of screens
            titleScreen = new TitleScreen(this.Content, new EventHandler(TitleScreenEvent));
            gameScreen = new GameScreen(this.Content, new EventHandler(GameScreenEvent), graphics);

            this.IsMouseVisible = true;
            currentScreen = titleScreen;
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




//            sun.Position = new Vector2(800 - sun.Size.Width, 20);

            currentScreen.Update(gameTime);
            base.Update(gameTime);
             
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            currentScreen.Draw(this.spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }


        /**
         * Screen events, to be used by each individual screen
         */
        
        //Used when the player intends to start the game
        public void TitleScreenEvent(object obj, EventArgs e)
        {
            this.IsMouseVisible = false;
            //Switch to the controller detect screen, the Title screen is finished being displayed
            currentScreen = gameScreen;
        }


        //Used when the an event is called during the game?  (change levels here, perhaps?)
        public void GameScreenEvent(object obj, EventArgs e)
        {
            //Switch to the controller detect screen, the Title screen is finished being displayed
            currentScreen = new GameScreen(this.Content, new EventHandler(GameScreenEvent), graphics);
        }

    }
}
