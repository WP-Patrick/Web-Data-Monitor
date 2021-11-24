using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Web_Monitor
{
    public static class Parsers
    {
        /// <summary>
        /// Converts the string to dictionary of parameters used for configuring the job
        /// The string is split by ,
        /// </summary>
        /// <param name="optionalValues"></param>
        /// <returns></returns>
        public static Dictionary<string, string> AddToSettings_OptionalValues(string optionalValues)
        {
            Dictionary<string, string> toReturn = new Dictionary<string, string>();
            // This will indicate that something is wrong and that we do not even bother checking the other conditions
            // if = is not in the string, then the whole string with arguments is invalid
            // We are also going to check wheter the string is not empty
            if (!optionalValues.Contains('=') || string.IsNullOrEmpty(optionalValues))
                return toReturn;
            string[] ov_splitted = optionalValues.Split(','); 
            foreach(string s in ov_splitted)
            {
                // Now we need to add the data into dictionary
                string[] s_splitted = s.Split('='); // temp variable
                if (s_splitted.Length != 2)
                    return toReturn;
                toReturn.Add(s_splitted[0], s_splitted[1]); // if everything is ok, add it to the dict
            }
            return toReturn;
        } 


        public static int GetIntFromStr(string s)
        {
            string s_replaced = Regex.Replace(s, @"\s+", "");
            return int.Parse(s_replaced);

        }

    }
}
