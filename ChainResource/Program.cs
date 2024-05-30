using System;
using System.Threading.Tasks;
using ChainResource.Storages;
using ChainResource.Models;

namespace ChainResource
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var chainResource = CreateExchangeRateChainResource();
                var exchangeRates = await chainResource.GetValue();

                if (exchangeRates != null)
                {
                    Console.WriteLine("Exchange rates retrieved successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to retrieve exchange rates.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static ChainResource<ExchangeRateList> CreateExchangeRateChainResource()
        {
            var memoryStorage = new MemoryStorage<ExchangeRateList>(TimeSpan.FromHours(1)); //outemost 
            var fileSystemStorage = new FileSystemStorage<ExchangeRateList>("exchangeRates.json", TimeSpan.FromHours(4));
            var webServiceStorage = new WebServiceStorage<ExchangeRateList>(
                "https://openexchangerates.org/api/latest.json?app_id=b083a22990554d51b704128629abc3ec"); //innermost 

            return new ChainResource<ExchangeRateList>(memoryStorage, fileSystemStorage, webServiceStorage);
        }
    }
}

