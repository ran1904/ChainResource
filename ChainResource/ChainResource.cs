using System.Threading.Tasks;
using ChainResource.Storages;

namespace ChainResource
{
    public class ChainResource<T>
    {
        private readonly IStorage<T>[] _storages;

        public ChainResource(params IStorage<T>[] storages)
        {
            _storages = storages;
        }

        // we will get the value from the srotages from innermost readable one (memorey -> web) 
        public async Task<T?> GetValue()
        {
            for (int i = 0; i < _storages.Length; i++)
            {
                try
                {
                    var value = await _storages[i].GetValue();
                    if (value != null)
                    {
                        await PropagateValueUpwards(value, i-1); // we dont need to set val to the storage that we just get val from it, one before it. 
                        return value;
                    }
                }
                catch (InvalidOperationException)
                {
                    // Continue to next storage
                }
            }
            throw new InvalidOperationException("No valid value found in chain");
        }

        // we will set the value in the srotages to outermost writeable (web -> memoery) 
        private async Task PropagateValueUpwards(T value, int startIndex) 
        {
            for (int i = startIndex; i >= 0; i--)
            {
                if (_storages[i].CanWrite)
                {
                    await _storages[i].SetValue(value);
                }
            }
        }
    }
}
