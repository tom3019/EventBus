using System.Collections;
using System.Data;
using EventBus.Subscriptions.Exceptions;

namespace EventBus.Subscriptions;

/// <summary>
/// Subscription Collection
/// </summary>
internal class SubscriptionCollection : ISubscriptionCollection
{
    /// <summary>
    /// Subscription Descriptor List
    /// </summary>
    private readonly List<SubscriptionDescriptor> _descriptors = [];

    /// <summary>
    /// Returns an enumerator that iterates through the SubscriptionDescriptor
    /// </summary>
    /// <returns></returns>
    public IEnumerator<SubscriptionDescriptor> GetEnumerator() => _descriptors.GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through the SubscriptionDescriptor
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Add SubscriptionDescriptor
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <typeparam name="TEventHandler"></typeparam>
    public void Add<TEvent, TEventHandler>() where TEvent : class where TEventHandler : IEventHandler<TEvent>
    {
        CheckReadOnly();
        _descriptors.Add(new SubscriptionDescriptor(typeof(TEvent), typeof(TEventHandler)));
    }

    /// <summary>
    /// Add SubscriptionDescriptor
    /// </summary>
    /// <param name="item"></param>
    public void Add(SubscriptionDescriptor item)
    {
        CheckReadOnly();
        _descriptors.Add(item);
    }

    /// <summary>
    /// Clear Subscription Collection
    /// </summary>
    public void Clear()
    {
        CheckReadOnly();
        _descriptors.Clear();
    }

    /// <summary>
    /// Check if Subscription Collection contains SubscriptionDescriptor
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(SubscriptionDescriptor item) => _descriptors.Contains(item);

    /// <summary>
    /// Copy SubscriptionDescriptor to array
    /// </summary>
    /// <param name="array"></param>
    /// <param name="arrayIndex"></param>
    public void CopyTo(SubscriptionDescriptor[] array, int arrayIndex) => _descriptors.CopyTo(array, arrayIndex);

    /// <summary>
    /// Remove SubscriptionDescriptor
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Remove(SubscriptionDescriptor item)
    {
        CheckReadOnly();
        return _descriptors.Remove(item);
    }

    /// <summary>
    /// Count of SubscriptionDescriptor
    /// </summary>
    public int Count => _descriptors.Count;

    /// <summary>
    /// Is Subscription Collection Read Only
    /// </summary>
    public bool IsReadOnly { get; private set; }

    /// <summary>
    /// Make Subscription Collection Read Only
    /// </summary>
    public void MakeReadOnly()
    {
        IsReadOnly = true;
    }

    /// <summary>
    /// Check if Subscription Collection is Read Only
    /// </summary>
    private void CheckReadOnly()
    {
        if (IsReadOnly)
        {
            ThrowReadOnlyException();
        }
    }

    /// <summary>
    /// Throw Subscription Collection Read Only Exception
    /// </summary>
    /// <exception cref="SubscriptionCollectionReadOnlyException"></exception>
    private static void ThrowReadOnlyException() =>
        throw new SubscriptionCollectionReadOnlyException();

    /// <summary>
    /// Get Index of SubscriptionDescriptor
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int IndexOf(SubscriptionDescriptor item) => _descriptors.IndexOf(item);

    /// <summary>
    /// Insert SubscriptionDescriptor
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    public void Insert(int index, SubscriptionDescriptor item)
    {
        CheckReadOnly();
        _descriptors.Insert(index, item);
    }

    /// <summary>
    /// Remove SubscriptionDescriptor at index
    /// </summary>
    /// <param name="index"></param>
    public void RemoveAt(int index)
    {
        CheckReadOnly();
        _descriptors.RemoveAt(index);
    }

    /// <summary>
    /// Get SubscriptionDescriptor at index
    /// </summary>
    /// <param name="index"></param>
    public SubscriptionDescriptor this[int index]
    {
        get => _descriptors[index];
        set
        {
            CheckReadOnly();
            _descriptors[index] = value;
        }
    }
}