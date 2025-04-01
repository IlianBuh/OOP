using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace GraphicEditor.intern.lib.ctrGetter
{
    class CtrGetter
    {

        public ConstructorInfo CurrCtr { get; set; }

        private List<ConstructorInfo> ctrs;

        public CtrGetter() {
            this.ctrs = this.getAllFigureClasses();
            this.CurrCtr = null;
        }

        private List<ConstructorInfo> getAllFigureClasses()
        {
            List<ConstructorInfo> figureConstructors = [];

            var assembly = Assembly.GetAssembly(typeof(AShape)) ??
                           throw new Exception("There’s nothing to draw");

            var shapeTypes = assembly.GetTypes()
                                     .Where(t => t != null && t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(AShape)));

            foreach (Type type in shapeTypes)
            {
                var ctor = type.GetConstructor([typeof(Point), typeof(Point), typeof(Brush), typeof(double), typeof(Brush)]);
                figureConstructors.Add(ctor);
            }

            return figureConstructors;
        }

        public void SetCurrCtr(int index) {
            if (index < 0 || index >= this.ctrs.Count) {
                throw new Exception("invalid index");
            }

            this.CurrCtr = this.ctrs[index];
        }

        public List<string> GetCtrNames() {
            return this.ctrs.Select(item => item.DeclaringType.Name).ToList();
        }
    }
}
