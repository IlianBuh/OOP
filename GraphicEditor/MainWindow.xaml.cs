using GraphicEditor.Controls;
using GraphicEditor.intern.plugins;
using GraphicEditor.intern.serializer;
using GraphicEditor.intern.drawer;
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
using System.Windows.Shapes;
using GraphicEditor.intern.lib.redo;
using Microsoft.Win32;
using WpfProject;

namespace GraphicEditor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window//, INotifyPropertyChanged
{

    private Drawer drawer;

    private List<FrameworkElement> previews;
    
    private int currItemColor;
    private IPluginResolver pres;
    private ISerializer serializer;

    public ObservableCollection<string> FigureNames { get; set; }

    public MainWindow()
    {
        InitializeComponent();

        DataContext = this;
        this.drawer = new Drawer(this.myCanvas, new RedoResolver<AShape>());
        this.setInitialFigure();
        this.FigureNames = new ObservableCollection<string>(this.drawer.GetFigureNames());
        this.KeyDown += this.EventCompletePolyShapeDrawing;
        this.Loaded += this.onLoaded;
        
        this.serializer = new JsonSerializerImpl();
        this.pres = new ShapePluginResolver();
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
    
    
    //       ----SERIALIZER----
    private void EventSaveCanvas(object sender, EventArgs e) {
        List<AShape> shapes = this.drawer.GetShapes();
        this.serializer.Serialize( shapes );
    }
    private void EventLoadCanvas(object sender, EventArgs e)
    {
        var shapes = this.serializer.Deserialise();
        this.drawer.DrawList(shapes);
    }

    //       ----PLUGIN RESOLVER----
    private void EventAddPlugin(object sender, EventArgs e) {
        var fpath = this.fetchFilePath(new OpenFileDialog());
        if (fpath == "")
        {
            return;
        }
        var (ctr, name) = this.pres.AddPlugin(fpath);
        this.drawer.AddFigure(ctr, name);
        this.FigureNames.Add(name);
    }


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
        this.drawer.SetStrockThickness(this.brushSizeSlider.Value);
        this.drawer.StartDrawing(e.GetPosition(this.myCanvas));

        this.myCanvas.MouseMove += this.EventDrawingFigure;
        this.myCanvas.MouseUp += EventEndDraw;
    }
    private void EventEndDraw(object sender, MouseButtonEventArgs e)
    {
        var sh = this.drawer.StopDrawing(e.GetPosition(this.myCanvas));
        if (sh == null) {
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
    
    private string fetchFilePath(FileDialog fileDialog)
    {
        fileDialog.Filter = "JSON Files (*.json)|*.json";
        fileDialog.DefaultExt = ".json";
        if (fileDialog.ShowDialog() == true)
        {
            return fileDialog.FileName.ToString();
        }
        else
        {
            return "";
        }

    }
}