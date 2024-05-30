using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ChainResource.Models;

namespace ChainResource.Storages
{
    public class WebServiceStorage<T> : StorageBase<T>
    {
        private readonly string _url;
        private readonly HttpClient _httpClient;
        public override bool CanWrite => false;

        public WebServiceStorage(string url) : base(TimeSpan.Zero)
        {
            _url = url;
            _httpClient = new HttpClient();
        }

        public override async Task<T?> GetValue()
        {
            await _semaphore.WaitAsync();
            try
            {
                var value = await FetchDataFromWebService();
                if (value == null)
                {
                    throw new InvalidOperationException("Fetched value is null");
                }
                LastUpdated = DateTime.Now;
                return value;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public override Task SetValue(T value)
        {
            throw new NotSupportedException("WebServiceStorage is read-only");
        }

        private async Task<T?> FetchDataFromWebService()
        {
            var response = await _httpClient.GetStringAsync(_url);
            var value = JsonSerializer.Deserialize<T>(response);
            if (value == null)
            {
                throw new InvalidOperationException("Deserialized value is null");
            }
            return value;
        }
    }
}
