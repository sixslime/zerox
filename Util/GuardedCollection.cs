using System.Collections.Generic;

/// <summary>
/// A wrapper around a collection of <typeparamref name="T"/> that only allows modification through 'Add' and 'Remove';
/// </summary>
/// <typeparam name="T"></typeparam>
public class GuardedCollectionHandle<T>
{
    private readonly ICollection<T> _collection;

    public GuardedCollectionHandle(ICollection<T> collection)
    {
        _collection = collection;
    }

    public void Add(T element) => _collection.Add(element);
    public bool Remove(T element) => _collection.Remove(element);

}
