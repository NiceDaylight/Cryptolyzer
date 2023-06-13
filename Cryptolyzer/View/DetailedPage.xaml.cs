using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cryptolyzer
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class DetailedPage : Page
    {
        public List<MarketModel> MarketModels { get; set; }

        public DetailedPage()
        {
            InitializeComponent();
        }
        public DetailedPage(CurrencyModel model)
        {
            Id = model.Id;
            Rank = model.Rank;
            newName = model.NewName;
            Symbol = model.Symbol;
            PriceUsd = model.PriceUsd;
            newVolume = model.Volume;
            Percentage = model.ChangePercent24Hr;
            InitializeComponent();
            marketList.Visibility = Visibility.Collapsed;
            marketList.DataContext = this;
            this.Loaded += Page_Loaded;
        }



        public string Id { get; set; }
        public string Rank { get; set; }
        public string newName { get; set; }
        public string Symbol { get; set; }
        public string PriceUsd { get; set; }
        public string newVolume { get; set; }
        public string Percentage { get; set; }

        private async void Page_Loaded(object sender, EventArgs e)
        {
            MarketModels = await GetMarkets();
            marketList.Items.Clear();
            marketList.ItemsSource = MarketModels;
            marketList.Visibility = Visibility.Visible;

        }

        public async Task<List<MarketModel>> GetMarkets()
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
