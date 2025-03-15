using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace GraphicEditor.Shapes
{
    class Rectangle : Base2DFigure
    {
        public Rectangle(Point tl, Point rb,
                         Brush Stroke = null, double StrokeThickness = 3, Brush Fill = null)
                : base(tl, rb, Stroke, StrokeThickness, Fill) { }

        public override void Draw(Canvas canvas)
        {
            var rect = new System.Windows.Shapes.Rectangle();

            Canvas.SetLeft(rect, this.Left);
            Canvas.SetTop(rect, this.Top);

            rect.Width = this.Width;
            rect.Height = this.Height;
            rect.Fill = this.Fill;
            rect.Stroke = this.Stroke;
            rect.StrokeThickness = this.StrokeThickness;

            canvas.Children.Add(rect);
        }
    }
}
