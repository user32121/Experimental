using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicCircles
{
    internal static class ExtensionMethods
    {
        public static PointF Scale(this PointF p, float scaleX, float scaleY)
        {
            return new PointF(p.X * scaleX, p.Y * scaleY);
        }
        public static PointF Scale(this PointF p, float scalar)
        {
            return Scale(p, scalar, scalar);
        }
        public static PointF Scale(this PointF p, SizeF scale)
        {
            return Scale(p, scale.Width, scale.Height);
        }
    }
}
