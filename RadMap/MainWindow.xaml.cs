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
using Telerik.Windows.Controls.Map;

namespace RadMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.radMap.Provider = new BingMapProvider(MapMode.Aerial, true, "key");
        }
        private void radMap_MapMouseClick(object sender, MapMouseRoutedEventArgs e)
        {
            //implement logic regarding single click here
        }

        private void radMap_MapMouseDoubleClick(object sender, MapMouseRoutedEventArgs e)
        {
            //implement logic regarding double click here
        }
    }
}
