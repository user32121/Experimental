using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MagicCircles
{
    [DataContract]
    internal class CircleData
    {
        [DataMember]
        public int minRadius;
        [DataMember]
        public int layers;
        [DataMember]
        public float[] layerSizes;


        [DataMember]
        public List<RuneData> runes;

        [DataMember]
        public LAYER_PATTERN[] patterns;
        [DataMember]
        public PatternParam[] patternParams;

        [DataMember]
        public LAYER_BORDER[] borders;
        [DataMember]
        public BorderParam[] borderParams;


        [DataMember]
        public Color? effectColor;
    }

    [Serializable]
    internal class RuneData
    {
        [DataMember]
        public RuneStroke[] strokes;
    }

    [Serializable]
    internal struct RuneStroke
    {
        [DataMember]
        public PointF start;
        [DataMember]
        public PointF end;
        [DataMember]
        public LINE_TYPE type;
        [DataMember]
        public int variant;
    }

    [Serializable]
    [KnownType(typeof(DiagonalsPatternParam))]
    [KnownType(typeof(DoubleDiagonalsPatternParam))]
    [KnownType(typeof(RunesPatternParam))]
    [KnownType(typeof(PolygonsPatternParam))]
    [KnownType(typeof(EmptyPatternParam))]
    internal abstract class PatternParam { }
    [Serializable]
    [KnownType(typeof(SolidBorderParam))]
    [KnownType(typeof(DashedBorderParam))]
    [KnownType(typeof(SpikesBorderParam))]
    [KnownType(typeof(CrossedBorderParam))]
    internal abstract class BorderParam { }

    [Serializable]
    internal class DiagonalsPatternParam : PatternParam { public bool flipped; public int count; public double delta; }
    [Serializable]
    internal class DoubleDiagonalsPatternParam : PatternParam { public int count; public double delta; }
    [Serializable]
    internal class RunesPatternParam : PatternParam { public int count; public int[] runeIndices; }
    [Serializable]
    internal class PolygonsPatternParam : PatternParam { public int sides, copies, winding; public bool isWinding; }
    [Serializable]
    internal class EmptyPatternParam : PatternParam { }

    [Serializable]
    internal class SolidBorderParam : BorderParam { }
    [Serializable]
    internal class DashedBorderParam : BorderParam { public int count; public float thetaOffset, dashLengthModifier; }
    [Serializable]
    internal class SpikesBorderParam : BorderParam { public int count, spikeLength; public double spikeWidth; }
    [Serializable]
    internal class CrossedBorderParam : BorderParam { public int count, hashLength; public double hashWidth; public bool isInner; }
}
