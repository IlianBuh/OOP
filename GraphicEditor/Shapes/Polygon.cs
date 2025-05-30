﻿using System;
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
    class Polygon : APolyShape
    {
        public Polygon(Point anch, Point endP,
                        Brush Stroke = null, double StrokeThickness = 3, Brush Fill = null)
                : base(anch, endP, Stroke, StrokeThickness, Fill) { }

        
        public override void AddPoint(Point point) {
            this.Points.Add(point);
        }
        public override void Draw(Canvas canvas)
        {
            var polygon = new System.Windows.Shapes.Polygon();

            foreach (var point in this.Points) {
                polygon.Points.Add(point);
            }

            polygon.Stroke = this.Stroke;
            polygon.StrokeThickness = this.StrokeThickness;
            polygon.Fill = this.Fill;

            canvas.Children.Add(polygon);
        }

    }
}
