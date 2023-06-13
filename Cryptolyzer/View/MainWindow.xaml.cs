using System.Net.Http;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Microsoft.Win32;

namespace Cryptolyzer
{
    public partial class MainWindow : Window
    {
        private HttpClient client = new HttpClient();
        private DispatcherTimer searchTimer;
        private bool isSearchInProgress;
        public MainWindow()
        {
            InitializeComponent();
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
            searchTimer = new DispatcherTimer();
            searchTimer.Interval = TimeSpan.FromSeconds(0.3);
            searchTimer.Tick += SearchTimer_Tick;
            SearchListView.DataContext = this;
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


        public async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void lwvMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Get the selected item from the ListView
            var selectedItem = SearchListView.SelectedItem as CurrencyModel;

            if (selectedItem != null)
            {
                // Create an instance of the DetailedPage
                var detailedPage = new DetailedPage(selectedItem);

                // Navigate to the DetailedPage
                NavigateToPage(detailedPage);
            }
            SearchTextBox.Text = "";
            SearchResultsPopup.IsOpen = false;
        }

        private async void SearchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            SearchResultsPopup.IsOpen = false;
            string searchTerm = SearchTextBox.Text;
            List<CurrencyModel> currencies = new List<CurrencyModel>();

            if (!isSearchInProgress && !string.IsNullOrEmpty(searchTerm))
            {
                isSearchInProgress = true;

                using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            HttpResponseMessage response = await client.GetAsync($"https://api.coincap.io/v2/assets/?limit=20&search={searchTerm}");
                            if (response.IsSuccessStatusCode)
                            {
                                
                                string responseBody = await response.Content.ReadAsStringAsync();
                                Console.WriteLine(responseBody);
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
                            Console.WriteLine(currencies[0].NewName);
                            }
                            isSearchInProgress = false;
                            SearchResultsPopup.IsOpen = true;
                            SearchListView.ItemsSource = currencies;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                    }
            }
        }
    }
}
