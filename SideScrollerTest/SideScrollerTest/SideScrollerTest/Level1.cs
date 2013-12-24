using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SideScroller
{
    class Level1 : Level
    {
        public Level1(GraphicsDeviceManager gdm) : base(gdm)
        {
            
            platforms = new int[30][];
            for (int i = 0; i < platforms.Length; i++)
            {

                platforms[i] = new int[MAX_HEIGHT];

            }


            for (int i = 0; i < platforms.Length; i++)
            {

                for (int j = 0; j < platforms[i].Length; j++)
                {


                    if (j == 10 && i < 30 && i != 10 && i != 11 && i != 0)
                    {
                        platforms[i][j] = 1;
                    }
                    else if (i == 0 && j == 11)
                    {
                        platforms[i][j] = 1;
                    }
                    else if (j == 9 && i == 5)
                    {
                        platforms[i][j] = 1;
                    }
                    else
                    {
                        platforms[i][j] = 0;
                    }

                }
            }

            END = platforms[1].Length * Platform.PLAT_WIDTH;



        }
    }
}
