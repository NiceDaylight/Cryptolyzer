using System;
using System.Collections.Generic;
using System.Linq;
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
        public DetailedPage()
        {
            InitializeComponent();
        }
        public DetailedPage(CurrencyModel model)
        {
            Rank = model.Rank;
            newName = model.NewName;
            Symbol = model.Symbol;
            PriceUsd = model.PriceUsd;
            newVolume = model.Volume;
            Percentage = model.ChangePercent24Hr;
            InitializeComponent();
        }

        public string Rank { get; set; }
        public string newName { get; set; }
        public string Symbol { get; set; }
        public string PriceUsd { get; set; }
        public string newVolume { get; set; }
        public string Percentage { get; set; }
    }
}
