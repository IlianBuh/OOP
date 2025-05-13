using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GraphicEditor.intern.plugins
{
    public class ShapePluginResolver : IPluginResolver
    {

        public ShapePluginResolver()
        {
        }

        public (ConstructorInfo, string) AddPlugin(string pluginPath)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(pluginPath);
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (typeof(AShape).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(Point), typeof(Point), typeof(Brush), typeof(double), typeof(Brush) });
                        if (constructor != null)
                        {
                            AShape shape = (AShape)constructor.Invoke(new object[]
                            {
                                new Point(0, 0), new Point(100, 100), Brushes.Black, 1.0, Brushes.Transparent
                            });
                            return (constructor, type.Name);
                        }
                        else
                        {
                            constructor = type.GetConstructor(Type.EmptyTypes);
                            if (constructor != null)
                            {
                                AShape shape = (AShape)constructor.Invoke(Array.Empty<object>());
                                return (constructor, type.Name);
                            }
                            else
                            {
                                throw new Exception($"No suitable constructor found in plugin: {type.FullName}");
                            }

                        }
                    }
                }
                throw new Exception("No valid AShape implementation found in plugin.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating shape from plugin: {ex.Message}");
                return (null, null);
            }
        }
    }
}
