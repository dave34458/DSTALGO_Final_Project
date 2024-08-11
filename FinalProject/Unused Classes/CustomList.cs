using System;

public class CustomList<T>
{
    private T[] _items;
    private int _size;

    public CustomList()
    {
        _items = new T[4];
        _size = 0;
    }

    public int Count()
    {
        return _size;
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _size)
                return default(T);
            return _items[index];
        }
        set
        {
            if (index < 0 || index >= _size)
                throw new ArgumentOutOfRangeException();
            _items[index] = value;
        }
    }

    public void Add(T item)
    {
        EnsureCapacity(_size + 1);
        _items[_size++] = item;
    }

    public bool Remove(T item)
    {
        int index = IndexOf(item);
        if (index >= 0)
        {
            RemoveAt(index);
            return true;
        }
        return false;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _size)
            throw new ArgumentOutOfRangeException();

        _size--;
        for (int i = index; i < _size; i++)
        {
            _items[i] = _items[i + 1];
        }
        _items[_size] = default(T);
    }

    public void Insert(int index, T item)
    {
        if (index < 0 || index > _size)
            throw new ArgumentOutOfRangeException();

        EnsureCapacity(_size + 1);
        for (int i = _size; i > index; i--)
        {
            _items[i] = _items[i - 1];
        }
        _items[index] = item;
        _size++;
    }

    public void Clear()
    {
        for (int i = 0; i < _size; i++)
        {
            _items[i] = default(T);
        }
        _size = 0;
    }

    public int IndexOf(T item)
    {
        for (int i = 0; i < _size; i++)
        {
            if (_items[i].Equals(item))
                return i;
        }
        return -1;
    }

    public bool Contains(T item)
    {
        return IndexOf(item) >= 0;
    }



    private void EnsureCapacity(int min)
    {
        if (_items.Length < min)
        {
            int newCapacity = _items.Length == 0 ? 4 : _items.Length * 2;
            if (newCapacity < min)
                newCapacity = min;

            T[] newItems = new T[newCapacity];
            for (int i = 0; i < _size; i++)
            {
                newItems[i] = _items[i];
            }
            _items = newItems;
        }
    }
}
