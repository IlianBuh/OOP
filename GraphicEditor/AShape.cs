using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GraphicEditor
{
    using System.Windows;
    abstract class AShape
    {
        public System.Windows.Media.Brush Stroke { set; get; }
        public double StrokeThickness { set; get; }
        public System.Windows.Media.Brush Fill { get; set; }
        public double Height { get => RightBottom.Y - LeftTop.Y; set => RightBottom.Y = LeftTop.Y + value; }
        public double Width { get => RightBottom.X - LeftTop.X; set => RightBottom.X = LeftTop.X + value; }
        public Point LeftTop;
        public Point RightBottom;
        
        protected AShape(Point tl, Point rb,
                         System.Windows.Media.Brush Stroke, double StrokeThickness, System.Windows.Media.Brush Fill)
        {
            this.LeftTop = tl;
            this.RightBottom = rb;
            this.Stroke = Stroke;
            this.StrokeThickness = StrokeThickness;
            this.Fill = Fill;
        }
        abstract public void Draw(Canvas canvas);

    }
}
