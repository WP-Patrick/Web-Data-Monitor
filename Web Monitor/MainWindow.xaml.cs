using System;
using System.Collections.Generic;
using System.Data;
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


        public static Database_Management db_management;
        public MainWindow()
        {
            InitializeComponent();
            // Initialize the DB management 
            db_management = new Database_Management();

            // Load the database data
            UI_CronData_ReloadData();


            Log_Add("Program init ok");
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

        /// <summary>
        /// Save the job to the database and log if the task was ok or failed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_AddToSettings_AddButton(object sender, RoutedEventArgs e)
        {
            string optionalvalues = addsettings_optionalargs.Text;
            string path = addsettings_scrapepath.Text;
            string sitetoscrape = addsettings_scrapersite.Text;


            if(string.IsNullOrEmpty(path) || string.IsNullOrEmpty(sitetoscrape))
            {
                Log_Add("Path or site to scrape is empty!");
                return;
            }

            int fetchtime = 60; // in seconds, default time that will be used for syncing
            bool isEnabled = false;
            Dictionary<string, string> dict =  Parsers.AddToSettings_OptionalValues(optionalvalues);
            // We can pretty much ignore if the string is empty as the user doesn't need to specify other args
            if (!string.IsNullOrEmpty(optionalvalues))
            {
                // We need to check if the dict is not empty, if it's, abort the operation
                if (dict.Count == 0)
                { 
                    MessageBox.Show("Invalid cmd for optional values, aborted.");
                    return;
                }

                foreach (KeyValuePair<string, string> pair in dict)
                {
                    // Here we are going to get our values, this is going to be the best way do it, with switch operator
                    switch (pair.Key)
                    {
                        // Any error caused in the case function will result in the whole function stopping, to prevent more errors
                        case "fetchtime":
                            if (!int.TryParse(pair.Value, out fetchtime)) // we are going to safely parse the string to int, in case it fails, we will display an error message
                            {
                                MessageBox.Show("Unable to parse parameter 'fetchtime', argument cannot be string!");
                                return;
                            }
                            break;
                        case "isEnabled":
                            if (pair.Value.ToLower() == "enabled")
                                isEnabled = true;
                            break;
                    }
                }
            }
           


            if(!db_management.CRON_AddJob(sitetoscrape, path, fetchtime, isEnabled))
            {
                MessageBox.Show("An error has occured while adding the job into job list!");
            }

            UI_CronData_ReloadData(); 
            // debug messages
            Log_Add("Add to db ok");
        }


        // We will create our array here to be able to load the data from anywhere from the class, not just in ReloadData func
        private UI_CronData[] dataToDisplay;

        /// <summary>
        /// Load the Cron data from database to the datagrid
        /// </summary>
        public void UI_CronData_ReloadData()
        {
            dataToDisplay = db_management.CRON_FetchData();
            UI_DataGrid.ItemsSource = dataToDisplay;

        }



        /// <summary>
        /// Adds a msg to the LogBox textbox
        /// </summary>
        /// <param name="s">Message to add</param>
        public void Log_Add(string s)
        {
            LogBox.Text += String.Format("{0} => {1}\r\n", DateTime.Now.ToString("HH:mm:ss"), s);
        }

        private void Start_Button(object sender, RoutedEventArgs e)
        {
            foreach(UI_CronData ucd in dataToDisplay)
            {
                scraper.LaunchScraperInstance(ucd);
                Log_Add(String.Format("Instance of '{0}' has been launched!", ucd.ID));
            }
        }

        private void Stop_Button(object sender, RoutedEventArgs e)
        {
            scraper.KillAllInstances();
            Log_Add("All instances were stopped.");
        }

        private void ApplyToDatabase_Click(object sender, RoutedEventArgs e)
        {
            foreach(UI_CronData ucd in dataToDisplay)
            {
                if (!db_management.CRON_UpdateData(ucd))
                    Log_Add("Database entry update has failed!");
            }
        }

        /// <summary>
        /// Function will get what button is calling the function and will get the ID from the object and create a chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowChart(object sender, RoutedEventArgs e)
        {
            if(sender is Button)
            {
                UI_CronData dataRowView = (UI_CronData)((Button)e.Source).DataContext;
                Charts chart = new Charts();
                chart.LoadChart(dataRowView.ID);
                chart.ShowDialog();
            }
            

        }

        /// <summary>
        /// removes data and reloads the ui
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_RemoveAllDataFromDB(object sender, RoutedEventArgs e)
        {
            db_management.DeleteAllData();
            UI_CronData_ReloadData();
        }
    }
}
