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
        }

        /// <summary>
        /// Add the job to database, so later we can access it, automatically stored the ID with the cron job
        /// </summary>
        /// <param name="sitename">URL</param>
        /// <param name="path">XPATH/Selector</param>
        /// <param name="fetchtime">Default = 60</param>
        /// <returns>True if all ok, false if task failed</returns>
        public bool CRON_AddJob(string sitename, string path, int fetchtime = 60)
        {
            // If one of the string parameters is empty, do not even bother continuing
            if (string.IsNullOrEmpty(sitename) || string.IsNullOrEmpty(path))
                return false;
            // create a new object of cron jobs
            CronJobs addjob = new CronJobs
            {
                SiteName = sitename,
                Path = path,
                FetchTime = fetchtime
            };


            // Securely add it to the database, in case of any error, just print it to console and return False
            try
            {
                db.Insert(addjob);
                db.Commit();
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR has occured -> " + e.Message.ToString());
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
    }

}
