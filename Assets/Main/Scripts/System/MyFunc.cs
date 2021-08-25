using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;
using UniRx;
using System.Linq;

namespace MyFunc
{
    public static class RxFunc
    {
        public static void DisposeSubject<TSubject>(ref Subject<TSubject> subject)
        {
            if (subject != null)
            {
                try
                {
                    subject.OnCompleted();
                }
                finally
                {
                    subject.Dispose();
                    subject = null;
                }
            }
        }
    }
    public static class NetworkFunc
    {
        public static bool TryGetComponent<T>(ulong networkId, out T result)
        {
            result = default;
            return NetworkSpawnManager.SpawnedObjects[networkId]?.TryGetComponent<T>(out result) ?? false;
        }
        public static T GetComponent<T>(ulong networkId)
        => NetworkSpawnManager.SpawnedObjects[networkId].GetComponent<T>();
    }
}

namespace MyClass.Rx
{
    public class ReactiveCollectionEx<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IDisposable
    {
        List<T> m_ItemList;

        public int Count => m_ItemList.Count;
        public T this[int index] { set { m_ItemList[index] = value; } get { return m_ItemList[index]; } }
        public bool IsReadOnly => false;

        public ReactiveCollectionEx()
        {
            m_ItemList = new List<T>();
        }
        public ReactiveCollectionEx(IEnumerable<T> collection)
        {
            if (collection != null) throw new ArgumentNullException("collection");
            m_ItemList = collection.ToList();
        }
        public ReactiveCollectionEx(List<T> list)
        {
            m_ItemList = list;
        }

