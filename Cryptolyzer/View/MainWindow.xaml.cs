using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Cryptolyzer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Handle window loaded event
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateToPage(new MainPage());
        }
        // Handle window title bar drag-and-drop for window replacement
        private void NavigateToPage(Page page)
        {
            MainContent.NavigationService.Navigate(page);
        }
        private void MainContent_Navigated(object sender, NavigationEventArgs e)
        {
            if (MainContent.Content is FrameworkElement frameworkElement)
            {
                // Set the data context of the page to itself
                frameworkElement.DataContext = frameworkElement;
            }
        }

    }
}
