using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows;
using System.Windows.Threading;

namespace Cryptolyzer
{
    public class MainWindowViewModel
    {
        MainWindowViewModel() { }
        public MainWindowViewModel(MainWindow mainWindow)
        {
            main = mainWindow;
        }

        private HttpClient client { get; } = new HttpClient();
        private MainWindow main { get; set; }

        public void NavigateToPage(Page page)
        {
            main.MainContent.NavigationService.Navigate(page);
        }

            public async Task<List<CurrencyModel>> Search(string searchTerm)
            {
                List<CurrencyModel> currencies = new List<CurrencyModel>();
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
                        }
                        return currencies;
                    }
                    catch (Exception ex)
                    {
                    return null;
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
            }
        }
    }
