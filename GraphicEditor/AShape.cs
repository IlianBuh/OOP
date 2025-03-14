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
    abstract class AShape
    {
        public System.Windows.Media.Brush Stroke { set; get; }
        public double StrokeThickness { set; get; }
        public System.Windows.Media.Brush Fill { get; set; }
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
