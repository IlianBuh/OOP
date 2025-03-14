using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace GraphicEditor.Shapes
{
    class Line: AShape
    {
        public Line(Point tl, Point rb)
            : this(tl, rb, Brushes.Black, 5, Brushes.Black) { }
        public Line(Point tl, Point rb, Brush Stroke)
            : this(tl, rb, Stroke, 5, Brushes.Black) { }
        public Line(Point tl, Point rb, Brush Stroke, double StrokeThickness)
            : this(tl, rb, Stroke, StrokeThickness, Brushes.Black) { }
        public Line(Point tl, Point rb,
                    Brush Stroke, double StrokeThickness, Brush Fill)
                : base(tl, rb, Stroke, StrokeThickness, Fill) { }
        
        public override void Draw(Canvas canvas) {
            var line = new System.Windows.Shapes.Line();

            line.X1 = LeftTop.X;
            line.Y1 = LeftTop.Y;
            line.X2 = RightBottom.X;
            line.Y2 = RightBottom.Y;

            line.Stroke = Stroke;
            line.StrokeThickness = StrokeThickness;

            canvas.Children.Add(line);
        }
    }
}
