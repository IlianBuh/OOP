using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;
using GraphicEditor.Shapes;
using System.Windows.Input;
namespace GraphicEditor
{
    class Drawer
    {
        private bool isDrawing = false;
        private AShape currentFigure;
        private Canvas myCanvas;
        private MyStack<System.Windows.Shapes.Shape> redoStack;
        private bool isPolyShape;
        private bool stopPolyShape = false;
        public Drawer(Canvas canvas)
        {
            this.myCanvas = canvas;
            this.redoStack = new();
        }

        public void StartDrawing(Point startPoint, ConstructorInfo constructor, Brush Strocke, double StrockeThickness, Brush Fill) {
            if (!isDrawing)
            {
                this.isDrawing = true;
                this.currentFigure = (AShape)constructor.Invoke([startPoint, startPoint, Strocke, StrockeThickness, Fill]);
                this.isPolyShape = this.currentFigure is APolyShape;
                this.drawFigure();
            }
        }
        public void StopDrawing(Point point) {

            this.stopPolyShape = false;
            this.CompletePolyShapeDrawing();
            if (isPolyShape && !stopPolyShape) {
                this.addPointToPolygon(point);
                return;
            }
            this.isDrawing = false;
            this.currentFigure = null;
            this.redoStack.Clear();
            this.stopPolyShape = false;
        }
        public void UpdateFigure(Point endPoint) {
            if (!isDrawing) return;

            if (this.myCanvas.Children.Count > 0)
            {
                if (this.isPolyShape) {
                    this.addPointToPolygon(endPoint);
                }
                else { 
                    this.currentFigure.EndPoint = endPoint;
                }
                
                this.myCanvas.Children.RemoveAt(this.myCanvas.Children.Count - 1);

            }
            this.drawFigure();
        }
        public void CompletePolyShapeDrawing() {
            if (this.isDrawing && this.isPolyShape) { 
                var currFigure = (this.currentFigure as APolyShape);
                this.stopPolyShape = true;
                currFigure.Points.Remove(currFigure.Points.Last());
                this.myCanvas.Children.RemoveAt(this.myCanvas.Children.Count - 1);
                currFigure.Draw(this.myCanvas);

                var lastItem = this.myCanvas.Children[^1];

                this.StopDrawing(new Point());
            }
        }

        public void Undo() {
            var c = this.myCanvas.Children.Count - 1;
            if (c >= 0)
            {
                this.redoStack.AddShape(this.myCanvas.Children[c] as System.Windows.Shapes.Shape);
                this.myCanvas.Children.RemoveAt(c);
            }
        }
        public void Redo() { 
            var res = this.redoStack.GetShape();
            if (res.ok) {
                this.myCanvas.Children.Add(res.shape);
            } else {
                MessageBox.Show("No shape in history.");
            }
        }

        private void drawFigure() {
            this.currentFigure.Draw(this.myCanvas);
        }
        private void addPointToPolygon(Point point)
        {
            (this.currentFigure as APolyShape).AddPoint(point);
        }
        
    }
}
