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
    class Platform : Sprite
    {

        private const String PLATFORM_NAME = "Platform";
        public const int PLAT_WIDTH = 50;
        public const int PLAT_HEIGHT = 50;
        protected Texture2D baseTexture;
        protected String platTypeName = "Wood";

        protected Rectangle colBox;

        public void LoadContent(ContentManager theContentManager)
        {

            base.LoadContent(theContentManager, platTypeName);
            baseTexture = theContentManager.Load<Texture2D>(PLATFORM_NAME);
            Source = new Rectangle(0, 0, Source.Width, Source.Height);
            colBox = new Rectangle(0, 0, Source.Width, Source.Height);

        }

        public void DrawBase( SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(baseTexture, Position, Source,
                 new Color(255, 255, 255, (byte)MathHelper.Clamp(alphaValue, 125, 255)), 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }


    }
}
