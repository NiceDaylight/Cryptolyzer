using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptolyzer
{
    public class CurrencyModel
    {
        public CurrencyModel() { }
        public CurrencyModel(string id,
            string symbol,
            string name,
            string rank,
            string priceUsd,
            string volume,
            string changePercent24Hr)
        {
            Id = id;
            Symbol = symbol;
            NewName = name;
            Rank = rank;
            PriceUsd = priceUsd;
            Volume = volume;
            ChangePercent24Hr = changePercent24Hr;
        }

        public string Id { get; set; }
        public string Symbol { get; set; }
        public string NewName { get; set; }
        public string Rank { get; set; }
        public string PriceUsd { get; set; }
        public string ChangePercent24Hr { get; set; }
        public string Volume { get; set; }
    }
}
