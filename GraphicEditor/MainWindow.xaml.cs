using GraphicEditor.Shapes;
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
    public MainWindow()
    {
        InitializeComponent();
        Line line = new Line(new Point(10, 10), new Point(100, 100), Brushes.BlanchedAlmond, 50);
        line.Draw(this.myCanvas);
    }
}