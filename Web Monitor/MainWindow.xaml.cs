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

        private Database_Management db_management;
        public MainWindow()
        {
            InitializeComponent();
            // Initialize the DB management 
            db_management = new Database_Management();

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
        /// Save the job to the database and then show a message box that it was succesfull or the task failed to be added
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
                // log it
                return;
            }

            int fetchtime = 60; // in seconds, default time that will be used for syncing

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

                // We need to loop through the dictionary to get the values that we need
                foreach (KeyValuePair<string, string> qva in dict)
                {
                    // Here we are going to get our values, this is going to be the best way do it, with switch operator
                    switch (qva.Key)
                    {
                        // Any error caused in the case function will result in the whole function stopping, to prevent more errors
                        case "fetchtime":
                            if (!Int32.TryParse(qva.Value, out fetchtime)) // we are going to safely parse the string to int, in case it fails, we will display an error message
                            {
                                MessageBox.Show("Unable to parse parameter 'fetchtime', argument cannot be string!");
                                return;
                            }
                            break;
                    }
                }
            }
           


            if(!db_management.CRON_AddJob(path, sitetoscrape))
            {
                MessageBox.Show("An error has occured while adding the job into job list!");
            }

            UI_CronData_ReloadData(); // Reload the data from db
            // debug messages
            Log_Add("Add to db ok");
        }


        public void UI_CronData_ReloadData()
        {

        }


        /// <summary>
        /// Adds a msg to the LogBox textbox
        /// </summary>
        /// <param name="s">Message to add</param>
        public void Log_Add(string s)
        {
            LogBox.Text += String.Format("{0} => {1}\r\n", DateTime.Now.ToString("HH:mm:ss"), s);
        }


    }
}
