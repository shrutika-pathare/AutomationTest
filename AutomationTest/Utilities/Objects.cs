using AutomationTest.PageObject;
using OpenQA.Selenium;

using OpenQA.Selenium.Support.UI;





namespace AutomationTest.Utilities
{
    public class Objects
    {
        IWebDriver driver = null;
        IWait<IWebDriver> iWait = null;

        public Objects(IWebDriver driver, BrowserInit browser)
        {
            this.driver = driver;
            this.iWait = browser.iWait;
        }
        
        public static Configuration.Config objConfig { get; set; }

        


       public static PageObject.GooglePO objGooglePO { get; set; }


        public void ObjectInitialization()
        {
            Logger.log.Info("Object Initialization is started.");

            objConfig = new Configuration.Config();

            objGooglePO = new GooglePO(driver, iWait);

            
        }

    }
}

