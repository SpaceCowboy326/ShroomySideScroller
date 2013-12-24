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
    class Player : Sprite
    {

        //Collision detection constants
        const int TOP = 0;
        const int BOTTOM = 1;
        const int LEFT = 2;
        const int RIGHT = 3;

        public static float PLAYER_X = SideScroller.SCREEN_RES.X/2 
            - (PLAYER_WIDTH/2);

        //constant values for movement
        const int START_POSITION_X = 125;
        const int START_POSITION_Y = 350;
        const int P_SPEED = 400;
        const int P_JUMP_SPEED = 450;
        const int JUMP_SPEED = 600;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        const int NO_MOVE = 0;

        public static int PLAYER_WIDTH = 85;
        public static int PLAYER_HEIGHT = 110;

        //Holds all of the player's animation source rectangles.
        //Currently 6 animations
        const int StandRight = 0;
        const int WalkRight1 = 1;
        const int WalkRight2 = 2;
        const int StandLeft = 3;
        const int WalkLeft1 = 4;
        const int WalkLeft2 = 5;
     //   Rectangle[] animations;

        private Animation runAnimation;
        private Animation idleAnimation;



        //keeps track of the current level
        Level theLevel;

        //How many milliseconds per frame.
       // int msPerFrame;


        GraphicsDeviceManager gdm;

        RenderTarget2D playerTBRender;
        RenderTarget2D playerLRRender;

 //       private Rectangle colBox;

        Vector2 jumpStart = Vector2.Zero;

        //keep track of player's state
        public enum State
        {
            Walking,
            Jumping,
            Dead
        }

        State curState = State.Walking;

        const String PLAYER_ASSET_NAME = "Shroomy";

        ContentManager mContentManager;

        KeyboardState mPreviousKeyboardState;

        //Used to denote the speed and direction of the object
        Vector2 mDirection = new Vector2(NO_MOVE, MOVE_DOWN);

        Vector2 mSpeed = Vector2.Zero;


        public void LoadContent(ContentManager theContentManager, GraphicsDeviceManager gm)
        {
            mContentManager = theContentManager;

            gdm = gm;



            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            mSpeed = new Vector2(P_SPEED, JUMP_SPEED);
            base.LoadContent(theContentManager, PLAYER_ASSET_NAME);
            Source = new Rectangle(0, 0, PLAYER_WIDTH, PLAYER_HEIGHT);


            //Initialize the animations
            runAnimation = new Animation();
            idleAnimation = new Animation();
            runAnimation.Initialize(mSpriteTexture, new Vector2(0, 0),1, PLAYER_WIDTH, PLAYER_HEIGHT, 3, 250, Color.White, (float)1.0, true);
            idleAnimation.Initialize(mSpriteTexture, new Vector2(0, 0),1, PLAYER_WIDTH, PLAYER_HEIGHT, 1, 250, Color.White, (float)1.0, true);

            //Add the animations to the player's list
            addAnimation("walk", runAnimation);
            addAnimation("idle", idleAnimation);
            playAnimation("idle");

            //Set up all the rectangles for different player animations
      /*
            animations = new Rectangle[6];
            int count = 0;
            for (int j = 0; j < PLAYER_HEIGHT * 2; j += PLAYER_HEIGHT)
            {
                for (int i = 0; i < PLAYER_WIDTH * 3; i += PLAYER_WIDTH)
                {
                    animations[count] = new Rectangle(i, j, PLAYER_WIDTH, PLAYER_HEIGHT);
                    count++;
                }
            }
       */
    
            
      /*      playerAnimation = new Animation();
            playerAnimation.Initialize(new Texture2D(), Position,
                PLAYER_WIDTH, PLAYER_HEIGHT, 3, 5, Color.White,  */

            //create Render Targets for the Player, so they can be passed to the level
            //for collision detection
            int topBottomSize = (int)((0.5) * Source.Width);
            int leftRightSize = (int)((3.0 / 4.0) * Source.Height);

            playerTBRender = new RenderTarget2D(gdm.GraphicsDevice, topBottomSize,
               5);
            playerLRRender = new RenderTarget2D(gdm.GraphicsDevice, 5,
               leftRightSize);
        }


        /*  Accessors
         
         
         
         */

        //returns the current player state
        public State getState()
        {
            return curState;
        }


        //return the player's current direction
        public Vector2 getDirection()
        {
            return mDirection;
        }


        public void setLevel(Level l)
        {
            theLevel = l;

        }


        /*Update
         * 
         */
        public void Update(GameTime theGameTime)
        {

            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            
            UpdateMovement(aCurrentKeyboardState, theGameTime);
            UpdateJump(aCurrentKeyboardState);



            //Collision detection information
            int[] col = theLevel.checkCollisions(playerTBRender, playerLRRender, Position + (mDirection * mSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds));
            //can't move up if collisions occur above
            if (col[TOP] == 1)
            {

                mDirection.Y = MOVE_DOWN;
            }
            //can't move down if collisions occur below
            if (col[BOTTOM] == 1 && mDirection.Y != MOVE_UP)
            {
                mDirection.Y = NO_MOVE;
            }//if not jumping without collision, time to fall.
            else if( col[BOTTOM] == 0 && mDirection.Y != MOVE_UP)
            {
                mDirection.Y = MOVE_DOWN;
                curState = State.Jumping;

            }

            //can't move left
            if (col[LEFT] == 1 && mDirection.X != MOVE_RIGHT)
            {
                mDirection.X = NO_MOVE;
            }

            //can't move right
            if (col[RIGHT] == 1 && mDirection.X != MOVE_LEFT)
            {
                mDirection.X = NO_MOVE;
            }
            mPreviousKeyboardState = aCurrentKeyboardState;

            base.Update(theGameTime, mSpeed, mDirection);

            //Player is dead if they drop off the screen
            if (Position.Y > SideScroller.SCREEN_RES.Y)
            {
                curState = State.Dead;
            }


        }





        //allow player to jump
        private void UpdateJump(KeyboardState aCurrentKeyboardState)
        {
            if (curState == State.Walking)
            {
                //jump is spacebar is pressed
                if (aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
                {
                    Jump();
                }
            }

            if (curState == State.Jumping)
            {
                //jump will peak at 150 pixels
                if (jumpStart.Y - Position.Y > 150)
                {
                    mDirection.Y = MOVE_DOWN;
                }

                //the player has returned to the original position
                //if (Position.Y > jumpStart.Y)
                if (mDirection.Y == NO_MOVE) 
                {                    
 //                   Position.Y = jumpStart.Y;
                    curState = State.Walking;
                    mSpeed = new Vector2(P_SPEED, JUMP_SPEED);
 //                  mDirection = new Vector2(NO_MOVE , MOVE_DOWN);
                    mDirection = Vector2.Zero;
                }
            }
        }





        public void UpdateMovement(KeyboardState kbState, GameTime gameTime)
        {

  //          mSpeed = Vector2.Zero;
  //          mDirection = Vector2.Zero;

            if (kbState.IsKeyDown(Keys.A) == true)
            {
              /*  if (Source == animations[WalkLeft2])
                {
               //     Source = animations[WalkLeft1];
                    playAnimation("walk");
                }
                else
                {
          //          Source = animations[WalkLeft2];
                }
               * */
                horizontalFlip = true;
                mDirection.X = MOVE_LEFT;
            }
            else if (kbState.IsKeyDown(Keys.D) == true)
            {
              /*
                if (Source == animations[WalkRight2])
                {
                    Source = animations[WalkRight1];
                }
                else
                {
                    Source = animations[WalkRight2];
                }
               * */
                horizontalFlip = false;
                mDirection.X = MOVE_RIGHT;
                
            }
            else
            {
                if (!horizontalFlip)
                {                    
             //       Source = animations[StandRight];
                }
                else
                {
            //        Source = animations[StandLeft];
                }
                mDirection.X = NO_MOVE;
            }

            //Determine which animation to play
            if (curState == State.Walking && mDirection.X != 0)
            {
                playAnimation("walk");
            }
            else if (curState == State.Walking && mDirection.X == 0)
            {
                playAnimation("idle");
            }
 
            //Position.X = PLAYER_X;
        }

        private void Jump()
        {
            if (curState != State.Jumping)
            {
                curState = State.Jumping;
                jumpStart = Position;
                mDirection.Y = MOVE_UP;
                mSpeed = new Vector2(P_SPEED, JUMP_SPEED);
            }
        }


        public new void Draw(SpriteBatch theSpriteBatch)
        {
            
         //   theSpriteBatch.Draw(mSpriteTexture, new Vector2(PLAYER_X, Position.Y), Source,
         //        new Color(255, 255, 255, (byte)MathHelper.Clamp(alphaValue, 125, 255)), 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            if (horizontalFlip)
            {
                theSpriteBatch.Draw(mSpriteTexture, new Vector2(PLAYER_X, Position.Y), animation.sourceRect,
                    new Color(255, 255, 255, (byte)MathHelper.Clamp(alphaValue, 125, 255)), 0.0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                theSpriteBatch.Draw(mSpriteTexture, new Vector2(PLAYER_X, Position.Y), animation.sourceRect,
                     new Color(255, 255, 255, (byte)MathHelper.Clamp(alphaValue, 125, 255)), 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            }
            //base.Draw(theSpriteBatch);


        }
    
    }



}
