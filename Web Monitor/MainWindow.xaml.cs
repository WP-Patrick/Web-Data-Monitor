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

namespace Web_Monitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Will reset the text in the text boxes from UI AddToSettings function
        /// Is triggered by a button 'Clear the boxes'
        /// </summary>
        private void UI_AddToSettings_ClearTextBoxes(object sender, RoutedEventArgs e)
        {
            addsettings_optionalargs.Text = "";
            addsettings_scrapepath.Text = "";
            addsettings_scrapersite.Text = "";
        }



    }
}
