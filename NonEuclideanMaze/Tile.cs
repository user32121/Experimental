using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonEuclideanMaze
{
    class Tile
    {
        static int counter;

        public enum INITALIZATIONSTATE
        {
            UNINITIALIZED,
            INITIALIZED,
        }
        public INITALIZATIONSTATE state;

        //x and y, positive and negative
        public Tile xp;
        public Tile xn;
        public Tile yp;
        public Tile yn;

        public int ID { get; }

        public Tile()
        {
            ID = counter++;
        }
    }
}
