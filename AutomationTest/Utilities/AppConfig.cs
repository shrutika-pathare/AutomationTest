using System;
using System.Configuration;

namespace AutomationTest.Utilities
{
    class AppConfig
    {
        static string appUrl = "";

        static string Google = ConfigurationManager.AppSettings["Google"];
        static string SanityExecutingOn = ConfigurationManager.AppSettings["SanityExecutingOn"];
       
        
        public static string TakeUrl(String siteName)
        {
            if (siteName == "Google")
            {
                return Google;
            }
            
            else
            {
                return null;
            }

        }

        public static string TakeExecutionEnvironment()
        {
            if (SanityExecutingOn.ToString() == "dev")
            {
                return "dev";
            }
            else if (SanityExecutingOn.ToLower() == "prod" || SanityExecutingOn.ToLower() == "production")
            {
                return "prod";
            }
            else return null;
        }
    }

}
