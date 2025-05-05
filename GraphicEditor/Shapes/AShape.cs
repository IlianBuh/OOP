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
using Newtonsoft.Json;

namespace GraphicEditor
{
    using System.Windows;
    public abstract class AShape
    {
        public Brush Stroke { set; get; }
        public double StrokeThickness { set; get; }
        public Brush Fill { get; set; }
        public Point Anchor;
        public virtual Point EndPoint { get; set; }
        
        protected AShape(Point anch, Point endP,
                         Brush Stroke = null, double StrokeThickness = 3, Brush Fill = null)
        {
            this.Anchor = anch;
            this.EndPoint = endP;
            this.Stroke = Stroke ?? Brushes.Black;
            this.StrokeThickness = StrokeThickness;
            this.Fill = Fill ?? Brushes.Black;
        }
        
        abstract public void Draw(Canvas canvas);

    }
}
