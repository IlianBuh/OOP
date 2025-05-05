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

        /// <summary>
        /// Возвращает текущий список элементов.
        /// </summary>
        /// <returns>Кортеж, содержащий список элементов и признак успеха.</returns>
        public (List<T> items, bool ok) GetItems()
        {
            return (_items.ToList(), true);
        }

        /// <summary>
        /// Добавляет элемент в список и сохраняет изменение для отмены.
        /// </summary>
        /// <param name="item">Добавляемый элемент.</param>
        public void AddItem(T item)
        {
            _undoStack.Push(new Change<T>(ChangeType.Add, item));
            _items.Add(item);
            _redoStack.Clear();
        }

        /// <summary>
        /// Отменяет последнее действие, восстанавливая предыдущее состояние.
        /// </summary>
        /// <returns>True, если отмена выполнена успешно, иначе false.</returns>
        public bool Undo()
        {
            if (_undoStack.Count == 0)
            {
                return false;
            }

            Change<T> change = _undoStack.Pop();
            _redoStack.Push(change); // Сохраняем для Redo

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

        /// <summary>
        /// Повторяет последнее отмененное действие.
        /// </summary>
        /// <returns>Восстановленный элемент и признак успеха.</returns>
        public (T val, bool ok) Redo()
        {
            if (_redoStack.Count == 0)
            {
                return (default(T)!, false);
            }

            Change<T> change = _redoStack.Pop();
            _undoStack.Push(change); // Сохраняем для Undo

            switch (change.Type)
            {
                case ChangeType.Add:
                    _items.Add(change.Item);
                    return (change.Item, true);
                case ChangeType.Remove:
                    if (change.Index >= 0 && change.Index <= _items.Count) // <= для Add
                    {
                        _items.RemoveAt(change.Index);
                        return (default(T)!, true); //Удаленный элемент не возвращаем.
                    }
                    else
                    {
                         return (default(T)!, false); // Индекс за пределами
                    }
                   
                default:
                    return (default(T)!, false);
            }
        }
    }

    /// <summary>
    /// Представляет изменение, которое можно отменить или повторить.
    /// </summary>
    /// <typeparam name="T">Тип элемента.</typeparam>
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

    /// <summary>
    /// Тип изменения.
    /// </summary>
    internal enum ChangeType
    {
        Add,
        Remove,
    }
}
