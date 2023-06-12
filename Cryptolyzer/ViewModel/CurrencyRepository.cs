using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cryptolyzer
{
    public class CurrencyRepository
    {
        public CurrencyRepository() { }

        public async Task<List<CurrencyModel>> GetCurrencies()
        {
            List<CurrencyModel> currencies = new List<CurrencyModel>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://api.coincap.io/v2/assets");
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
                        foreach (var asset in responseObject.data)
                        {
                            
                            string id = asset.id;
                            string symbol = asset.symbol;
                            string name = asset.name;
                            string rank = asset.rank;
                            string price = asset.priceUsd;
                            string percent = asset.changePercent24Hr;
                            string volume = asset.volumeUsd24Hr;
                            currencies.Add(new CurrencyModel(id, symbol, name, rank, price, volume, percent));
                        }
                        return currencies;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");

                    return null;
                }
            }
        }

        public async Task<CurrencyModel> GetCurrency(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"https://api.coincap.io/v2/assets/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
                        string new_id = responseObject.id;
                        string symbol = responseObject.symbol;
                        string name = responseObject.name;
                        string rank = responseObject.rank;
                        string price = responseObject.priceUsd;
                        string volume = responseObject.volumeUsd24Hr;
                        string percent = responseObject.changePercent24Hr;
                        return new CurrencyModel(new_id, symbol, name, rank, price, volume, percent);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return null;
                }
            }
        }

        public async Task UpdateCurrencies(List<CurrencyModel> currencies)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://api.coincap.io/v2/assets");
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
                        
                        foreach(var asset in responseObject.data)
                        {
                            var existingCurrency = currencies.Find(currency => currency.Id == (string)asset.id);

                            if (existingCurrency != null)
                            {
                                existingCurrency.NewName = asset.name;
                                existingCurrency.Rank = asset.rank;
                                existingCurrency.Symbol = asset.symbol;
                                existingCurrency.PriceUsd = asset.priceUsd;
                                existingCurrency.ChangePercent24Hr = asset.changePercent24Hr;
                                existingCurrency.Volume = asset.volume;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
