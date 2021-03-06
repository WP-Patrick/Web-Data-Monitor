using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Web_Monitor
{
    public static class scraper
    {
        private static Database_Management db_management = MainWindow.db_management;

        private static List<Timer> timer_list = new List<Timer>();
        /// <summary>
        /// Will spawn a timer with the selected time and will save the data to database
        /// </summary>
        /// <param name="ID">ID in DB</param>
        public static void LaunchScraperInstance(UI_CronData ucd)
        {
            int timeSync = ucd.FetchTime * 1000; // we need to multiply it by * 1000 as the value is in miliseconds

            Timer ccTime = new Timer(timeSync);
            timer_list.Add(ccTime); // we need to add it to list if we want to stop all the tasks
            ccTime.Elapsed += async (sender, e) => await FetchData(ucd);
            // we need to enable auto-reset, this will make it loop
            ccTime.AutoReset = true;
            ccTime.Enabled = ucd.Enabled;
        }

        /// <summary>
        /// Will loop through the timer_list and kill every instance running
        /// </summary>
        public static void KillAllInstances()
        {
            foreach(Timer t in timer_list)
            {
                t.Stop();
            }
            timer_list.Clear();
        }


        public static async Task FetchData(UI_CronData ucd) {
            HtmlWeb www = new HtmlWeb();
            HtmlDocument docu = await www.LoadFromWebAsync(ucd.SiteName);
            var node = docu.DocumentNode.SelectNodes(ucd.Path);
            if (node.Count == 0)
                return;
            if (!db_management.ScrapedData_SaveData(ucd.ID, Parsers.GetIntFromStr(node[0].InnerText)))
                throw new Exception("Scraped data update failed!");
        }
    }

}
