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
        private Dictionary<string, ConstructorInfo> mapCtrs;

        public CtrGetter() {
            (this.mapCtrs, this.ctrs) = this.getAllFigureClasses();
            
            this.CurrCtr = null;
        }

        private (Dictionary<string, ConstructorInfo>, List<ConstructorInfo>) getAllFigureClasses()
        {
            List<ConstructorInfo> listFigureCtr = [];
            Dictionary<string, ConstructorInfo> dictFigureCtr = [];

            var assembly = Assembly.GetAssembly(typeof(AShape)) ??
                           throw new Exception("There’s nothing to draw");

            var shapeTypes = assembly.GetTypes()
                                     .Where(t => t != null && t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(AShape)));

            foreach (Type type in shapeTypes)
            {
                var ctor = type.GetConstructor([typeof(Point), typeof(Point), typeof(Brush), typeof(double), typeof(Brush)]);
                listFigureCtr.Add(ctor);
                dictFigureCtr[type.Name] = ctor;
            }

            return (dictFigureCtr,listFigureCtr);
        }

        public void SetCurrCtr(int index) {
            if (index < 0 || index >= this.ctrs.Count) {
                throw new Exception("invalid index");
            }

            this.CurrCtr = this.ctrs[index];
        }
        public (string, bool) SetCurrCtr(string name)
        {
            string msg = $"a shape with the name \"{name}\" not found";
            if (this.mapCtrs.ContainsKey(name))
            { 
                this.CurrCtr = this.mapCtrs[name];
                return ("", true);
            }

            return (msg, false);
        }

        public bool AddCtr(ConstructorInfo ctr, string name)
        {
            this.ctrs.Add(ctr);
            this.mapCtrs[name] = ctr;
            return true;
        }
        public List<string> GetCtrNames() {
            return this.ctrs.Select(item => item.DeclaringType.Name).ToList();
        }
    }
}
