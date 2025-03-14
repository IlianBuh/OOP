using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace GraphicEditor.Shapes
{
    class Polyline : Base2DFigure
    {
        public List<Point> Points;
        public Polyline(Point tl, Point rb,
                        Brush Stroke = null, double StrokeThickness = 3, Brush Fill = null)
                : base(tl, rb, Stroke, StrokeThickness, Fill) {
            this.Points = new List<Point>();
        }

        public override void Draw(Canvas canvas)
        {
            var polyline = new System.Windows.Shapes.Polyline();

            foreach (var point in this.Points) {
                polyline.Points.Add(point);
            }

            polyline.Stroke = this.Stroke;
            polyline.StrokeThickness = this.StrokeThickness;
            polyline.Fill = this.Fill;

            canvas.Children.Add(polyline);
        }

    }
}
