using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Monitor
{
    public class UI_CronData
    {
        public int ID { get; set; }
        public string SiteName { get; set; }
        public string Path { get; set; }
        public int FetchTime { get; set; }
        
        public bool Enabled { get; set; }
    }
}