        public void Add(T item)
        {
            var index = Count;
            m_ItemList.Add(item);
            m_AddSubject?.OnNext(new CollectionAddRangeEvent<T>(index, item));
            m_CountChangeSubject?.OnNext(Count);
        }
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection.Count() == 0) return;
            var index = Count;
            m_ItemList.AddRange(collection);
            m_AddSubject?.OnNext(new CollectionAddRangeEvent<T>(index, collection));
            m_CountChangeSubject?.OnNext(Count);
        }
        public void Insert(int index, T item)
        {
            m_ItemList.Insert(index, item);
            m_AddSubject?.OnNext(new CollectionAddRangeEvent<T>(index, item));
            m_CountChangeSubject?.OnNext(Count);
        }
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection.Count() == 0) return;

            m_ItemList.InsertRange(index, collection);
            m_AddSubject?.OnNext(new CollectionAddRangeEvent<T>(index, collection));
            m_CountChangeSubject?.OnNext(Count);
        }
        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            else
                return false;
        }
        public void RemoveAt(int index)
        {
            var item = this[index];
            m_ItemList.RemoveAt(index);
            m_RemoveSubject?.OnNext(new CollectionRemoveRangeEvent<T>(index, item));
            m_CountChangeSubject?.OnNext(Count);
        }
        public void RemoveRange(int index, int count)
        {
            if (count <= 0) return;

            var collection = m_ItemList.GetRange(index, count);
            m_ItemList.RemoveRange(index, count);
            m_RemoveSubject?.OnNext(new CollectionRemoveRangeEvent<T>(index, collection));
            m_CountChangeSubject?.OnNext(Count);
        }
        public void Move(int oldIndex, int newIndex)
        {
            var item = m_ItemList[oldIndex];
            m_ItemList.RemoveAt(oldIndex);
            m_ItemList.Insert(newIndex, item);
            m_MoveSubject?.OnNext(new CollectionMoveEvent<T>(oldIndex, newIndex, item));
        }
        public void Shuffle(int seed) => m_ItemList.Suffle(seed);
        public void Clear()
        {
            m_ResetSubject?.OnNext(default);
            if (Count > 0)
                m_CountChangeSubject?.OnNext(0);
            m_ItemList.Clear();
        }

        public bool Contains(T item) => m_ItemList.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => m_ItemList.CopyTo(array, arrayIndex);
        public int IndexOf(T item) => m_ItemList.IndexOf(item);
        public List<T> GetRange(int index, int count) => m_ItemList.GetRange(index, count);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => m_ItemList.GetEnumerator();
        public IEnumerator<T> GetEnumerator() => m_ItemList.GetEnumerator();

        Subject<CollectionAddRangeEvent<T>> m_AddSubject;
        public Subject<int> m_CountChangeSubject;
        Subject<CollectionMoveEvent<T>> m_MoveSubject;
        public Subject<CollectionRemoveRangeEvent<T>> m_RemoveSubject;
        Subject<CollectionReplaceEvent<T>> m_ReplaceSubject;
        Subject<Unit> m_ResetSubject;
        public IObservable<CollectionAddRangeEvent<T>> ObserveAdd()
        {
            if (m_IsDisposed) return Observable.Empty<CollectionAddRangeEvent<T>>();
            return m_AddSubject ?? (m_AddSubject = new Subject<CollectionAddRangeEvent<T>>());
        }
        public IObservable<int> ObserveCountChanged(bool notifyCurrentCount = false)
        {
            if (m_IsDisposed) return Observable.Empty<int>();

            var subject = m_CountChangeSubject ?? (m_CountChangeSubject = new Subject<int>());
            if (notifyCurrentCount)
                return subject.StartWith(() => Count);
            else
                return subject;
        }
        public IObservable<CollectionMoveEvent<T>> ObserveMove()
        {
            if (m_IsDisposed) return Observable.Empty<CollectionMoveEvent<T>>();
            return m_MoveSubject ?? (m_MoveSubject = new Subject<CollectionMoveEvent<T>>());
        }
        public IObservable<CollectionRemoveRangeEvent<T>> ObserveRemove()
        {
            if (m_IsDisposed) return Observable.Empty<CollectionRemoveRangeEvent<T>>();
            return m_RemoveSubject ?? (m_RemoveSubject = new Subject<CollectionRemoveRangeEvent<T>>());
        }
        public IObservable<CollectionReplaceEvent<T>> ObserveReplace()
        {
            if (m_IsDisposed) return Observable.Empty<CollectionReplaceEvent<T>>();
            return m_ReplaceSubject ?? (m_ReplaceSubject = new Subject<CollectionReplaceEvent<T>>());
        }
        public IObservable<Unit> ObserveReset()
        {
            if (m_IsDisposed) return Observable.Empty<Unit>();
            return m_ResetSubject ?? (m_ResetSubject = new Subject<Unit>());
        }
        bool m_IsDisposed;
        public void Dispose() => Dispose(true);
        protected virtual void Dispose(bool disposing)
        {
            if (!m_IsDisposed)
            {
                if (disposing)
                {
                    DisposeSubject(ref m_AddSubject);
                    DisposeSubject(ref m_CountChangeSubject);
                    DisposeSubject(ref m_MoveSubject);
                    DisposeSubject(ref m_RemoveSubject);
                    DisposeSubject(ref m_ReplaceSubject);
                    DisposeSubject(ref m_ResetSubject);

                    m_IsDisposed = true;
                }
            }
        }
        void DisposeSubject<TSubject>(ref Subject<TSubject> subject)
        {
            if (subject != null)
            {
                try
                {
                    subject.OnCompleted();
                }
                finally
                {
                    subject.Dispose();
                    subject = null;
                }
            }
        }
        override public string ToString()
        => $"[{string.Join(" , ", m_ItemList)}]";
    }
    readonly public struct CollectionAddRangeEvent<T>
    {
        readonly public int FirstIndex;
        readonly public T[] Values;
        public CollectionAddRangeEvent(int index, IEnumerable<T> collection)
        {
            FirstIndex = index;
            Values = collection.ToArray();
        }
        public CollectionAddRangeEvent(int index, T item)
        {
            FirstIndex = index;
            Values = new[] { item };
        }


    }
    readonly public struct CollectionRemoveRangeEvent<T>
    {
        readonly public int FirstIndex;
        readonly public T[] Values;
        public CollectionRemoveRangeEvent(int index, IEnumerable<T> collection)
        {
            FirstIndex = index;
            Values = collection.ToArray();
        }
        public CollectionRemoveRangeEvent(int index, T item)
        {
            FirstIndex = index;
            Values = new[] { item };
        }
    }

}