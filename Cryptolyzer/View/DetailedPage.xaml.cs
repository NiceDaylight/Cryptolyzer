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
        public DetailedViewModel viewModel { get; set; }
        public string Id { get; set; }
        public string Rank { get; set; }
        public string newName { get; set; }
        public string Symbol { get; set; }
        public string PriceUsd { get; set; }
        public string newVolume { get; set; }
        public string Percentage { get; set; }

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
            viewModel = new DetailedViewModel();
            this.DataContext = viewModel;
            marketList.Visibility = Visibility.Collapsed;
            marketList.DataContext = this;
            this.Loaded += Page_Loaded;

        }


        private async void Page_Loaded(object sender, EventArgs e)
        {
            MarketModels = await viewModel.GetMarkets(Id);
            marketList.Items.Clear();
            marketList.ItemsSource = MarketModels;
            marketList.Visibility = Visibility.Visible;
        }
    }
}
