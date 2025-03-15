using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace GraphicEditor.Shapes
{
    abstract class APolyShape : AShape
    {

        public List<Point> Points = [];
        public APolyShape(Point anch, Point endP,
                        Brush Stroke = null, double StrokeThickness = 3, Brush Fill = null)
                : base(anch, endP, Stroke, StrokeThickness, Fill)
        {
            this.Points = [anch, endP];
        }
        abstract public void AddPoint(Point point);
    }
}
