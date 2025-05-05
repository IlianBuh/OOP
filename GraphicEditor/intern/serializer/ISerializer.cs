namespace GraphicEditor.intern.serializer;


public interface ISerializer
{ 
    public void Serialize(List<AShape> shapes);
    public List<AShape>? Deserialise();

}