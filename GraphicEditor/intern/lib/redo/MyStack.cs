using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;
using GraphicEditor;
using GraphicEditor.intern.lib.redo;

namespace WpfProject
{
    public class RedoResolver<T> : IRedoResolver<T>
    {
        private List<T> _items = new List<T>();
        private Stack<T> _undoStack = new Stack<T>();
        private Stack<T> _redoStack = new Stack<T>();

        public (List<T> items, bool ok) GetItems()
        {
            return (_items.ToList(), true);
        }

        public void AddItem(T item)
        {
            _undoStack.Push(item);
            _items.Add(item);
            _redoStack.Clear();
        }

        public bool Undo()
        {
            if (_undoStack.Count == 0)
            {
                return false;
            }

            T change = _undoStack.Pop();
            _items.Remove(change);
            _redoStack.Push(change); 

            return true;
        }

        public (T val, bool ok) Redo()
        {
            if (_redoStack.Count == 0)
            {
                return (default(T)!, false);
            }

            T change = _redoStack.Pop();
            _undoStack.Push(change); 
            
            _items.Add(change);
            return (change, true);
        }
    }
}
