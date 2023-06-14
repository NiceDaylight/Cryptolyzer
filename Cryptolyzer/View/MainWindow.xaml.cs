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
        public MainWindowViewModel ViewModel { get; set; }
        private DispatcherTimer searchTimer { get; } = new DispatcherTimer();
        private bool isSearchInProgress;
        public MainWindow()
        {
            InitializeComponent();
            SearchListView.DataContext = this;
            ViewModel = new MainWindowViewModel(this);
            DataContext = ViewModel;
            searchTimer.Interval = TimeSpan.FromSeconds(0.3);
            searchTimer.Tick += SearchTimer_Tick;
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
            Loaded += Window_Loaded;
            MainContent.Navigated += MainContent_Navigated;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.NavigateToPage(new MainPage());
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
            ViewModel.NavigateToPage(new MainPage());
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
                ViewModel.NavigateToPage(detailedPage);
            }
            SearchTextBox.Text = "";
            SearchResultsPopup.IsOpen = false;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
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
                currencies = await ViewModel.Search(searchTerm);
                isSearchInProgress = false;
                SearchResultsPopup.IsOpen = true;
                SearchListView.ItemsSource = currencies;
            }
        }
    }
}
