using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Web_Monitor
{
    /// <summary>
    /// Interaction logic for Charts.xaml
    /// </summary>
    public partial class Charts : Window
    {
        private static Database_Management db_management = MainWindow.db_management;

        public Charts()
        {
            InitializeComponent();
        }

        public void LoadChart(int id)
        {
            ((LineSeries)Chart.Series[0]).ItemsSource = db_management.Chart_GetDataByID(id).ToArray();
        }
    }
}
