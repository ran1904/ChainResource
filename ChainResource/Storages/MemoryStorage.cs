using System;
using System.Threading.Tasks;

namespace ChainResource.Storages
{
    public class MemoryStorage<T> : StorageBase<T>
    {
        private T? _value;
        public override bool CanWrite => true;

        public MemoryStorage(TimeSpan expiration) : base(expiration) { }

        public override async Task<T?> GetValue()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!IsExpired() && _value != null)
                {
                    return _value;
                }
                throw new InvalidOperationException("Value is expired or not set");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public override async Task SetValue(T value)
        {
            await _semaphore.WaitAsync();
            try
            {
                _value = value;
                LastUpdated = DateTime.Now;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
