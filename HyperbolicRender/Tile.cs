using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperbolicRender
{
    class Tile
    {
        public KnownColor color;
        public int[] sides;  //starts from right, goes counter clockwise
        public int alreadyDrawn;
    }
}
