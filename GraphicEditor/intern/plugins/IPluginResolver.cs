using System.Reflection;

namespace GraphicEditor.intern.plugins;

public interface IPluginResolver
{
    public (ConstructorInfo, string) AddPlugin(string pluginPath);
}