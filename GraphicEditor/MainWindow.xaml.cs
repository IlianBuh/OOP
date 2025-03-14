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
using System.Windows.Shapes;

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

        AShape e = (AShape)(this.currentFigureConstructor.Invoke([new Point(10, 10), new Point(100,100), null, 3, null]));
        e.Draw(this.myCanvas);
    }

    private void initAllFigureList() {
        this.getAllFigureClasses();
        this.addFiguresNameInDropList();
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
    private void setInitialFigure() {
        this.cmbTools.SelectedIndex = 0;
        this.setCurrentFigure(0);
    }
    private void addFiguresNameInDropList() {
        foreach (var constructor in this.figureConstructors) {

            this.addComboBoxItem( constructor.DeclaringType.Name );
        }
    }
    private void addComboBoxItem(string name) {
        var comboBoxItem = new ComboBoxItem { Tag = name, Content = name };

        this.cmbTools.Items.Add(comboBoxItem);
    }
    private void setCurrentFigure(int index) {
        this.currentFigureConstructor = this.figureConstructors[index];
    }
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
    }
    private void EventDrawingFigure(object sender, MouseEventArgs e) {

        var mousePosition = e.GetPosition(this.myCanvas);

        if (this.myCanvas.Children.Count > 0) {
            this.myCanvas.Children.RemoveAt(this.myCanvas.Children.Count-1);
            this.currentFigure.EndPoint.X = mousePosition.X;
            this.currentFigure.EndPoint.Y = mousePosition.Y;
        }

        this.drawFigure();
    }
    private void EventNewFigureSelected(object sender, SelectionChangedEventArgs e) {
        if (this.cmbTools.SelectedItem is ComboBoxItem selectedItem)
        {
            this.currentFigureConstructor = this.figureConstructors[this.cmbTools.SelectedIndex];
        }
    }

    private void drawFigure() {
        this.currentFigure.Draw(this.myCanvas);
        this.myCanvas.Children[this.myCanvas.Children.Count-1].MouseDown += 
            (object sender, MouseButtonEventArgs e) => myCanvas.RaiseEvent(new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left)
            {
                RoutedEvent = Canvas.MouseUpEvent,
                Source = sender
            });

    }
}