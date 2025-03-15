using GraphicEditor.Shapes;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace GraphicEditor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<ConstructorInfo> figureConstructors = [];
    private ConstructorInfo currentFigureConstructor = null;
    private AShape currentFigure = null;

    bool isDrawing = false;
    public MainWindow()
    {
        InitializeComponent();
        
        this.initAllFigureList();
        this.KeyDown += EventCompletePolyShapeDrawing;
    }

    // ----COMBO BOX HANDLING----
    private void initAllFigureList() {
        this.getAllFigureClasses();
        this.setComboBoxItems();
        this.setInitialFigure();
    }
    private void getAllFigureClasses() {
        var assembly = Assembly.GetAssembly(typeof(AShape)) ?? 
                       throw new Exception("There’s nothing to draw");

        var shapeTypes = assembly.GetTypes()
                                 .Where(t => t != null && t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(AShape)));

        foreach (Type type in shapeTypes) {
            var ctor = type.GetConstructor([typeof(Point), typeof(Point), typeof(Brush), typeof(double), typeof(Brush)]);
            this.figureConstructors.Add(ctor);
        }
    }
    private void setComboBoxItems() {
        this.cmbTools.ItemsSource = this.figureConstructors.Select(item => item.DeclaringType.Name).ToList();
    }
    private void setInitialFigure() {
        this.cmbTools.SelectedIndex = 0;
        this.setCurrentFigure(0);
    }
    private void setCurrentFigure(int index) {
        this.currentFigureConstructor = this.figureConstructors[index];
    }

    // ----EVENTS-----
    private void EventStartDraw(object sender, MouseButtonEventArgs e) {
        if (!isDrawing) {
            this.isDrawing = true;
            var mousePosition = e.GetPosition(this.myCanvas);
            this.currentFigure = (AShape)this.currentFigureConstructor.Invoke([mousePosition, 
                                                                               mousePosition, null, 3, null]);
            
            this.myCanvas.MouseMove += this.EventDrawingFigure;
            this.drawFigure();
        }
    }
    private void EventEndDraw(object sender, MouseButtonEventArgs e) {
        this.myCanvas.MouseMove -= this.EventDrawingFigure;
        this.isDrawing = false;
        this.currentFigure = null;
        this.myCanvas.Focus();
    }
    private void EventDrawingFigure(object sender, MouseEventArgs e) {

        var mousePosition = e.GetPosition(this.myCanvas);

        if (this.myCanvas.Children.Count > 0) {
            this.myCanvas.Children.RemoveAt(this.myCanvas.Children.Count-1);
            this.currentFigure.EndPoint = mousePosition;
        }

        this.drawFigure();
    }
    private void EventNewFigureSelected(object sender, SelectionChangedEventArgs e) {
        this.currentFigureConstructor = this.figureConstructors[this.cmbTools.SelectedIndex];
    }
    private void EventAddPointToPolygon(object sender, MouseButtonEventArgs e) {
        (this.currentFigure as APolyShape).AddPoint(e.GetPosition(this.myCanvas));
    }
    private void EventCompletePolyShapeDrawing(object sender, KeyEventArgs e) {
        if (e.Key == Key.Escape && this.isDrawing) {
            
            var currFigure = (this.currentFigure as APolyShape);
            
            currFigure.Points.Remove(currFigure.Points.Last());
            this.myCanvas.Children.RemoveAt(this.myCanvas.Children.Count - 1);
            currFigure.Draw(this.myCanvas);

            var lastItem = this.myCanvas.Children[^1];
           
            this.EventEndDraw(lastItem, null);
        }
    }
    private void setFigureEventHandlers(UIElement currItemOnCanvas) {

        currItemOnCanvas.MouseUp += (this.currentFigure is APolyShape) ? this.EventAddPointToPolygon :
                                                                         this.EventEndDraw;
        
    }
    
    // ----OTHER----
    private void drawFigure() {
        this.currentFigure.Draw(this.myCanvas);
        this.setFigureEventHandlers(this.myCanvas.Children[^1]);       
    }


}