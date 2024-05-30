using System.Threading.Tasks;

namespace ChainResource.Storages
{
    public interface IStorage<T>
    {
        Task<T?> GetValue();
        Task SetValue(T value);
        bool IsExpired();
        bool CanWrite { get; }
    }
}
