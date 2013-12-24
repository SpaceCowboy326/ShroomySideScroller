using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SideScroller
{

    class Sprite
    {

        //The current position of the Sprite
        public Vector2 Position = new Vector2(0, 0);

        //The asset name for the Sprite's Texture
        public string AssetName;

        //Hangs on to the ame of the current animation
        string curKey;

        //The texture object used when drawing the sprite
        protected Texture2D mSpriteTexture;

        //The animation the sprite is currentyl playing
        protected Dictionary<string, Animation> animations;
        protected Animation animation;

        //The size of the Sprite
        public Rectangle Size;

        //Used to size the Sprite up or down from the original image
        public float mScale = 1.0f;

        public int alphaValue = 255;

        protected bool horizontalFlip;

        //The Rectangular area from the original image that 
        //defines the Sprite. 
        Rectangle mSource;
        public Rectangle Source
        {
            get { return mSource; }
            set
            {
                mSource = value;
                Size = new Rectangle(0, 0, (int)(mSource.Width * Scale), (int)(mSource.Height * Scale));
            }
        }




        //When the scale is modified through he property, the Size of the 
        //sprite is recalculated with the new scale applied.
        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                //Recalculate the Size of the Sprite with the new scale
                Size = new Rectangle(0, 0, (int)(Source.Width * Scale), (int)(Source.Height * Scale));
            }
        }

        //Load the texture for the sprite using the Content Pipeline
        public virtual void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            horizontalFlip = false;
            animations = new Dictionary<string, Animation>();
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            AssetName = theAssetName;
            Source = new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height);
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));

            Animation defaultAnimation = new Animation();
            defaultAnimation.Initialize(mSpriteTexture, Vector2.Zero, 1, mSpriteTexture.Width, mSpriteTexture.Height, 1, 0, Color.White, 1.0f, true);

            addAnimation("default", defaultAnimation);
            animation = defaultAnimation;
            //

        }

        //Adds an animation to the sprite's list of animations
        public void addAnimation(string key, Animation newAnimation)
        {
            animations.Add(key, newAnimation);
        }

        //Plays the referenced animation
        public void playAnimation(string key)
        {
            if (key != curKey)
            {
                Animation nextAnimation;
                animations.TryGetValue(key, out nextAnimation);
                animation = nextAnimation;
                curKey = key;
            }

        }

        //Update the Sprite and change it's position based on the passed in speed, direction and elapsed time.
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            animation.Update(theGameTime);
        }

        //Draw the sprite to the screen
        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
          //  Rectangle r2 = new Rectangle();
            if (horizontalFlip)
            {
                theSpriteBatch.Draw(mSpriteTexture, Position, animation.sourceRect,
                    new Color(255, 255, 255, (byte)MathHelper.Clamp(alphaValue, 125, 255)), 0.0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                theSpriteBatch.Draw(mSpriteTexture, Position, animation.sourceRect,
                     new Color(255, 255, 255, (byte)MathHelper.Clamp(alphaValue, 125, 255)), 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            }
         /*
            theSpriteBatch.Draw(mSpriteTexture, Position, r2,
                 new Color(255, 255, 255, (byte)MathHelper.Clamp(alphaValue, 125, 255)), 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            */

            
        }
    }

}
