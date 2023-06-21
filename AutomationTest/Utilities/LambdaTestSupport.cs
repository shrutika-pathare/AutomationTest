
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Configuration;

namespace AutomationTest.Utilities
{
    public class LambdaTestSupport
    {
        private static IWebDriver driver;
        private static String ltUserName;
        private static String ltAppKey;
        private static String platform;
        private static String browser;
        private static String browserVersion;
        public IWebDriver LambdaDriver()
        {
            InitCaps();
            DesiredCapabilities caps = new DesiredCapabilities();
            caps.SetCapability("platform", platform);
            caps.SetCapability("browserName", browser); // name of your browser to be defined in App.Config
            caps.SetCapability("version", browserVersion); // version of your selected browser to be defined in App.Config
            caps.SetCapability("name", "CSharpTestSample");
            caps.SetCapability("build", "LambdaTestSampleApp");
            caps.SetCapability("user", ltUserName); // LambdaTest Username to be defined in App.Config
            caps.SetCapability("accessKey", ltAppKey); // LambdaTest AccessKey to be defined in App.Config
            caps.SetCapability("tunnel", false);
            caps.SetCapability("visual", true);
            caps.SetCapability("resolution", "1920x1080");

            Console.WriteLine(ConfigurationSettings.AppSettings["LTUrl"]);

            driver = new RemoteWebDriver(new Uri(ConfigurationSettings.AppSettings["LTUrl"]), caps, TimeSpan.FromSeconds(600));
            return driver;
        }
        public static void InitCaps()

        {
            if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("LT_USERNAME")))
            {
                ltUserName = ConfigurationSettings.AppSettings["LTUser"];
            }
            if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("LT_APPKEY")))

                ltAppKey = ConfigurationSettings.AppSettings["LTAccessKey"];

            if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("LT_OPERATING_SYSTEM")))

                platform = ConfigurationSettings.AppSettings["OS"];

            if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("LT_BROWSER")))

                browser = ConfigurationSettings.AppSettings["Browser"];

            if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("LT_BROWSER_VERSION")))

                browserVersion = ConfigurationSettings.AppSettings["BrowserVersion"];
        }
    }
}