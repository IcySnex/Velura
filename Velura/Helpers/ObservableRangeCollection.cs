using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Velura.Helpers;

[Serializable]
public class ObservableRangeCollection<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
    // Classes
    sealed class SimpleMonitor : IDisposable
    {
        internal int BusyCount;

        [NonSerialized]
        internal ObservableRangeCollection<T> Collection;

        public SimpleMonitor(
            ObservableRangeCollection<T> collection)
        {
            Debug.Assert(collection != null);
            Collection = collection;
        }

        public void Dispose() =>
            Collection.blockReentrancyCount--;
    }

    internal static class EventArgsCache
    {
        internal static readonly PropertyChangedEventArgs CountPropertyChanged = new("Count");
        internal static readonly PropertyChangedEventArgs IndexerPropertyChanged = new("Item[]");
        internal static readonly NotifyCollectionChangedEventArgs ResetCollectionChanged = new(NotifyCollectionChangedAction.Reset);
    }


    // Reentrancy
    int blockReentrancyCount;

    protected IDisposable BlockReentrancy()
    {
        blockReentrancyCount++;
        return EnsureMonitorInitialized();
    }

    protected void CheckReentrancy()
    {
        if (blockReentrancyCount <= 0 || !(CollectionChanged?.GetInvocationList().Length > 1))
            return;

        throw new InvalidOperationException("SR.ObservableCollectionReentrancyNotAllowed");
    }


    // Monitor
    SimpleMonitor? monitor;

    SimpleMonitor EnsureMonitorInitialized() =>
        monitor ??= new(this);


    // Constructors
    public ObservableRangeCollection() { }

    public ObservableRangeCollection(
        IEnumerable<T> collection) : base(new List<T>(collection ?? throw new ArgumentNullException(nameof(collection)))) { }

    public ObservableRangeCollection(
        List<T> list) : base(new List<T>(list ?? throw new ArgumentNullException(nameof(list)))) { }


    // INotify
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(
        PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }


    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    protected virtual void OnCollectionChanged(
        NotifyCollectionChangedEventArgs e)
    {
        NotifyCollectionChangedEventHandler? handler = CollectionChanged;
        if (handler is null)
            return;

        blockReentrancyCount++;
        try
        {
            handler(this, e);
        }
        finally
        {
            blockReentrancyCount--;
        }
    }


    // Serialization
    [OnSerializing]
    void OnSerializing(
        StreamingContext context)
    {
        EnsureMonitorInitialized();
        monitor!.BusyCount = blockReentrancyCount;
    }

    [OnDeserialized]
    void OnDeserialized(
        StreamingContext context)
    {
        if (monitor is null)
            return;

        blockReentrancyCount = monitor.BusyCount;
        monitor.Collection = this;
    }


    // Methods
    protected virtual void MoveItem(
        int oldIndex,
        int newIndex)
    {
        CheckReentrancy();
        T removedItem = this[oldIndex];

        Items.RemoveAt(oldIndex);
        Items.Insert(newIndex, removedItem);

        OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
        OnCollectionChanged(new(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex));
    }

    protected override void SetItem(
        int index,
        T item)
    {
        CheckReentrancy();
        T originalItem = this[index];

        Items[index] = item;

        OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
        OnCollectionChanged(new(NotifyCollectionChangedAction.Replace, originalItem, item, index));
    }

    protected override void InsertItem(
        int index,
        T item)
    {
        CheckReentrancy();

        Items.Insert(index, item);

        OnPropertyChanged(EventArgsCache.CountPropertyChanged);
        OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
        OnCollectionChanged(new(NotifyCollectionChangedAction.Add, item, index));
    }

    protected override void RemoveItem(
        int index)
    {
        CheckReentrancy();
        T removedItem = this[index];

        Items.RemoveAt(index);

        OnPropertyChanged(EventArgsCache.CountPropertyChanged);
        OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
        OnCollectionChanged(new(NotifyCollectionChangedAction.Remove, removedItem, index));
    }

    protected override void ClearItems()
    {
        CheckReentrancy();

        Items.Clear();

        OnPropertyChanged(EventArgsCache.CountPropertyChanged);
        OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
        OnCollectionChanged(EventArgsCache.ResetCollectionChanged);
    }


    // Additional Methods
    public void AddRange(
        IEnumerable<T> items)
    {
        CheckReentrancy();
        
        int startingIndex = Items.Count;

        using IEnumerator<T> enumerator = items.GetEnumerator();
        while (enumerator.MoveNext())
            Items.Add(enumerator.Current);

        OnPropertyChanged(EventArgsCache.CountPropertyChanged);
        OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
        OnCollectionChanged(new(NotifyCollectionChangedAction.Add, items, startingIndex));
    }
}