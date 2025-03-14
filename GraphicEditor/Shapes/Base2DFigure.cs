using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace GraphicEditor.Shapes
{
    abstract class Base2DFigure : AShape
    {
        public Base2DFigure(Point lt, Point rb,
                    Brush Stroke, double StrokeThickness, Brush Fill)
                : base(lt, rb, Stroke, StrokeThickness, Fill)
        {
            (this.LeftTop.X, this.RightBottom.X) = lt.X > rb.X ? (rb.X, lt.X) : (this.LeftTop.X, this.RightBottom.X);
            (this.LeftTop.Y, this.RightBottom.Y) = lt.Y > rb.Y ? (rb.Y, lt.Y) : (this.LeftTop.Y, this.RightBottom.Y);
        }
    }
}
