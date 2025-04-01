using GraphicEditor.Controls;
using GraphicEditor.@internal.drawer;
using GraphicEditor.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
public partial class MainWindow : Window//, INotifyPropertyChanged
{

    private Drawer drawer;

    // TODO : implement preview color rectangles
    private List<FrameworkElement> previews = new (32);
    private int currItemColor;

    public ObservableCollection<string> FigureNames { get; set; }

    public MainWindow()
    {
        InitializeComponent();

        DataContext = this;
        this.drawer = new Drawer(this.myCanvas);
        this.setInitialFigure();
        this.FigureNames = new ObservableCollection<string>(this.drawer.GetFigureNames());
        this.KeyDown += this.EventCompletePolyShapeDrawing;
        this.Loaded += this.onLoaded;
    }
    private void onLoaded(object sender, RoutedEventArgs e){
        this.previews = this.getPreviews(this.toolBar, new());
    }
    private List<FrameworkElement>  getPreviews(DependencyObject parent, List<FrameworkElement> res) {

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++) { 
            var child = VisualTreeHelper.GetChild(parent, i);

            if (child is FrameworkElement frameworkElement) {
                if (!string.IsNullOrEmpty(frameworkElement.Name) &&
                    frameworkElement.Name.IndexOf("preview") == 0) {
                    res.Add(frameworkElement);
                }
            }

            getPreviews(child, res);
        }

        return res;
    }

    // ----COMBO BOX HANDLING----
    private void setInitialFigure()
    {
        this.cmbTools.SelectedIndex = 0;
        this.setCurrentFigure(0);
    }
    private void setCurrentFigure(int index)
    {
        this.drawer.SetFigure(index);
    }

    // ----EVENTS-----
    
    //       ----COLOR PICKER----
    private void EventShowColorChoosingPanel(object sender, RoutedEventArgs e)
    {
        this.colorPopup.IsOpen = true;
        this.currItemColor = this.parseNumberAtEnd((sender as FrameworkElement).Name);
    }
    private void EventMouseMoveColorPicker(object sender, MouseEventArgs e)
    {
        this.changeOnColorPickerValue();
    }
    private void EventMouseDownColorPicker(object sender, MouseEventArgs e)
    {
        this.changeOnColorPickerValue();
        (sender as Popup).MouseMove += EventMouseMoveColorPicker;
    }
    private void EventMouseUpColorPicker(object sender, MouseEventArgs e)
    {
        this.colorPicker.MouseMove -= EventMouseMoveColorPicker;
        this.colorPopup.IsOpen = false;
    }
    private void EventCloseColorFillPopup(object sender, EventArgs args) {
        this.drawer.SetColor(currItemColor, this.colorPicker.SelectedColor);
    }
    private void EventMouseLeaveColorPicker(object sender, MouseEventArgs e)
    {
        this.colorPicker.MouseMove -= EventMouseMoveColorPicker;
    }
    public int parseNumberAtEnd(string input)
    {
        int i = input.Length - 1;
        while (i >= 0 && char.IsDigit(input[i]))
        {
            i--;
        }

        string numberStr = input.Substring(i + 1);
        return int.Parse(numberStr);
    }
    private void changeOnColorPickerValue()
    {
        Color color = this.colorPicker.SelectedColor;
        if (this.previews[this.currItemColor] is System.Windows.Shapes.Rectangle rect) {
            rect.Fill = new SolidColorBrush(color);
        }
    }


    //       ----DRAWING----
    private void EventStartDraw(object sender, MouseButtonEventArgs e)
    {
        this.drawer.StartDrawing(e.GetPosition(this.myCanvas));

        this.myCanvas.MouseMove += this.EventDrawingFigure;
        this.myCanvas.MouseUp += EventEndDraw;
    }
    
    private void EventEndDraw(object sender, MouseButtonEventArgs e)
    {
        if (!this.drawer.StopDrawing(e.GetPosition(this.myCanvas))) {
            return;
        }

        this.myCanvas.MouseMove -= this.EventDrawingFigure;
        this.myCanvas.Focus();
    }
    private void EventDrawingFigure(object sender, MouseEventArgs e)
    {
        this.drawer.UpdateFigure(e.GetPosition(this.myCanvas));
        this.myCanvas.Children[^1].MouseUp += EventEndDraw;
    }
    private void EventNewFigureSelected(object sender, SelectionChangedEventArgs e)
    {
        this.drawer.SetFigure(this.cmbTools.SelectedIndex);
    }
    private void EventCompletePolyShapeDrawing(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.drawer.StopDrawing(new Point(), true);
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

    private void colorFillPopup_Closed(object sender, EventArgs e)
    {

    }
}