using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicEditor.intern.lib.redo
{
    interface IRedoResolver<T>
    {
        public (T shape, bool ok) GetShape();
        public void AddShape(T shape);
        public void Clear();

    }
}
