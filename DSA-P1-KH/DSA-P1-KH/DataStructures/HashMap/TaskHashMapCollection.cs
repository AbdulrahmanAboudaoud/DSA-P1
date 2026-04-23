using System.Collections;
using DSA_P1_KH.DataStructures.ArrayList;
using DSA_P1_KH.DataStructures.Interfaces;
using DSA_P1_KH.Model;

namespace DSA_P1_KH.DataStructures.HashMap;

public class TaskHashMapCollection : IMyCollection<TaskItem>
{
    private readonly MyHashMap<int, TaskItem> _map;
    private readonly MyArrayList<TaskItem> _items;

    public bool Dirty { get; set; }

    public int Count => _items.Count;

    // Creates a HashMap-backed task collection.
    public TaskHashMapCollection()
    {
        _map = new MyHashMap<int, TaskItem>();
        _items = new MyArrayList<TaskItem>();
        Dirty = false;
    }

    // Adds a task into both the map and ordered item list.
    public void Add(TaskItem item)
    {
        var existing = _map.Get(item.Id);

        if (existing != null)
        {
            _items.Remove(existing);
            _map.Remove(new KeyValuePair<int, TaskItem>(existing.Id, existing));
        }

        _map.Put(item.Id, item);
        _items.Add(item);
        Dirty = true;
    }

    // Removes a task from both the map and ordered item list.
    public void Remove(TaskItem item)
    {
        var existing = _map.Get(item.Id);

        if (existing == null)
            return;

        _map.Remove(new KeyValuePair<int, TaskItem>(existing.Id, existing));
        _items.Remove(existing);
        Dirty = true;
    }

    // Finds a task using a custom comparison rule.
    public TaskItem FindBy<K>(K key, Func<TaskItem, K, bool> comparer)
    {
        return _items.FindBy(key, comparer);
    }

    // Returns a filtered task collection.
    public IMyCollection<TaskItem> Filter(Func<TaskItem, bool> predicate)
    {
        return _items.Filter(predicate);
    }

    // Sorts the visible task order.
    public void Sort(Comparison<TaskItem> comparison)
    {
        _items.Sort(comparison);
        Dirty = true;
    }

    // Reduces all tasks into one accumulated value.
    public R Reduce<R>(Func<R, TaskItem, R> accumulator)
    {
        return _items.Reduce(accumulator);
    }

    public R Reduce<R>(R initial, Func<R, TaskItem, R> accumulator)
    {
        return _items.Reduce(initial, accumulator);
    }

    // Returns iterator for traversing tasks.
    public IMyIterator<TaskItem> GetIterator()
    {
        return _items.GetIterator();
    }

    // Returns enumerator for foreach support.
    public IEnumerator<TaskItem> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}