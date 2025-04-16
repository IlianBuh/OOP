namespace GraphicEditor.intern.serializer;

public interface ISerializer
{
    public void SaveCanvas(System.Windows.Controls.Canvas canvas);
    public void LoadFile(System.Windows.Controls.Canvas canvas);
}