using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicEditor.intern.lib.redo
{
    interface IRedoResolver<T>
    {
        public (List<T> items, bool ok) GetItems();
        public bool Undo();
        public (T val, bool ok) Redo();
        public void AddItem(T shape);

    }
}
