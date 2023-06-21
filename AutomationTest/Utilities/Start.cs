using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using AutomationTest.Browser;
using AutomationTest.Common;
using AutomationTest.Configuration;
using AutomationTest.Enum;
using System;
using System.Configuration;
using System.IO;
using System.Threading;
using TechTalk.SpecFlow;

namespace AutomationTest.Utilities
{
    [Binding]

    public class Start : Driver
    {
        private static ExtentTest featurname;
        private static ExtentTest scenario;
        private static ExtentReports extent = new ExtentReports();
        static string appUrl = "";

        static string TestEnvionment = ConfigurationManager.AppSettings["TestEnvironment"];
        static string DevUrl = ConfigurationManager.AppSettings["DevApplicationURL"];
        static string UatURL = ConfigurationManager.AppSettings["UatApplicationURL"];



        Config config = new Config();
        Objects obj = null;


        [BeforeTestRun]
        public static void InitializeReport()
        {
            String BrowserName = CurrentBrowserForExecution();
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
          

            string configDir = Directory.GetParent(baseDir).Parent.FullName; //For Windows OS


            var htmlReporter = new ExtentHtmlReporter(configDir + "index.html");
            htmlReporter.Config.DocumentTitle = "AutomationTest Automation Report";
            htmlReporter.Config.ReportName = "Executed on--> Browser: <b>" + BrowserName + "</b> ," + " Environment" + ": <b>" + TestEnvionment + "</b>";
            extent.AttachReporter(htmlReporter);

            DisplayEnvironmentConfigurationError();
        }


        [BeforeFeature]
        public static void BeforeFeature()
        {
            featurname = extent.CreateTest<Feature>(FeatureContext.Current.FeatureInfo.Title);       
        }

        [BeforeScenario]
        public void Setup()
        {
            try
            {


                scenario = featurname.CreateNode<Scenario>(ScenarioContext.Current.ScenarioInfo.Title);

                #region Initialization 

                // Initiating Logger
                Logger.SetLogger();

                // Initiating Browser
                Intitialize();

                // Initializing all the Page Objects
                obj = new Objects(Driver.browser.driver, Driver.browser);
                obj.ObjectInitialization();

                // Navigating to URL           

                //Driver.Navigate(appUrl);
                //Thread.Sleep(10000);

                #region Using AutoIT for handling windows and desktop applications
                //String username = ConfigurationManager.AppSettings["ApplicationUsername"];

                //String password = ConfigurationManager.AppSettings["Applicationpassword"];

                //AutoItX.WinWaitActive("Windows Security");
                //AutoItX.Send("dev\\" + username);
                //AutoItX.Send("{TAB}");
                //Thread.Sleep(2000);
                //AutoItX.Send(password);
                //////AutoItX.Send("{!}");
                //Thread.Sleep(2000);
                //AutoItX.Send("{TAB}");
                //AutoItX.Send("{TAB}");
                //AutoItX.Send("{ENTER}");
                #endregion

                #endregion
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Close();
            }

        }



        [AfterStep]
        public static void ScenarioSteps()
        {

            var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
            if (ScenarioContext.Current.TestError == null)
            {

                if (stepType == "Given")
                {
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
                }
                else if (stepType == "When")
                {
                    scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text);
                }
                else if (stepType == "Then")
                {
                    scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text);
                }
                else if (stepType == "And")
                {
                    scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text);
                    
                }
            }
            else if (ScenarioContext.Current.TestError != null)
            {
                if (stepType == "Given")
                {
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message);
                   
                }

                else if (stepType == "When")
                {
                    scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message);
                   
                }
                else if (stepType == "Then")
                {
                    scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message);
                    
                }
                else if (stepType == "And")
                {
                    scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message);
                   
                }
            }
        }

        [AfterScenario]
        public void TearDown()
        {
            Close();
            Helper.lastId = 0;

        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            Thread.Sleep(5000);
            extent.Flush();
        }

        public static string CurrentBrowserForExecution()
        {
            Browsers browser = new Browsers();
            if (Convert.ToBoolean(browser.SelectBrowser(BrowserCollection.firefox.ToString(), "Browser.xml")) == true)
            {
                return "Firefox";
            }
            else if (Convert.ToBoolean(browser.SelectBrowser(BrowserCollection.chrome.ToString(), "Browser.xml")) == true)
            {
                return "Chrome";
            }
            else if (Convert.ToBoolean(browser.SelectBrowser(BrowserCollection.ie.ToString(), "Browser.xml")) == true)
            {
                return "Internet Explorer";
            }
            else if (Convert.ToBoolean(browser.SelectBrowser(BrowserCollection.headless.ToString(), "Browser.xml")) == true)
            {
                return "Headless(Chrome)";
            }
            else if (Convert.ToBoolean(browser.SelectBrowser(BrowserCollection.edge.ToString(), "Browser.xml")) == true)
            {
                return "Microsoft Edge";
            }
            else if (Convert.ToBoolean(browser.SelectBrowser(BrowserCollection.safari.ToString(), "Browser.xml")) == true)
            {
                return "Safari";
            }
            else
                return null;
        }

        

        public static void DisplayEnvironmentConfigurationError()
        {
            if (TestEnvionment == "DEV")
            {
                appUrl = DevUrl;
            }
            else if ((TestEnvionment == "UAT"))
            {
                appUrl = UatURL;
            }
            else
            {
                featurname = extent.CreateTest("Configuration File Error");
                scenario = featurname.CreateNode("Environment name is not mentioned properly in App.config. Please mention Either DEV or UAT in app.config File").Fail("");
                throw new Exception("Execution environment is not mentioned properly");
            }

            
        }
    }

   
    

}

