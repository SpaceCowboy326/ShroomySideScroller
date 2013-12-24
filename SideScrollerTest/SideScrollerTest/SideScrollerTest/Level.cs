/* Level
 * 
 * This class is used to represent a single level in the game.  Levels are designed using a simple 2-d array of integers.
 * Each integer represents a different form of tile type.  If the block is a 0, no tile is used. These arrays are designated
 * using the "Platform" class, which contains the actual array to be used.
 * 
 * Collision detection also currently occurs in the level class.  Each time the screen is refreshed, the level creates four small
 * rectangles around objects sent to the "checkCollisions" function.  A collision key, identical to the environment shown but containing
 * only two colors, is checked for any overlapping.  The function then returns an array describing in which directions the
 * object is currently colliding with the environment.  
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SideScroller
{



    class Level
    {
        //Collision detection constants
        const int TOP = 0;
        const int BOTTOM = 1;
        const int LEFT = 2;
        const int RIGHT = 3;

            
        public const int MAX_HEIGHT = 12;
        protected int BEGIN = 0;
        protected int END = 0;
        private Vector2 start = Vector2.Zero;
        private const int alphaValue = 255;
        protected Texture2D collisionKey;

        SpriteBatch mySpriteBatch;

        GraphicsDeviceManager graphics;
        RenderTarget2D topBottomRender;
        RenderTarget2D leftRightRender;
        RenderTarget2D collisionRender;

        //the color of platforms
        private Color keyColor = new Color(0, 255, 255);

        protected int[][] platforms;
        Platform p;

        public Level(GraphicsDeviceManager gdm)
        {
            p = new Platform();
            graphics = gdm;
            mySpriteBatch = new SpriteBatch(gdm.GraphicsDevice);
        }

        public void LoadContent(ContentManager theContentManager)
        {
            p.LoadContent(theContentManager);
            MakeCollisionKey();
        }


        /* Checks for collisions, and returns an integer array explaining which
         * borders of the object have collided with Platforms.
         * 
         * TOP = 0
         * BOTTOM = 1
         * LEFT = 2
         * RIGHT = 3
         * 
         */
        public int[] checkCollisions(RenderTarget2D topBottom, RenderTarget2D leftRight, Vector2 loc)
        {
            //int array used to show which directions the collisions occured in

            int[] collisions = new int[4] { 0, 0, 0, 0 };

            int topBottomStart = (int)((0.25) * (topBottom.Width / (0.5)));
            int leftRightStart = (int)((1.0 / 8.0) * (leftRight.Width / (0.75)));


            topBottomRender = topBottom;
            leftRightRender = leftRight;

            graphics.GraphicsDevice.SetRenderTarget(topBottomRender);
            graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Red, 0, 0);

            //draw the top collision detector  
            mySpriteBatch.Begin();

            Rectangle source = new Rectangle((int)loc.X + topBottomStart, (int)loc.Y, topBottomRender.Width, topBottomRender.Height);


            Rectangle destination = new Rectangle(0, 0, topBottomRender.Width, topBottomRender.Height);
            mySpriteBatch.Draw(collisionKey, destination,
                  source, Color.White);

            mySpriteBatch.End();
            graphics.GraphicsDevice.SetRenderTarget(null);

            int aPixels = topBottomRender.Width * topBottomRender.Height;
            Color[] myColors = new Color[aPixels];
            topBottomRender.GetData<Color>(myColors);

            foreach (Color aColor in myColors)
            {

                //if there is a teal pixel, there is a platform in the way
                if (aColor == keyColor)
                {
                    collisions[TOP] = 1;
                    break;
                }
            }

            graphics.GraphicsDevice.SetRenderTarget(topBottomRender);
            graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Red, 0, 0);

            //don't bother calculating if the player goes beyond the level,
            //just fall and die.
            if (loc.X + Player.PLAYER_WIDTH < 0 || loc.X > collisionKey.Width)
            {
                collisions[BOTTOM] = 0;
            }
            else
            {
                //draw the bottom collision detector  

                mySpriteBatch.Begin();

                source = new Rectangle((int)loc.X + topBottomStart, (int)loc.Y + (int)((leftRightRender.Height) / 0.75) - topBottomRender.Height,
                          topBottomRender.Width, topBottomRender.Height);
                mySpriteBatch.Draw(collisionKey, destination,
                      source, Color.White);

                mySpriteBatch.End();
                graphics.GraphicsDevice.SetRenderTarget(null);
                topBottomRender.GetData<Color>(myColors);

                foreach (Color aColor in myColors)
                {
                    //if there is a teal pixel, there is a platform in the way
                    if (aColor == keyColor)
                    {
                        collisions[BOTTOM] = 1;

                        break;
                    }
                }


            }
            graphics.GraphicsDevice.SetRenderTarget(leftRightRender);
            graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Red, 0, 0);

            //draw the left collision detector  
            mySpriteBatch.Begin();


            destination = new Rectangle(0, 0, leftRightRender.Width, leftRightRender.Height);

            //bad - put a magic number into left source rectangle, due to some issues with the model
            //check to see if this is persistent with other objects
            source = new Rectangle((int)loc.X + 15, (int)loc.Y + leftRightStart,
                      leftRightRender.Width, leftRightRender.Height);

            mySpriteBatch.Draw(collisionKey, destination,
                  source, Color.White);

            mySpriteBatch.End();
            graphics.GraphicsDevice.SetRenderTarget(null);

            aPixels = leftRightRender.Width * leftRightRender.Height;
            myColors = new Color[aPixels];
            leftRightRender.GetData<Color>(myColors);

            foreach (Color aColor in myColors)
            {
                //if there is a teal pixel, there is a platform in the way
                if (aColor == keyColor)
                {

                    collisions[LEFT] = 1;
                    break;
                }
            }

            graphics.GraphicsDevice.SetRenderTarget(leftRightRender);
            graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Red, 0, 0);
            //draw the right collision detector  
            mySpriteBatch.Begin();

            destination = new Rectangle(0, 0, leftRightRender.Width, leftRightRender.Height);
            //more magic numbers.  sigh.
            source = new Rectangle((int)loc.X + topBottomRender.Width - leftRightRender.Width + 30, (int)loc.Y + leftRightStart,
                      leftRightRender.Width, leftRightRender.Height);

            mySpriteBatch.Draw(collisionKey, destination,
                  source, Color.White);

            mySpriteBatch.End();
            graphics.GraphicsDevice.SetRenderTarget(null);
            leftRightRender.GetData<Color>(myColors);

            foreach (Color aColor in myColors)
            {
                //if there is a teal pixel, there is a platform in the way
                if (aColor == keyColor)
                {

                    collisions[RIGHT] = 1;
                    break;
                }
            }


            return collisions;
        }


        public void Update(GameTime theGameTime, int playerX)
        {
            start.X = -playerX;

        }

        /*
         * MakeCollisionKey
         * 
         * Called once, when the level is originally created.  This creates the collision key which makes it much simpler
         * to determine whether collisions have occued between an object and the environment.
         */
        public void MakeCollisionKey()
        {
            collisionRender = new RenderTarget2D(graphics.GraphicsDevice, platforms.Length * 50,
                           MAX_HEIGHT * 50);
            graphics.GraphicsDevice.SetRenderTarget(collisionRender);
            graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Red, 0, 0);


            mySpriteBatch.Begin();

            for (int i = 0; i < platforms.Length; i++)
            {

                for (int j = 0; j < platforms[i].Length; j++)
                {
                    if (platforms[i][j] == 1)
                    {

                        p.Position = start + new Vector2(i * Platform.PLAT_WIDTH, j * Platform.PLAT_HEIGHT);
                        p.DrawBase(mySpriteBatch);
                    }
                }
            }
            mySpriteBatch.End();

            graphics.GraphicsDevice.SetRenderTarget(null);
            //probably unnecessary
            collisionKey = collisionRender;
        }


        /*Draws the level contained in the class's platform object.
         */
        public void Draw(SpriteBatch theSpriteBatch)
        {
            //Debug:  shows what the collision render is actually seeing.
                //  theSpriteBatch.Draw(collisionKey, new Rectangle((int)start.X, (int)start.Y, collisionKey.Width, collisionKey.Height), Color.White);

       //     theSpriteBatch.Draw(topBottomRender, new Rectangle(50, 50, topBottomRender.Width, topBottomRender.Height), Color.White);
      //      theSpriteBatch.Draw(leftRightRender, new Rectangle(50, 100, leftRightRender.Width, leftRightRender.Height), Color.White);

            for (int i = 0; i < platforms.Length; i++)
            {

                for (int j = 0; j < platforms[i].Length; j++)
                {
                    if (platforms[i][j] == 1)
                    {

                        p.Position = start + new Vector2(i * Platform.PLAT_WIDTH, j * Platform.PLAT_HEIGHT);
                        p.Draw(theSpriteBatch);
                    }
                }
            }


        }





    }




}
