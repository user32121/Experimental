using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProceduralPathways
{
    public partial class AdditionMethod
    {
        const KnownColor Green = KnownColor.Lime, Blue = KnownColor.Blue;

        List<Tile> availableTiles = new List<Tile>() {
            //L R U D Col

            //0
            //new Tile(),
            //1
            //new Tile(){ left=Red },  //L
            //new Tile(){ left=Blue },
            //new Tile(){ right=Red },  //R
            //new Tile(){ right=Blue },
            //new Tile(){ up=Red },  //U
            //new Tile(){ up=Blue },
            //new Tile(){ down=Red },  //D
            //new Tile(){ down=Blue },
            //2
            new Tile(){ center=Green, left=Green,right=Green },  //LR
            //new Tile(){ center=Blue, left=Blue,right=Red },
            //new Tile(){ center=Red, left=Red,right=Blue },
            new Tile(){ center=Blue, left=Blue,right=Blue },
            new Tile(){ center=Green, left=Green,up=Green },  //LU
            //new Tile(){ center=Red, left=Red,up=Blue },
            //new Tile(){ center=Blue, left=Blue,up=Red },
            new Tile(){ center=Blue, left=Blue,up=Blue },
            new Tile(){ center=Green,left=Green,down=Green },  //LD
            //new Tile(){ center=Red, left=Red,down=Blue },
            //new Tile(){ center=Blue, left=Blue,down=Red },
            new Tile(){ center=Blue, left=Blue,down=Blue },
            new Tile(){ center=Green, right=Green,up=Green },  //RU
            //new Tile(){ center=Red, right=Red,up=Blue },
            //new Tile(){ center=Blue, right=Blue,up=Red },
            new Tile(){ center=Blue, right=Blue,up=Blue },
            new Tile(){ center=Green, right=Green,down=Green },  //RD
            //new Tile(){ center=Red, right=Red,down=Blue },
            //new Tile(){ center=Blue, right=Blue,down=Red },
            new Tile(){ center=Blue, right=Blue,down=Blue },
            new Tile(){ center=Green, up=Green,down=Green },  //UD
            //new Tile(){ center=Red, up=Red,down=Blue },
            //new Tile(){ center=Blue, up=Blue,down=Red },
            new Tile(){ center=Blue, up=Blue,down=Blue },
            //3
            //new Tile(){ center=Green, right=Green,up=Green,down=Green },  //RUD
            //new Tile(){ center=Red, right=Red,up=Red,down=Blue },
            //new Tile(){ center=Red, right=Red,up=Blue,down=Red },
            //new Tile(){ center=Blue, right=Red,up=Blue,down=Blue },
            //new Tile(){ center=Red, right=Blue,up=Red,down=Red },
            //new Tile(){ center=Blue, right=Blue,up=Red,down=Blue },
            //new Tile(){ center=Blue, right=Blue,up=Blue,down=Red },
            //new Tile(){ center=Blue, right=Blue,up=Blue,down=Blue },
            //new Tile(){ center=Green, left=Green,up=Green,down=Green },  //LUD
            //new Tile(){ center=Red, left=Red,up=Red,down=Blue },
            //new Tile(){ center=Red, left=Red,up=Blue,down=Red },
            //new Tile(){ center=Blue, left=Red,up=Blue,down=Blue },
            //new Tile(){ center=Red, left=Blue,up=Red,down=Red },
            //new Tile(){ center=Blue, left=Blue,up=Red,down=Blue },
            //new Tile(){ center=Blue, left=Blue,up=Blue,down=Red },
            //new Tile(){ center=Blue, left=Blue,up=Blue,down=Blue },
            //new Tile(){ center=Green, left=Green,right=Green,down=Green },  //LRD
            //new Tile(){ center=Red, left=Red,right=Red,down=Blue },
            //new Tile(){ center=Red, left=Red,right=Blue,down=Red },
            //new Tile(){ center=Blue, left=Red,right=Blue,down=Blue },
            //new Tile(){ center=Red, left=Blue,right=Red,down=Red },
            //new Tile(){ center=Blue, left=Blue,right=Red,down=Blue },
            //new Tile(){ center=Blue, left=Blue,right=Blue,down=Red },
            //new Tile(){ center=Blue, left=Blue,right=Blue,down=Blue },
            //new Tile(){ center=Green, left=Green,right=Green,up=Green },  //LRU
            //new Tile(){ center=Red, left=Red,right=Red,up=Blue },
            //new Tile(){ center=Red, left=Red,right=Blue,up=Red },
            //new Tile(){ center=Blue, left=Red,right=Blue,up=Blue },
            //new Tile(){ center=Red, left=Blue,right=Red,up=Red },
            //new Tile(){ center=Blue, left=Blue,right=Red,up=Blue },
            //new Tile(){ center=Blue, left=Blue,right=Blue,up=Red },
            //new Tile(){ center=Blue, left=Blue,right=Blue,up=Blue },
            ////4
            //new Tile(){ center=Red, left=Red,right=Red,up=Red,down=Red },  //LRUD
            //new Tile(){ center=Red, left=Red,right=Red,up=Red,down=Blue },
            //new Tile(){ center=Red, left=Red,right=Red,up=Blue,down=Red },
            new Tile(){ center=Green, left=Green,right=Green,up=Blue,down=Blue },
            //new Tile(){ center=Red, left=Red,right=Blue,up=Red,down=Red },
            new Tile(){ center=Green, left=Green,right=Blue,up=Green,down=Blue },
            new Tile(){ center=Green, left=Green,right=Blue,up=Blue,down=Green },
            //new Tile(){ center=Blue, left=Red,right=Blue,up=Blue,down=Blue },
            //new Tile(){ center=Red, left=Blue,right=Red,up=Red,down=Red },
            new Tile(){ center=Blue, left=Blue,right=Green,up=Green,down=Blue },
            new Tile(){ center=Blue, left=Blue,right=Green,up=Blue,down=Green },
            //new Tile(){ center=Blue, left=Blue,right=Red,up=Blue,down=Blue },
            new Tile(){ center=Blue, left=Blue,right=Blue,up=Green,down=Green },
            //new Tile(){ center=Blue, left=Blue,right=Blue,up=Red,down=Blue },
            //new Tile(){ center=Blue, left=Blue,right=Blue,up=Blue,down=Red },
            //new Tile(){ center=Blue, left=Blue,right=Blue,up=Blue,down=Blue },
        };
        bool[] availableFlags;

        Tile errorTile = new Tile() { backCol = KnownColor.Red, center = KnownColor.Red };
    }
}
