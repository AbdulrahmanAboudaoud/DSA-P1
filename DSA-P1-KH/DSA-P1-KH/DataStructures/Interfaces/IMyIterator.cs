namespace DSA_P1_KH.DataStructures.Interfaces;

public interface IMyIterator<T>
{
    bool HasNext();
    T Next();
    void Reset();
}