using System;

namespace FinalProject
{
    internal class CustomStack
    {
        public object[] array;
        private int top;
        public CustomStack()
        {
            array = new object[5];
            top = -1;
        }
        public void Push(object item)
        {
            if (top >= array.Length - 1)
                IncreaseSize();
            array[++top] = item;
        }
        public object Pop()
        {
            if (top == -1)
                throw new Exception("Stack is Empty");
            else
                return array[top--];
        }
        public object Peek()
        {
            if (top == -1)
                throw new Exception("Stack is Empty");
            else
                return array[top];
        }
        public void Print()
        {
            for (int i = top; i >= 0; i--)
                Console.WriteLine(array[i]);
        }
        public object[] Get()
        {
            object[] tempArray = new object[top + 1];
            for (int i = 0; i < top + 1; i++)
                tempArray[i] = array[i];
            return tempArray;
        }
        public int Count()
        {
            return top + 1;
        }
        public int CountOf(object item)
        {
            int counter = 0;
            for (int i = 0; i < top + 1; i++)
            {
                if (array[i] == item)
                    counter++;
            }
            return counter;
        }
        public bool Contains(object item)
        {
            for (int i = 0; i < top + 1; i++)
            {
                if (array[i] == item)
                    return true;
            }
            return false;
        }
        public void Clear()
        {
            top = -1;
        }
        public void IncreaseSize()
        {
            object[] tempArray = new object[array.Length * 2];
            for (int i = 0; i < top + 1; i++)
                tempArray[i] = array[i];
            array = tempArray;
        }
    }
}