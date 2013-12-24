using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace SideScroller
{
    class GameScreen : Screen
    {
        //background images
        Background bgOne;
        Background bgTwo;
        Background bgThree;
        //      Sprite sun;

        MediaLibrary songLibrary;

       
        Player p;
        Level l1;



        ContentManager myContentManager;

        public GameScreen(ContentManager theContent, EventHandler theScreenEvent,
            GraphicsDeviceManager graphics) : base(theScreenEvent)
        {
            myContentManager = theContent;


            bgOne = new Background();
            bgOne.mScale = 1.2f;
            bgOne.Position.Y = -300;

            bgTwo = new Background();
            bgTwo.mScale = 1.2f;
            bgTwo.Position.Y = -300;

            bgThree = new Background();
            bgThree.mScale = 1.2f;
            bgThree.Position.Y = -300;

            bgOne.setNeighbors(bgThree, bgTwo);
            bgTwo.setNeighbors(bgOne, bgThree);
            bgThree.setNeighbors(bgTwo, bgOne);

            l1 = new Level1(graphics);


            //load the content of all the objects
            bgOne.LoadContent(myContentManager, "MountainBG1");
            bgOne.Position = new Vector2(0, 0);

            bgTwo.LoadContent(myContentManager, "MountainBG2");
            bgTwo.Position = new Vector2(bgOne.Position.X + bgOne.Size.Width, 0);

            bgThree.LoadContent(myContentManager, "MountainBG3");
            bgThree.Position = new Vector2(bgTwo.Position.X + bgTwo.Size.Width, 0);

            //            sun.LoadContent(this.Content, "Sun");

            l1.LoadContent(myContentManager);
            p = new Player();
            p.LoadContent(myContentManager, graphics);
            p.setLevel(l1);

            songLibrary = new MediaLibrary();

        }


        public override void Update(GameTime gameTime)
        {
            //Update all the game's objects
            l1.Update(gameTime, (int)(p.Position.X - Player.PLAYER_X));
            p.Update(gameTime);

            if (p.getState() == Player.State.Dead)
            {
                ScreenEvent.Invoke(this, new EventArgs());
            }

            bgOne.UpdateOrder();
            bgTwo.UpdateOrder();
            bgThree.UpdateOrder();

            Vector2 playerDirection = p.getDirection();

            bgOne.Update(gameTime, playerDirection);
            bgTwo.Update(gameTime, playerDirection);
            bgThree.Update(gameTime, playerDirection);

            

            base.Update(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {

      //      spriteBatch.Begin();

            bgOne.Draw(spriteBatch);
            bgTwo.Draw(spriteBatch);
            bgThree.Draw(spriteBatch);
            //          sun.Draw(this.spriteBatch);

            l1.Draw(spriteBatch);

            p.Draw(spriteBatch);

   //         spriteBatch.End();

            base.Draw(spriteBatch);


        }


    }
}
