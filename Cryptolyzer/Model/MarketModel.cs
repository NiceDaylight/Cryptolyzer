using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptolyzer
{
    public class MarketModel
    {
        public MarketModel() { }
        public MarketModel(string exchangeId,
            string baseId,
            string quoteId,
            string baseSymbol,
            string quoteSymbol,
            string volumeUsd24Hr,
            string priceUsd,
            string volumePercent)
        {
            ExchangeId = exchangeId;
            BaseId = baseId;
            QuoteId = quoteId;
            BaseSymbol = baseSymbol;
            QuoteSymbol = quoteSymbol;
            VolumeUsd24Hr = volumeUsd24Hr;
            PriceUsd = priceUsd;
            VolumePercent = volumePercent;
        }

        public string ExchangeId { get; set; }
        public string BaseId { get; set; }
        public string QuoteId { get; set; }
        public string BaseSymbol { get; set; }
        public string QuoteSymbol { get; set; }
        public string VolumeUsd24Hr { get; set; }
        public string PriceUsd { get; set; }
        public string VolumePercent { get; set; }
    }
}
