using AutomationTest.Configuration;
using System;

namespace AutomationTest.Utilities
{
    public class Driver
    {
        Config config = new Config();

        public static BrowserInit browser { get; set; }

        public static void Intitialize()
        {
            browser = new BrowserInit();
            TurnOnWait();
            browser.driver.Manage().Window.Maximize();
        }

        public static void Navigate(String homeURL)
        {
            browser.GoToUrl(homeURL);
        }

        public static void Close()
        {
            

            try
            {
                browser.driver.Close();
                browser.driver.Quit();
                browser.driver.Dispose();
            }
            catch
            {

            }
        }

        private static void TurnOnWait()
        {
            browser.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(190000);
        }

    }
}
