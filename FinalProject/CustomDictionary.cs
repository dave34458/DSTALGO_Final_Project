using System;

public class CustomDictionary<TKey, TValue>
{
    private TKey[] _keys;
    private TValue[] _values;
    private int _size;

    public CustomDictionary()
    {
        _keys = new TKey[4];
        _values = new TValue[4];
        _size = 0;
    }

    public int Count => _size;

    public TValue this[TKey key]
    {
        get
        {
            int index = IndexOfKey(key);
            if (index >= 0)
                return _values[index];
            throw new ArgumentOutOfRangeException("Key not found.");
        }
        set
        {
            int index = IndexOfKey(key);
            if (index >= 0)
                _values[index] = value;
            else
                Add(key, value);
        }
    }

    public CustomList<TKey> Keys()
    {
        CustomList<TKey> keysList = new CustomList<TKey>();
        for (int i = 0; i < _size; i++)
        {
            keysList.Add(_keys[i]);
        }
        return keysList;
    }

    public CustomList<TValue> Values()
    {
        CustomList<TValue> valuesList = new CustomList<TValue>();
        for (int i = 0; i < _size; i++)
        {
            valuesList.Add(_values[i]);
        }
        return valuesList;
    }

    public void Add(TKey key, TValue value)
    {
        if (IndexOfKey(key) >= 0)
            throw new ArgumentException("Key already exists.");

        EnsureCapacity(_size + 1);
        _keys[_size] = key;
        _values[_size] = value;
        _size++;
    }

    public bool Remove(TKey key)
    {
        int index = IndexOfKey(key);
        if (index >= 0)
        {
            RemoveAt(index);
            return true;
        }
        return false;
    }

    public bool ContainsKey(TKey key)
    {
        return IndexOfKey(key) >= 0;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int index = IndexOfKey(key);
        if (index >= 0)
        {
            value = _values[index];
            return true;
        }
        value = default(TValue);
        return false;
    }

    private int IndexOfKey(TKey key)
    {
        for (int i = 0; i < _size; i++)
        {
            if (_keys[i].Equals(key))
                return i;
        }
        return -1;
    }

    private void RemoveAt(int index)
    {
        _size--;
        for (int i = index; i < _size; i++)
        {
            _keys[i] = _keys[i + 1];
            _values[i] = _values[i + 1];
        }
        _keys[_size] = default(TKey);
        _values[_size] = default(TValue);
    }

    private void EnsureCapacity(int min)
    {
        if (_keys.Length < min)
        {
            int newCapacity = _keys.Length == 0 ? 4 : _keys.Length * 2;
            if (newCapacity < min)
                newCapacity = min;

            TKey[] newKeys = new TKey[newCapacity];
            TValue[] newValues = new TValue[newCapacity];
            for (int i = 0; i < _size; i++)
            {
                newKeys[i] = _keys[i];
                newValues[i] = _values[i];
            }
            _keys = newKeys;
            _values = newValues;
        }
    }
}
