using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace GraphicEditor.Shapes
{
    public abstract class APolyShape : AShape
    {

        public List<Point> Points = [];
        public APolyShape(Point anch, Point endP,
                        Brush Stroke = null, double StrokeThickness = 3, Brush Fill = null)
                : base(anch, endP, Stroke, StrokeThickness, Fill)
        {
            this.Points = [anch, endP];
        }
        abstract public void AddPoint(Point point);
        public override Point EndPoint
        {
            get => this.Points.Count > 0 ? this.Points[^1] : new Point();
            set
            {
                if (this.Points.Count > 0)
                    this.Points[^1] = value;
                else
                    this.Points.Add(value);
            }
        }
    }
}
