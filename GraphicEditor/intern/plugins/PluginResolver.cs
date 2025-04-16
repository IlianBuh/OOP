using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphicEditor.intern.plugins
{
    class PluginResolver: IPluginResolver
    {
        public PluginResolver() {
            
        }

        public void AddPlugin() {
            MessageBox.Show("Add plugin");
        }
    }
}
