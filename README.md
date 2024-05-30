# ChainResource

This ChainResource class encapsulate several potential Storages setup as a chain, each storage can be read only or read and write, and have some expiration interval, if the data saved in any such Storage is expired, the Resource would move to the next storage in the chain and try to read from that. 

Once a value is read from a Storage in the chain, the value is propagated upwards and stored in each Storage up the chain which supports writing.

## Storage Chain
Its storages from outermost to innermost:
1. Memory (read and write, expiration 1 hour),
2. FileSystem (read and write JSON file called `exchangeRates.json` in the `bin` folder, expiration 4 hours),
3. WebService (read only, expiration is irrelevant here)

WebService is a read only storage which gets the data from an API call to https://openexchangerates.org/ 

