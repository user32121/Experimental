using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProceduralPathways
{
    class Tile
    {
        public KnownColor left = defaultBackColor, right = defaultBackColor, up = defaultBackColor, down = defaultBackColor, 
            center = defaultBackColor, backCol = defaultBackColor;
        private const KnownColor defaultBackColor = KnownColor.Gray;

        public override string ToString()
        {
            //Color {color} left right up down
            return (left != backCol ? "L: " + left.ToString() : "") +
                (right != backCol ? "R: " + right.ToString() : "") +
                (up != backCol ? "U: " + up.ToString() : "") +
                (down != backCol ? "D: " + down.ToString() : "");
        }
    }
}
