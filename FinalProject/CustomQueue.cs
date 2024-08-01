using System;

namespace FinalProject
{
    internal class CustomQueue<T>
    {
        private T[] array;
        private int count, front, rear;
        public CustomQueue()
        {
            array = new T[5];
            count = 0; front = 0; rear = -1;
        }
        public int Count()
        {
            return count;
        }
        public void Enqueue(T item)
        {
            if (count < array.Length)
            {
                rear = (rear + 1) % array.Length;
                array[rear] = item;
                count++;
            }
            else
            {
                throw new Exception("Queue is Full!");
            }
        }
        public T Dequeue()
        {
            if (count > 0)
            {
                T item = array[front];
                front = (front + 1) % array.Length;
                count--;
                return item;
            }
            else
                throw new Exception("Queue is Empty!");
        }
        public T Peek()
        {
            if (count > 0)
            {
                T item = array[front];
                return item;
            }
            else
                throw new Exception("Queue is Empty!");
        }
        public T[] GetQueue()
        {
            if (count > 0)
            {
                T[] temp = new T[count];
                for (int i = 0; i < count; i++)
                    temp[i] = array[(front + i) % array.Length];
                return temp;
            }
            else
                return new T[] { };
        }

    }
}
