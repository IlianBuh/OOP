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
        public Line(Point tl, Point rb,
                    Brush Stroke = null, double StrokeThickness = 3, Brush Fill = null)
                : base(tl, rb, Stroke, StrokeThickness, Fill) { }
        
        public override void Draw(Canvas canvas) {
            var line = new System.Windows.Shapes.Line();

            line.X1 = Anchor.X;
            line.Y1 = Anchor.Y;
            line.X2 = EndPoint.X;
            line.Y2 = EndPoint.Y;

            line.Stroke = Stroke;
            line.StrokeThickness = StrokeThickness;

            canvas.Children.Add(line);
        }
    }
}
