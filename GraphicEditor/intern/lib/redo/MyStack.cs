using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;
using GraphicEditor.intern.lib.redo;

namespace WpfProject
{
    public class RedoResolver<T> : IRedoResolver<T>
    {
        private List<T> _items = new List<T>();
        private Stack<Change<T>> _undoStack = new Stack<Change<T>>();
        private Stack<Change<T>> _redoStack = new Stack<Change<T>>();

        public (List<T> items, bool ok) GetItems()
        {
            return (_items.ToList(), true);
        }

        public void AddItem(T item)
        {
            _undoStack.Push(new Change<T>(ChangeType.Add, item));
            _items.Add(item);
            _redoStack.Clear();
        }

        public bool Undo()
        {
            if (_undoStack.Count == 0)
            {
                return false;
            }

            Change<T> change = _undoStack.Pop();
            _redoStack.Push(change); 

            switch (change.Type)
            {
                case ChangeType.Add:
                    _items.Remove(change.Item);
                    break;
                case ChangeType.Remove:
                    _items.Insert(change.Index, change.Item);
                    break;
            }
            return true;
        }

        public (T val, bool ok) Redo()
        {
            if (_redoStack.Count == 0)
            {
                return (default(T)!, false);
            }

            Change<T> change = _redoStack.Pop();
            _undoStack.Push(change); 

            switch (change.Type)
            {
                case ChangeType.Add:
                    _items.Add(change.Item);
                    return (change.Item, true);
                case ChangeType.Remove:
                    if (change.Index >= 0 && change.Index <= _items.Count) 
                    {
                        _items.RemoveAt(change.Index);
                        return (default(T)!, true);
                    }
                    else
                    {
                         return (default(T)!, false); 
                    }
                   
                default:
                    return (default(T)!, false);
            }
        }
    }

    internal class Change<T>
    {
        public ChangeType Type { get; }
        public T Item { get; }
        public int Index { get; }

        public Change(ChangeType type, T item, int index = -1)
        {
            Type = type;
            Item = item;
            Index = index;
        }
    }

    internal enum ChangeType
    {
        Add,
        Remove,
    }
}
