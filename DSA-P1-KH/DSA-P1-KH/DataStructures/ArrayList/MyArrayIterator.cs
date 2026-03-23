using DSA_P1_KH.DataStructures.Interfaces;

namespace DSA_P1_KH.DataStructures.ArrayList;

public class MyArrayIterator<T> : IMyIterator<T>
{
    private readonly T[] _data;
    private readonly int _count;
    private int _position = -1;

    public MyArrayIterator(T[] data, int count)
    {
        _data = data;
        _count = count;
    }

    public bool HasNext()
    {
        return _position + 1 < _count;
    }

    public T Next()
    {
        _position++;
        return _data[_position];
    }

    public void Reset()
    {
        _position = -1;
    }
}