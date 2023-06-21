using OpenQA.Selenium;
using AutomationTest.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationTest.Common
{
    class CDriver: BrowserInit
    {
        

        public CDriver()
        {
        }
        public CDriver(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void Capture()
        {
            Helper h = new Helper();

            h.TakeScreenshot(driver);
        }
        
    }
}
