using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChainResource.Storages
{
    public class FileSystemStorage<T> : StorageBase<T>
    {
        private readonly string _filePath;
        public override bool CanWrite => true;

        public FileSystemStorage(string filePath, TimeSpan expiration) : base(expiration)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
            }

            _filePath = filePath;
        }

        public override async Task<T?> GetValue()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (File.Exists(_filePath))
                {
                    var json = await File.ReadAllTextAsync(_filePath);
                    var value = JsonSerializer.Deserialize<T>(json);
                    if (value == null)
                    {
                        throw new InvalidOperationException("Deserialized value is null");
                    }
                    LastUpdated = File.GetLastWriteTime(_filePath);
                    if (!IsExpired())
                    {
                        return value;
                    }
                }
                throw new InvalidOperationException("Value is expired or file does not exist");
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
                var json = JsonSerializer.Serialize(value);
                await File.WriteAllTextAsync(_filePath, json);
                LastUpdated = DateTime.Now;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
