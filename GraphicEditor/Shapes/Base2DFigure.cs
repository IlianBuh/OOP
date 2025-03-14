using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace GraphicEditor.Shapes
{
    abstract class Base2DFigure : AShape
    {
        public double Height { get => Math.Abs(EndPoint.Y - Anchor.Y); set => EndPoint.Y = Anchor.Y + value; }
        public double Width { get => Math.Abs(EndPoint.X - Anchor.X); set => EndPoint.X = Anchor.X + value; }
        public double Left { 
            get => this.Anchor.X > this.EndPoint.X ? this.EndPoint.X : this.Anchor.X;
            set => this.EndPoint.X = value;
        }
        public double Top { 
            get => this.Anchor.Y > this.EndPoint.Y ? this.EndPoint.Y : this.Anchor.Y;
            set => this.EndPoint.Y = value;
        }
        public Base2DFigure(Point lt, Point rb,
                    Brush Stroke, double StrokeThickness, Brush Fill)
                : base(lt, rb, Stroke, StrokeThickness, Fill)
        { }
    }
}
