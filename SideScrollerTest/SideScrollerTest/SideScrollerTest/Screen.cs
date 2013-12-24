using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SideScroller
{
    class Screen
    {

        protected EventHandler ScreenEvent;
        public Screen(EventHandler theScreenEvent)
        {
            ScreenEvent = theScreenEvent;
        }



        //Update any information specific to the screen
        public virtual void Update(GameTime theTime)
        {
        }

        //Draw any objects specific to the screen
        public virtual void Draw(SpriteBatch theBatch)
        {
        }

    }
}
