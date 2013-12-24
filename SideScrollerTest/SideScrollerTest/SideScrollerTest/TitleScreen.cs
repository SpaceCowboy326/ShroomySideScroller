using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace SideScroller
{



    class TitleScreen : Screen
    {
        
        //The image to be displayed for the Title Screen
        Texture2D titleScreenBg;
        Sprite startButton;
        Sprite optionsButton;
        Sprite scoresButton;

        float buttonScale = 1.0f;
        const int startXLoc = 900;
        const int startYLoc = 360;
        const int optionXLoc = 900;
        const int optionYLoc = 460;
        const int scoreXLoc = 900;
        const int scoreYLoc = 560;

        bool grow = true;

      //  ContentManager myContentManager;

        bool mousePressed = false;

        public TitleScreen( ContentManager theContent, EventHandler theScreenEvent ) : base( theScreenEvent )
        {
            titleScreenBg = theContent.Load<Texture2D>("ShroomTitle");
            startButton = new Sprite();
            startButton.mScale = buttonScale;
            startButton.LoadContent(theContent,"TitleStartButton");
            startButton.Position = new Vector2(startXLoc, startYLoc);

            optionsButton = new Sprite();
            optionsButton.mScale = buttonScale;
            optionsButton.LoadContent(theContent, "TitleOptionsButton");
            optionsButton.Position = new Vector2(optionXLoc, optionYLoc);

            scoresButton = new Sprite();
            scoresButton.mScale = buttonScale;
            scoresButton.LoadContent(theContent, "TitleScoreButton");
            scoresButton.Position = new Vector2(scoreXLoc, scoreYLoc);

        }



        public override void Draw(SpriteBatch theBatch)
        {
            theBatch.Draw(titleScreenBg, Vector2.Zero, Color.White);
            startButton.Draw(theBatch);
            optionsButton.Draw(theBatch);
            scoresButton.Draw(theBatch);

        }


        //Update all of the elements that need updating in the Title Screen        
        public override void Update(GameTime theTime)
        {
            if (startButton.mScale > 1.05)
            {
                grow = false;
            }
            else if (startButton.mScale < 0.95)
            {
                grow = true;
            }

            if (startButton.mScale < 1.05 && grow)
            {
                startButton.mScale += 0.001f;
                scoresButton.mScale += 0.001f;
                optionsButton.mScale += 0.001f;
            }
            else
            {
                startButton.mScale -= 0.001f;
                scoresButton.mScale -= 0.001f;
                optionsButton.mScale -= 0.001f;
            }

            
            MouseState mState = Mouse.GetState();
            int mouseX = mState.X;
            int mouseY = mState.Y;
            mousePressed = mState.LeftButton == ButtonState.Pressed;
 //           Console.WriteLine("MousePressed?:" + mousePressed);
            if( mousePressed && buttonClicked( mouseX, mouseY, startButton, startXLoc, startYLoc) )
            {
                ScreenEvent.Invoke(this, new EventArgs());

            }
            else if (mousePressed && buttonClicked(mouseX, mouseY, optionsButton, startXLoc, startYLoc))
            {

            }
            else if (mousePressed && buttonClicked(mouseX, mouseY, scoresButton, startXLoc, startYLoc))
            {

            }

            base.Update(theTime);
        }



        //Check to see if the start button was clicked
        private bool buttonClicked(int mouseX, int mouseY, Sprite button, int buttonX, int buttonY)
        {
            return mouseX >= buttonX && mouseX <= (buttonX+button.Size.Width)
                && mouseY >= buttonY && mouseY <= (buttonY+button.Size.Height);
        }

    }
}
