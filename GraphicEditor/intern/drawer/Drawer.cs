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
        public const int FILL_COLOR_INDEX = 0;
        public const int STROKE_COLOR_INDEX = 1;
        
        private bool isDrawing = false;

        private List<Color> colors = [Colors.White, Colors.Black];
        private double strokeThickness = 5;

        private lib.ctrGetter.CtrGetter ctrGetter { get; set; }
        private AShape currentFigure;
        private Canvas myCanvas;
        private lib.redo.IRedoResolver<GraphicEditor.AShape> redoStack;

        private bool isPolyShape;
        private bool stopPolyShape = false;

        public Drawer(Canvas canvas, lib.redo.IRedoResolver<GraphicEditor.AShape> redoStack)
        {
            this.myCanvas = canvas;
            this.ctrGetter = new();
            this.redoStack = redoStack;
        }

        public void StartDrawing(Point startPoint) {
            if (!isDrawing)
            {
                this.isDrawing = true;
                this.currentFigure = (AShape)this.ctrGetter.CurrCtr.Invoke(
                    [startPoint, startPoint, 
                    new SolidColorBrush(this.colors[STROKE_COLOR_INDEX]), this.strokeThickness, new SolidColorBrush(colors[FILL_COLOR_INDEX])]
                );
                this.isPolyShape = this.currentFigure is APolyShape;
                drawFigure();
            }
        }
        public AShape? StopDrawing(Point point, bool isKeyPressed = false) {


            CompletePolyShapeDrawing(isKeyPressed);
            if (isPolyShape && !stopPolyShape) {
                addPointToPolygon(point);
                return null;
            }
            isDrawing = false;
            var res = this.currentFigure;
            stopPolyShape = false;
            if (this.currentFigure != null)
                redoStack.AddItem(this.currentFigure);
            currentFigure = null;
            return res;
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
                redoStack.Undo();
                myCanvas.Children.RemoveAt(c);
            }
        }
        public void Redo() { 
            var res = redoStack.Redo();
            if (res.ok) {
                this.DrawList([res.val]);
            } else {
                MessageBox.Show("No shape in history.");
            }
        }

        private void drawFigure() {
            currentFigure.Draw(myCanvas);
        }

        public void DrawList(List<AShape> shapes)
        {
            if (shapes == null)
                return;
            foreach (var shape in shapes)
            {
                this.ctrGetter.SetCurrCtr(shape.ToString()
                                                .Substring(shape.ToString()
                                                .LastIndexOf('.')+1)
                );
                this.SetStrockThickness(shape.StrokeThickness);
                this.SetColor(Drawer.FILL_COLOR_INDEX, ((SolidColorBrush)shape.Fill).Color);
                this.SetColor(Drawer.STROKE_COLOR_INDEX, ((SolidColorBrush)shape.Stroke).Color);
                this.currentFigure = (AShape)this.ctrGetter.CurrCtr.Invoke(
                    [shape.Anchor, shape.Anchor,
                        new SolidColorBrush(this.colors[STROKE_COLOR_INDEX]), this.strokeThickness, new SolidColorBrush(colors[FILL_COLOR_INDEX])]
                );
                this.StartDrawing(shape.Anchor);
                this.UpdateFigure(shape.EndPoint);
                var sh = this.StopDrawing(shape.EndPoint, shape is APolyShape);
            
            }
        }

        private void addPointToPolygon(Point point)
        {
            (currentFigure as APolyShape).AddPoint(point);
        }

        public List<AShape> GetShapes()
        {
            var (items, ok) = this.redoStack.GetItems();
            if (!ok)
            {
                return [];
            }
            return items;
        }
        public List<string> GetFigureNames() {
            return this.ctrGetter.GetCtrNames();
        }
        public void SetFigure(int ind) {
            this.ctrGetter.SetCurrCtr(ind);
        }

        public void AddFigure(ConstructorInfo ctr, string name)
        {
            this.ctrGetter.AddCtr(ctr, name);
        }
        public void SetStrockThickness(double thickness)
        {
            this.strokeThickness = thickness;
        }
        public void SetColor(int ind, Color color) {
            this.colors[ind] = color;
        }
    }
}
