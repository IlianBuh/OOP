using System.Windows;

namespace WpfApp1;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls; // Required for Canvas
using System.Windows.Shapes;   // Required for Polygon
using GraphicEditor;            // Add this to use AShape, Base2DFigure, etc.
using GraphicEditor.Shapes;  //  and this



public class Triangle : Base2DFigure
{
    public Triangle(Point anch, Point endP,
        Brush Stroke = null, double StrokeThickness = 3, Brush Fill = null)
        : base(anch, endP, Stroke, StrokeThickness, Fill) { }

    
    public override void Draw(Canvas canvas)
    {
        var polygon = new System.Windows.Shapes.Polygon();
        
        polygon.Stroke = this.Stroke;
        polygon.StrokeThickness = this.StrokeThickness;
        polygon.Fill = this.Fill;
        // Calculate the points of the triangle based on Anchor and EndPoint
        Point point1 = Anchor;
        Point point2 = new Point(EndPoint.X, Anchor.Y);
        Point point3 = new Point((Anchor.X + EndPoint.X) / 2, EndPoint.Y);

        polygon.Points = new PointCollection() { point1, point2, point3 };

        canvas.Children.Add(polygon);
    }

}