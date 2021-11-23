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

            int fetchtime = 60; // in seconds, default time that will be used for syncing

            Dictionary<string, string> dict =  Parsers.AddToSettings_OptionalValues(optionalvalues);
            // We can pretty much ignore this as the user doesn't need to specify other args, so we will just show an message that args are not specified in the log
            if(dict.Count == 0)
            { 
                Console.WriteLine("Either the argument is invalid, or the argument is not specified");
            }
            else
            {
                // We need to loop through the dictionary to get the values that we need
                foreach (KeyValuePair<string, string> qva in dict)
                {
                    // Here we are going to get our values, this is going to be the best way do it, with case operator
                    switch (qva.Key)
                    {
                        // If any of those arguments is badly specified, it will cancel the adding function
                        case "fetchtime":
                            if(!Int32.TryParse(qva.Value, out fetchtime)) // we are going to safely parse the string to int, in case it fails, we will display an error message
                            {
                                MessageBox.Show("Unable to parse parameter 'fetchtime', argument cannot be string!");
                                return;
                            }
                            break;
                    }
                }
            }

            // debug messages
            Console.WriteLine($"Fetch-time argument is: {fetchtime}");
        }
    }
}
