using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cryptolyzer
{
    public class DetailedViewModel
    {
        public DetailedViewModel() { }

            public async Task<List<MarketModel>> GetMarkets(string Id)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"https://api.coincap.io/v2/assets/{Id}" + "/markets");
                    List<MarketModel> Markets = new List<MarketModel>();
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
                        foreach (var asset in responseObject.data)
                        {

                            string id = asset.exchangeId;
                            string baseId = asset.baseId;
                            string quoteId = asset.quoteId;
                            string baseSymbol = asset.baseSymbol;
                            string quoteSymbol = asset.quoteSymbol;
                            string volumeUsd24Hr = asset.volumeUsd24Hr;
                            string priceUsd = asset.priceUsd;
                            string volumePercent = asset.volumePercent;
                            if (priceUsd != null) Markets.Add(new MarketModel(id, (char.ToUpper(baseId[0]) + baseId.Substring(1)),
                                (char.ToUpper(quoteId[0]) + quoteId.Substring(1)), baseSymbol, quoteSymbol,
                                volumeUsd24Hr.Substring(0, volumeUsd24Hr.Length - 14),
                                priceUsd.Substring(0, priceUsd.Length - 14),
                                volumePercent.Substring(0, volumePercent.Length - 14)));
                        }
                        return Markets;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
