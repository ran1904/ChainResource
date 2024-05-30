using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainResource.Storages
{
    public abstract class StorageBase<T> : IStorage<T>
    {
        protected DateTime LastUpdated;
        public TimeSpan Expiration { get; }
        public abstract bool CanWrite { get; }

        protected readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        protected StorageBase(TimeSpan expiration)
        {
            Expiration = expiration;
        }

        public bool IsExpired()
        {
            return DateTime.Now - LastUpdated > Expiration;
        }

        public abstract Task<T?> GetValue();
        public abstract Task SetValue(T value);
    }
}
