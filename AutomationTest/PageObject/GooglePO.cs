
using OpenQA.Selenium;

using System;

using Config = AutomationTest.Configuration.Config;
using OpenQA.Selenium.Support.UI;
using AventStack.ExtentReports.Model;
using System.Drawing;



namespace AutomationTest.PageObject
{
    public class GooglePO
    {
        public IWebDriver driver;
        public IWait<OpenQA.Selenium.IWebDriver> iWait;
      
        Config cf = new Config();
        ScreenCapture job = new ScreenCapture();

        #region Selector
        By searchTextBox = By.CssSelector("div.L3eUgb>div.o3j99.ikrT4e.om7nvf>form>div:nth-child(1)>div.A8SBwf>div.RNNXgb>div>div.a4bIc>input");
        By searchBtn = By.CssSelector("div.L3eUgb>div.o3j99.ikrT4e.om7nvf>form>div:nth-child(1)>div.A8SBwf>div.UUbT9>div.aajZCb>div.lJ9FBc>center>input.gNO89b");
        By link = By.CssSelector("a[title='Software Testing']");
        By button = By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div/c-wiz/div/div/div/div[2]/div[2]/button");

        #endregion

        public GooglePO(IWebDriver driver, IWait<OpenQA.Selenium.IWebDriver> iWait)
        {
            this.driver = driver;
            this.iWait = iWait;
        }
        public GooglePO(IWebDriver driver)
        {
            this.driver = driver;

        }

       
        public void navigateToGoogle() {

            String appURL = cf.readingXMLFile("AutomationTest", "Google", "startURL", "Config.xml");
            driver.Navigate().GoToUrl(appURL);
           
        }
        public void searchText()
        {

        }
        public void resultPage() {
           
            driver.FindElement(link).Click();
            driver.Quit();
        }
    }

    
}
