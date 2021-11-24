using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Monitor
{

    public class Database_Management
    {
        private SQLiteConnection db;
        private static readonly string DB_PATH = "webmonitor.db";

        // We are going to initialize the database connection when a new instance of this object is spawned
        public Database_Management()
        {
            // We need to inicialize a DB connection 
            db = new SQLiteConnection(DB_PATH);

            // Create a table if it doesn't exists!
            db.CreateTable<CronJobs>();
            db.CreateTable<ScrapedData>();
        }

        /// <summary>
        /// Add the job to database, so later we can access it, automatically stored the ID with the cron job
        /// </summary>
        /// <param name="sitename">URL</param>
        /// <param name="path">XPATH/Selector</param>
        /// <param name="fetchtime">Default = 60</param>
        /// <returns>True if all ok, false if task failed</returns>
        public bool CRON_AddJob(string sitename, string path, int fetchtime = 60, bool enabled = true)
        {
            // If one of the string parameters is empty, do not even bother continuing
            if (string.IsNullOrEmpty(sitename) || string.IsNullOrEmpty(path))
                return false;
            // create a new object of cron jobs
            CronJobs newJob = new CronJobs
            {
                SiteName = sitename,
                Path = path,
                FetchTime = fetchtime,
                Enabled = enabled
            };


            // Add to the database, in case of error, just print it to console and return false
            try
            {
                db.Insert(newJob);
                db.Commit();
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR has occured -> " + e.Message.ToString());
                return false; 
            }
            return true;
        }



        /// <summary>
        ///  Will return all data from the database
        /// </summary>
        /// <returns>array of UI_CronData</returns>
        public UI_CronData[] CRON_FetchData()
        {
            var query = db.Table<CronJobs>().ToList();
            UI_CronData[] data = Extensions.InitArr<UI_CronData>(query.Count); // create the data arr based on how many rows were returned by the db
            int i = 0;
            foreach (CronJobs cj in query)
            {
                // Create a new object and assign all the values to it
                UI_CronData crdt = new UI_CronData();
                crdt.FetchTime = cj.FetchTime;
                crdt.ID = cj.ID;
                crdt.Path = cj.Path;
                crdt.SiteName = cj.SiteName;
                // add it to arr
                data[i] = crdt;
                i++;
            }
            return data;
        }

        /// <summary>
        ///  This will return only one required data for scraper, search by ID
        /// </summary>
        /// <returns>obj ui_crondata</returns>
        public UI_CronData Scraper_GetData(int id)
        {
            var query = db.Table<CronJobs>().Where(i => i.ID == id).ToList();
            UI_CronData toReturn = new UI_CronData();
            if (query.Count == 0)
                return toReturn;
            toReturn.ID = query[0].ID;
            toReturn.Path = query[0].Path;
            toReturn.SiteName = query[0].SiteName;
            toReturn.FetchTime = query[0].FetchTime;
            toReturn.Enabled = query[0].Enabled;
            return toReturn;

        }

        /// <summary>
        /// Updates the data in database by ID
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true if ok, false if error occured</returns>
        public bool CRON_UpdateData(UI_CronData data)
        {
            CronJobs crjob = new CronJobs()
            {
                ID = data.ID,
                SiteName = data.SiteName,
                Path = data.Path,
                FetchTime = data.FetchTime,
                Enabled = data.Enabled
            };
            try
            {
                db.Update(crjob);
                db.Commit();
            }
            catch{
                return false;
            }
            
            return true;
        }

        public bool ScrapedData_SaveData(int id, int value)
        {
            ScrapedData newData = new ScrapedData()
            {
                ID = id,
                Value = value,
                Time = DateTime.Now
            };
            try
            {
                db.Insert(newData);
                db.Commit();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }


    [Table("CronJobs")]
    public class CronJobs
    {
        [PrimaryKey, AutoIncrement]
        [Column("ID")]
        public int ID { get; set; }

        [NotNull]
        [Column("SiteName")]
        public string SiteName { get; set; }

        [NotNull]
        [Column("Path")]
        public string Path { get; set; }

        [NotNull]
        [Column("FetchTime")]
        public int FetchTime { get; set; }

        [NotNull]
        [Column("Enabled")]
        public bool Enabled { get; set; }
    }

    [Table("ScraperData")]
    public class ScrapedData
    {
        [NotNull]
        [Column("ID")]

        public int ID { get; set; }

        [NotNull]
        [Column("Value")]
        public int Value { get; set; }

        [NotNull]
        [Column("TimeStamp")]
        public DateTime Time { get; set; }
    }

}
