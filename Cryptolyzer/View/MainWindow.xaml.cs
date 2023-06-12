using System.Net.Http;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Cryptolyzer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SearchTextBox.KeyDown += SearchTextBox_KeyDown;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateToPage(new MainPage());
        }
        private void NavigateToPage(Page page)
        {
            MainContent.NavigationService.Navigate(page);
        }

        private void MainContent_Navigated(object sender, NavigationEventArgs e)
        {
            if (MainContent.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.DataContext = frameworkElement;
            }
        }

        private void DefaultButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(new MainPage());
        }



        private async void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CurrencyModel model = await Search();
                NavigateToPage(new DetailedPage(model));
            }
        }

        private HttpClient client = new HttpClient();

        private async Task<CurrencyModel> Search()
        {
            string searchTerm = SearchTextBox.Text.Trim();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    try
                    {
                        string url = $"https://api.coincap.io/v2/assets/{searchTerm}";
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                        string new_id = responseObject.id;
                        string symbol = responseObject.symbol;
                        string name = responseObject.name;
                        string rank = responseObject.rank;
                        string price = responseObject.priceUsd;
                        string volume = responseObject.volumeUsd24Hr;
                        string percent = responseObject.changePercent24Hr;
                        return new CurrencyModel(new_id, symbol, name, rank, price, volume, percent);
                    }
                    catch (Exception ex)
                    {
                    MessageBox.Show($"Error: {ex.Message}", "Search Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                return null;
            }
            return null;
        }
    }
}
