using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Memory
    {
        private Stack<string> memoryStack = new Stack<string>(20);

        public void MemoryClear()
        {
            memoryStack.Clear();
        }

        public string MemoryRecall()
        {
            return memoryStack.Peek();
        }

        public void MemoryAdd(string text)
        {
            if (memoryStack.Count == 0)
            {
                memoryStack.Push(text);
            }
            else
            {
                memoryStack.Push((Double.Parse(text) + Double.Parse(memoryStack.Pop())).ToString());
            }
        }

        public void MemorySubstract(string text)
        {
            if (memoryStack.Count == 0)
            {
                memoryStack.Push((0 - Double.Parse(text)).ToString());
            }
            else
            {
                memoryStack.Push((Double.Parse(memoryStack.Pop()) - Double.Parse(text)).ToString());
            }
        }

        public void MemoryStore(string text)
        {
            memoryStack.Push(memoryStack.Pop());
            memoryStack.Push(text);
        }
    }
}
