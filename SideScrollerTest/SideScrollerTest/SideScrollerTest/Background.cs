using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SideScroller
{
    class Background : Sprite
    {
        //constants regarding movement of backgrounds
        const int BG_SPEED = 700;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        const int NO_MOVE = 0;


        Background nextBG;
        Background prevBG;

   //     public static readonly Background FINAL_BG = new Background(null, null);

        //used for the background opacity
        const int mAlphaValue = 175;
        //stores the previous keyboardstate
      //  KeyboardState mPreviousKeyboardState;

        //Used to denote the speed and direction of the object
        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = new Vector2(BG_SPEED, 0);



        public void setNeighbors(Background prev, Background next)
        {
            prevBG = prev;
            nextBG = next;        
        }


        /*Update
         * 
         */
        public void Update(GameTime theGameTime, Vector2 playerDirection)
        {
   //         UpdateOrder();

    /*        KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            UpdateMovement(aCurrentKeyboardState);

            mPreviousKeyboardState = aCurrentKeyboardState;
            */
            mDirection = -playerDirection;
            base.Update(theGameTime, mSpeed, mDirection);

        }

        /* Allows Backgrounds to reset positions based on the positions of the other
         * backgrounds.
         * 
         * 
         */
        public void UpdateOrder()
        {
            if (Position.X < -Size.Width)
            {
                if (prevBG != null)
                {
                    Position.X = prevBG.Position.X + prevBG.Size.Width;
                }
            }
            if (nextBG.Position.X < nextBG.Size.Width && nextBG.Position.X > 0 )
            {

                Position.X = nextBG.Position.X - Size.Width;
            }
        }

        /*UpdateMovement
         * 
         * Updates the position of the background based on movement
         */
        public void UpdateMovement(KeyboardState kbState)
        {
/*
            mSpeed = Vector2.Zero;
            mDirection = Vector2.Zero;
            */
 /*           if (kbState.IsKeyDown(Keys.D) == true)
            {
                mSpeed.X = BG_SPEED;
                mDirection.X = MOVE_LEFT;
            }
            else if (kbState.IsKeyDown(Keys.A) == true)
            {
                mSpeed.X = BG_SPEED;
                mDirection.X = MOVE_RIGHT;
            }
            else
            {
                mDirection.X = NO_MOVE;
            }
            
            */

        }
    
        /*Draw
         */
        public override void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position, Source,
                new Color(255, 255, 255, (byte)MathHelper.Clamp(mAlphaValue, 125, 255)), 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);

        }
    }
}
