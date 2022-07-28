using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicCircles
{
    public enum LINE_TYPE
    {
        STRAIGHT,
        SPLINE,
        BEZIER,
        ARC,
    }

    public enum LAYER_PATTERN
    {
        DIAGONALS,
        DOUBLE_DIAGONALS,
        RUNES,
        POLYGONS,
        SMALL_CIRCLES,  //TODO
        CUSTOM,  //TODO
        EMPTY,
    }
    public enum LAYER_BORDER
    {
        SOLID,
        DASHED,
        SPIKES,
        CROSSED,
    }
    partial class Form1
    {
        private readonly LINE_TYPE[] availableLineTypes = new LINE_TYPE[]
        {
            LINE_TYPE.STRAIGHT,
            LINE_TYPE.SPLINE,
            LINE_TYPE.BEZIER,
        };
        private readonly float[] threePoints = new float[] { 0.2f, 0.5f, 0.8f };
        private readonly float[] fourPoints = new float[] { 0.2f, 0.4f, 0.6f, 0.8f };

        private readonly LAYER_PATTERN[] availablePatterns = new LAYER_PATTERN[]
        {
            LAYER_PATTERN.DIAGONALS,
            LAYER_PATTERN.DOUBLE_DIAGONALS,
            LAYER_PATTERN.RUNES,
            LAYER_PATTERN.POLYGONS,
        };
        private readonly LAYER_BORDER[] availableBorders = new LAYER_BORDER[]
        {
            LAYER_BORDER.SOLID,
            LAYER_BORDER.DASHED,
            LAYER_BORDER.SPIKES,
            LAYER_BORDER.CROSSED,
        };
    }
}
