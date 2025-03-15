using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace GraphicEditor.Shapes
{
    class Polyline : APolyShape
    {
        public Polyline(Point anch, Point endP,
                        Brush Stroke = null, double StrokeThickness = 3, Brush Fill = null)
                : base(anch, endP, Stroke, StrokeThickness, Fill)
        { }
        public override void AddPoint(Point point) {
            this.Points.Add(point);
        }

        public override void Draw(Canvas canvas) {
            var polyline = new System.Windows.Shapes.Polyline();

            foreach (var point in this.Points)
            {
                polyline.Points.Add(point);
            }

            polyline.Stroke = this.Stroke;
            polyline.StrokeThickness = this.StrokeThickness;

            canvas.Children.Add(polyline);
        }
    }
}
