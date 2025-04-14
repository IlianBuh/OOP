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
using GraphicEditor.intern.lib.redo;
namespace GraphicEditor.intern.drawer
{
    class Drawer
    {
        const int FILL_COLOR_INDEX = 0;
        const int STROKE_COLOR_INDEX = 1;

        private bool isDrawing = false;

        private List<Color> colors = [Colors.White, Colors.Black];

        private lib.ctrGetter.CtrGetter ctrGetter { get; set; }
        private AShape currentFigure;
        private Canvas myCanvas;
        private lib.redo.IRedoResolver<System.Windows.Shapes.Shape> redoStack;

        private bool isPolyShape;
        private bool stopPolyShape = false;

        public Drawer(Canvas canvas)
        {
            this.myCanvas = canvas;
            this.ctrGetter = new();
            this.redoStack = new MyStack<System.Windows.Shapes.Shape>();
        }

        public void StartDrawing(Point startPoint) {
            if (!isDrawing)
            {
                this.isDrawing = true;
                this.currentFigure = (AShape)this.ctrGetter.CurrCtr.Invoke(
                    [startPoint, startPoint, 
                    new SolidColorBrush(this.colors[STROKE_COLOR_INDEX]), 5, new SolidColorBrush(colors[FILL_COLOR_INDEX])]
                );
                this.isPolyShape = this.currentFigure is APolyShape;
                drawFigure();
            }
        }
        public bool StopDrawing(Point point, bool isKeyPressed = false) {


            CompletePolyShapeDrawing(isKeyPressed);
            if (isPolyShape && !stopPolyShape) {
                addPointToPolygon(point);
                return false;
            }
            isDrawing = false;
            currentFigure = null;
            stopPolyShape = false;
            redoStack.Clear();
            return true;
        }
        public void UpdateFigure(Point endPoint) {
            if (!isDrawing) return;

            if (myCanvas.Children.Count > 0)
            {
                
                myCanvas.Children.RemoveAt(myCanvas.Children.Count - 1);
                currentFigure.EndPoint = endPoint;

            }
            drawFigure();
        }
        private void CompletePolyShapeDrawing(bool isKeyPressed) {
            if (isDrawing && isPolyShape && isKeyPressed)
            {
                var currFigure = currentFigure as APolyShape;
                stopPolyShape = true;
                currFigure.Points.Remove(currFigure.Points.Last());
                myCanvas.Children.RemoveAt(myCanvas.Children.Count - 1);
                currFigure.Draw(myCanvas);

                var lastItem = myCanvas.Children[^1];

            }

        }

        public void Undo() {
            var c = myCanvas.Children.Count - 1;
            if (c >= 0)
            {
                redoStack.AddShape(myCanvas.Children[c] as System.Windows.Shapes.Shape);
                myCanvas.Children.RemoveAt(c);
            }
        }
        public void Redo() { 
            var res = redoStack.GetShape();
            if (res.ok) {
                myCanvas.Children.Add(res.shape);
            } else {
                MessageBox.Show("No shape in history.");
            }
        }

        private void drawFigure() {
            currentFigure.Draw(myCanvas);
        }
        private void addPointToPolygon(Point point)
        {
            (currentFigure as APolyShape).AddPoint(point);
        }

        public List<string> GetFigureNames() {
            return this.ctrGetter.GetCtrNames();
        }
        public void SetFigure(int ind) {
            this.ctrGetter.SetCurrCtr(ind);
        }

        public void SetColor(int ind, Color color) {
            this.colors[ind] = color;
        }
    }
}
