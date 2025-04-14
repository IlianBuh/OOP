using GraphicEditor.intern.lib.stack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicEditor.@internal.lib.stack
{
    class MyStack<T> : IRedoResolver<T>
    {
        private const int stackSize = 20;
        private T?[] stack = new T?[stackSize];
        private int addS = 0;
        private int endS = stackSize - 1;

        public void AddShape(T shape)
        {
            stack[addS] = shape;
            var newPoint = (addS + 1) % stackSize;

            if (addS == endS)
            {
                endS = newPoint;
            }

            addS = newPoint;
        }
        public (T shape, bool ok) GetShape()
        {
            if ((endS + 1) % stackSize == addS)
            {
                return (default, false);
            }
            addS = addS == 0 ? 19 : addS - 1;
            return (stack[addS], true);
        }
        public void Clear()
        {
            endS = stackSize - 1;

            for (addS = 0; addS < stackSize && stack[addS] != null; addS++)
            {
                stack[addS] = default;
            }

            addS = 0;
        }
    }

}
