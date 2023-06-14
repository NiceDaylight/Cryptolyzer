using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
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
using System.Windows.Threading;

namespace Cryptolyzer
{
    public partial class MainPage : Page
    {
        CurrencyRepository currencyRepository;
        private DispatcherTimer timer;
        private List<CurrencyModel> currencies = new List<CurrencyModel>();
        private Ping ping = new Ping();
        public MainPage()
        {
            if(ping.Send("8.8.8.8").Status == 0)
                {
                currencyRepository = new CurrencyRepository();
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(5000);
                timer.Tick += Timer_Tick;
                this.Loaded += Page_Loaded;
                timer.Start();
                InitializeComponent();
                itemList.Visibility = Visibility.Collapsed;
                itemList.ItemsSource = currencies;
            }
            else
            {
                currencyRepository = new CurrencyRepository();
                timer = new DispatcherTimer();
                TextBlock textBlock = new TextBlock();
                textBlock.Text = "No internet connection, sorry";
                InitializeComponent();
                itemList.Visibility = Visibility.Collapsed;    
                Content.Child = textBlock;
            }

        }


        private async void Timer_Tick(object sender, EventArgs e)
        {
            await UpdateApplicationStateAsync();
        }

        private async void Page_Loaded(object sender, EventArgs e)
        {
            currencies = await currencyRepository.GetCurrencies();
            itemList.ItemsSource = currencies;
            itemList.Visibility = Visibility.Visible;
        }
        private void lwvMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = itemList.SelectedItem as CurrencyModel;

            if (selectedItem != null)
            {
                var detailedPage = new DetailedPage(selectedItem);
                NavigationService.Navigate(detailedPage);
            }
        }

        private async Task UpdateApplicationStateAsync()
        {
            await currencyRepository.UpdateCurrencies(currencies);
            itemList.ItemsSource = currencies;
        }
    }
}
