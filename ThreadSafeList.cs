using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Moves;

namespace fire_ash_server
{
    public class ThreadSafeList<T> : IEnumerable<T>, IEnumerable, IDisposable
    {
        private readonly List<T> _list = new List<T>();
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private bool _disposed = false;

        public void Add(T item)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Add(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public T GetAt(int index)
        {
            _lock.EnterReadLock();
            try
            {
                return _list[index];
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public T RemoveLastItemAndGetNextItem()
        {
            _lock.EnterReadLock();
            try
            {
                if (_list.Count > 1)
                {
                    T item = _list[_list.Count - 2];
                    _list.RemoveAt(_list.Count - 1);
                    return item;
                }
                return _list[0];
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }


            public bool Remove(T item)
        {
            _lock.EnterWriteLock();
            try
            {
                return _list.Remove(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void RemoveAt(int index)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.RemoveAt(index);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public int Count
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _list.Count;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        public void Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public bool Contains(T item)
        {
            _lock.EnterReadLock();
            try
            {
                return _list.Contains(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            List<T> snapshot;
            _lock.EnterReadLock();
            try
            {
                snapshot = new List<T>(_list);
            }
            finally
            {
                _lock.ExitReadLock();
            }
            foreach (var item in snapshot)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _lock.Dispose();
                }

                _disposed = true;
            }
        }

        ~ThreadSafeList()
        {
            Dispose(false);
        }
    }
}
