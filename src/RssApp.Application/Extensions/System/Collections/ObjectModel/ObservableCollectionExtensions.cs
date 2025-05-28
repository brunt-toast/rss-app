using System.Collections.ObjectModel;

namespace RssApp.Application.Extensions.System.Collections.ObjectModel;

/// <summary>
///     Extension methods for <see cref="ObservableCollection{T}"/>
///     Joosh.Utils 2025-05-28
/// </summary>
public static class ObservableCollectionExtensions
{
    /// <summary>
    ///     Shorthand for adding multiple items to the collection.
    ///     <see cref="ObservableCollection{T}.CollectionChanged"/> will still
    ///     be called for each new item, as opposed to once when the AddRange
    ///     operation has completed. 
    /// </summary>
    /// <typeparam name="T">
    ///     The type of items in the collection
    /// </typeparam>
    /// <param name="source">
    ///     The collection to act upon
    /// </param>
    /// <param name="newRange">
    ///     The new items to add to the collection
    /// </param>
    public static void AddRange<T>(this ObservableCollection<T> source, IEnumerable<T> newRange)
    {
        foreach (T item in newRange)
        {
            source.Add(item);
        }
    }

    /// <summary>
    ///     <para>
    ///         Shorthand for adding multiple items to the collection.
    ///         <see cref="ObservableCollection{T}.CollectionChanged"/> will still
    ///         be called for each new item, as opposed to once when the AddRange
    ///         operation has completed.
    ///     </para>
    ///     <para>
    ///         This method will <see langword="await" /> <see cref="Task.Yield()"/>
    ///         for each new item that is added to the collection. 
    ///     </para>
    /// </summary>
    /// <typeparam name="T">
    ///     The type of items in the collection
    /// </typeparam>
    /// <param name="source">
    ///     The collection to act upon
    /// </param>
    /// <param name="newRange">
    ///     The new items to add to the collection
    /// </param>
    public static async Task AddRangeAsync<T>(this ObservableCollection<T> source, IEnumerable<T> newRange)
    {
        using IEnumerator<T> enumerator = newRange.GetEnumerator();
        while (enumerator.MoveNext())
        {
            source.Add(enumerator.Current);
            await Task.Yield();
        }
    }

    /// <summary>
    ///     Like <see cref="ObservableCollection{T}.Clear()"/>, but will invoke
    ///     <see cref="ObservableCollection{T}.CollectionChanged"/> for each item
    ///     leaving the collection, instead of once at the end. 
    /// </summary>
    /// <typeparam name="T">
    ///     The type of items in the collection
    /// </typeparam>
    /// <param name="source">
    ///     The collection to act upon
    /// </param>
    public static void ClearIncrementally<T>(this ObservableCollection<T> source)
    {
        while (source.Count > 0)
        {
            source.RemoveAt(0);
        }
    }

    /// <summary>
    ///     <para>
    ///         Like <see cref="ObservableCollection{T}.Clear()"/>, but will invoke
    ///         <see cref="ObservableCollection{T}.CollectionChanged"/> for each item
    ///         leaving the collection, instead of once at the end.
    ///     </para>
    ///     <para>
    ///         This method will <see langword="await" /> <see cref="Task.Yield()"/>
    ///         for each item that is removed from the collection. 
    ///     </para>
    /// </summary>
    /// <typeparam name="T">
    ///     The type of items in the collection
    /// </typeparam>
    /// <param name="source">
    ///     The collection to act upon
    /// </param>
    public static async Task ClearIncrementallyAsync<T>(this ObservableCollection<T> source)
    {
        while (source.Count > 0)
        {
            source.RemoveAt(0);
            await Task.Yield();
        }
    }

    /// <summary>
    ///     <para>
    ///         Remove all existing items in the collection,
    ///         then re-populate the collection with new items.
    ///     </para>
    ///     <para>
    ///         <see cref="ObservableCollection{T}.CollectionChanged"/>
    ///         will be raised for each new item entering the collection.
    ///         If <see cref="useIncrementalClear"/>, the event will also
    ///         be raised for each item leaving the collection.
    ///     </para>
    /// </summary>
    /// <typeparam name="T">
    ///     The type of items in the collection
    /// </typeparam>
    /// <param name="source">
    ///     The collection to act upon
    /// </param>
    /// <param name="newRange">
    ///     The new items to add to the collection
    /// </param>
    /// <param name="useIncrementalClear">
    ///     When <see langword="true"/>, <see cref="ObservableCollection{T}.CollectionChanged"/>
    ///     will be invoked for each leaving the collection as it clears, as opposed to once when
    ///     fully cleared.
    /// </param>
    public static void ReplaceRange<T>(this ObservableCollection<T> source, IEnumerable<T> newRange, bool useIncrementalClear = false)
    {
        if (useIncrementalClear)
        {
            source.ClearIncrementally();
        }
        else
        {
            source.Clear();
        }

        source.AddRange(newRange);
    }

    /// <summary>
    ///     <para>
    ///         Remove all existing items in the collection,
    ///         then re-populate the collection with new items.
    ///     </para>
    ///     <para>
    ///         <see cref="ObservableCollection{T}.CollectionChanged"/>
    ///         will be raised for each new item entering the collection.
    ///         If <see cref="useIncrementalClear"/>, the event will also
    ///         be raised for each item leaving the collection.
    ///     </para>
    ///     <para>
    ///         This method will <see langword="await" /> <see cref="Task.Yield()"/>
    ///         after each item is added to the collection, and after each item is removed
    ///         if <see cref="useIncrementalClear"/>.
    ///     </para>
    /// </summary>
    /// <typeparam name="T">
    ///     The type of items in the collection
    /// </typeparam>
    /// <param name="source">
    ///     The collection to act upon
    /// </param>
    /// <param name="newRange">
    ///     The new items to add to the collection
    /// </param>
    /// <param name="useIncrementalClear">
    ///     When <see langword="true"/>, <see cref="ObservableCollection{T}.CollectionChanged"/>
    ///     will be invoked for each leaving the collection as it clears, as opposed to once when
    ///     fully cleared.
    /// </param>
    public static async Task ReplaceRangeAsync<T>(this ObservableCollection<T> source, IEnumerable<T> newCollection, bool useIncrementalClear = false)
    {
        if (useIncrementalClear)
        {
            await source.ClearIncrementallyAsync();
        }
        else
        {
            source.Clear();
        }

        await source.AddRangeAsync(newCollection);
    }
}
