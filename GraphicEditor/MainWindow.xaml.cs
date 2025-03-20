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

    private Drawer drawer;
    private int currChangingColor;
    private System.Windows.Shapes.Rectangle[] previewsFillStroke;
    private Color[] colorsFillStroke = [Colors.White, Colors.Black];

    private List<ConstructorInfo> figureConstructors = [];
    private ConstructorInfo currentFigureConstructor = null;

    public MainWindow()
    {
        InitializeComponent();

        this.initAllFigureList();
        this.drawer = new Drawer(this.myCanvas);
        this.KeyDown += EventCompletePolyShapeDrawing;
        this.previewsFillStroke = [this.fillColorPreview, this.strokeColorPreview];
    }

    // ----COMBO BOX HANDLING----
    private void initAllFigureList()
    {
        this.getAllFigureClasses();
        this.setComboBoxItems();
        this.setInitialFigure();
    }
    private void getAllFigureClasses()
    {
        var assembly = Assembly.GetAssembly(typeof(AShape)) ??
                       throw new Exception("There’s nothing to draw");

        var shapeTypes = assembly.GetTypes()
                                 .Where(t => t != null && t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(AShape)));

        foreach (Type type in shapeTypes)
        {
            var ctor = type.GetConstructor([typeof(Point), typeof(Point), typeof(Brush), typeof(double), typeof(Brush)]);
            this.figureConstructors.Add(ctor);
        }
    }
    private void setComboBoxItems()
    {
        this.cmbTools.ItemsSource = this.figureConstructors.Select(item => item.DeclaringType.Name).ToList();
    }
    private void setInitialFigure()
    {
        this.cmbTools.SelectedIndex = 0;
        this.setCurrentFigure(0);
    }
    private void setCurrentFigure(int index)
    {
        this.currentFigureConstructor = this.figureConstructors[index];
    }

    // ----EVENTS-----
    //       ----COLOR PICKER----
    private void EventShowColorChoosingPanel(object sender, RoutedEventArgs e)
    {
        ColorPopup.IsOpen = true;
        switch ((sender as Button).Name)
        {
            case "btnColorPicker1":
                this.currChangingColor = 0;
                break;

            case "btnColorPicker2":
                this.currChangingColor = 1;
                break;
        }
    }
    private void EventMouseMoveColorPicker(object sender, MouseEventArgs e)
    {
        this.changeOnColorPickerValue();
    }
    private void EventMouseDownColorPicker(object sender, MouseEventArgs e)
    {
        this.changeOnColorPickerValue();
        this.ColorPicker.MouseMove += EventMouseMoveColorPicker;
    }

    private void changeOnColorPickerValue()
    {
        Color color = this.ColorPicker.SelectedColor;
        this.colorsFillStroke[this.currChangingColor] = color;
        this.previewsFillStroke[this.currChangingColor].Fill = new SolidColorBrush(color);
    }
    private void EventMouseLeaveColorPicker(object sender, MouseEventArgs e)
    {
        this.ColorPicker.MouseMove -= EventMouseMoveColorPicker;
    }
    //       ----DRAWING----
    private void EventStartDraw(object sender, MouseButtonEventArgs e)
    {
        this.drawer.StartDrawing(e.GetPosition(this.myCanvas), this.currentFigureConstructor,
            new SolidColorBrush(this.colorsFillStroke[1]), this.brushSizeSlider.Value, new SolidColorBrush(this.colorsFillStroke[0]));
        this.myCanvas.MouseMove += this.EventDrawingFigure;
        this.myCanvas.Children[^1].MouseUp += EventEndDraw;
    }
    
    private void EventEndDraw(object sender, MouseButtonEventArgs e)
    {
        this.drawer.StopDrawing(e.GetPosition(this.myCanvas));

        this.myCanvas.MouseMove -= this.EventDrawingFigure;
        this.myCanvas.Focus();
    }
    private void EventDrawingFigure(object sender, MouseEventArgs e)
    {
        this.drawer.UpdateFigure(e.GetPosition(this.myCanvas));
    }
    private void EventNewFigureSelected(object sender, SelectionChangedEventArgs e)
    {
        this.currentFigureConstructor = this.figureConstructors[this.cmbTools.SelectedIndex];
    }
    private void EventCompletePolyShapeDrawing(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.drawer.CompletePolyShapeDrawing();
        }
    }

    // ----OTHER----


    private void btnUndo_Click(object sender, RoutedEventArgs e)
    {
        this.drawer.Undo();
    }

    private void btnRedo_Click(object sender, RoutedEventArgs e) {
        this.drawer.Redo();
    }
}