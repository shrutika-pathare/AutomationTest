using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using AutomationTest.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;

namespace AutomationTest.Common
{
    public class Helper
    {
        public static IWebElement GetParentNode(IWebDriver driver, IWebElement childElement)
        {
            return (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].parentNode;", childElement);
        }

        
        public static Boolean IsElementPresent(IWebDriver driver, IWebElement parentElement, By element)
        {
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                parentElement.FindElement(element);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Boolean IsElementPresent(IWebDriver driver, By element)
        {
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
                driver.FindElement(element);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string TakeScreenshot(IWebDriver driver)
        {
            string path1 = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", "");
            string path = path1 + "Screenshot\\" + Helper.GenerateRandomGuid(7) + ".png";
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
            return path;
        }
        public void HandleUnsecureWebsite(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("javascript:document.getElementById('overridelink').click()");
        }

        public void hideWindow(IWebDriver driver)
        {
            driver.Manage().Window.Position = new Point(-2000, 0);

            driver.Manage().Window.Size = new Size(0, 0);

        }

        public void showWindow(IWebDriver driver)
        {
            driver.Manage().Window.Position = new Point(0, 0);

            driver.Manage().Window.Maximize();
        }

        public static string GetPostData(string fromLanguage, string toLanguage, string text)
        {
            string data = string.Format("hl=en&ie=UTF8&langpair={0}%7C{1}&text=" + text + "#{0}/{1}/" + text, fromLanguage, toLanguage);

            return data;
        }

        public static Boolean IsElementPresent(IWebDriver driver, By element, int timeoutValue)
        {
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeoutValue);
                driver.FindElement(element);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// This function return the child count of parent element.
        /// </summary>
        /// <param name="driver">Object of IWebDriver</param>
        /// <param name="parentElement">Parent element whose child elements need to count</param>
        /// <param name="byWhich">Which javascript locator is used to find child node count</param>
        /// <param name="value">value of attribute</param>
        /// <returns>total number of child elements</returns>
        public static int? GetChildCount(IWebDriver driver, IWebElement parentElement, String byWhich, String value)
        {
            IJavaScriptExecutor je = (IJavaScriptExecutor)driver;

            switch (byWhich.ToLower())
            {
                case "tag":
                    return Convert.ToInt32(je.ExecuteScript("return arguments[0].getElementsByTagName('" + value + "').length;", parentElement));
                case "class":
                    return Convert.ToInt32(je.ExecuteScript("return arguments[0].getElementsByClassName('" + value + "').length;", parentElement));
                case "name":
                    return Convert.ToInt32(je.ExecuteScript("return arguments[0].getElementsByName('" + value + "').length;", parentElement));
                case "css":
                    return Convert.ToInt32(je.ExecuteScript("return arguments[0].querySelectorAll('" + value + "').length;", parentElement));
                default:
                    Logger.log.Error("Locator is not correctly specified.");
                    return null;
            }

        }

        // This function returns screen height 
        public static int GetScreenHeight(IWebDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            return Int32.Parse(js.ExecuteScript("return screen.height").ToString());
        }
        public static string TrimText(string strText)
        {
            return strText.Trim().ToLower().Replace(" ", string.Empty);
        }

        // This function returns screen width 
        public static int GetScreenWidth(IWebDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            return Int32.Parse(js.ExecuteScript("return screen.width").ToString());
        }

        // This function returns currently running browser
        public static string GetIEVersion(IWebDriver driver, IWebElement element)
        {
            // string result; 

            String currentIEVersion = null, ie10Result = null, ie11Result = null, ie9Result = null;

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            ie11Result = (string)js.ExecuteScript("return navigator.msMaxTouchPoints !== void 0", element).ToString().ToLower();

            ie10Result = (string)js.ExecuteScript("return navigator.appVersion.indexOf('MSIE 10')", element).ToString();

            ie9Result = (string)js.ExecuteScript("return navigator.appVersion.indexOf('MSIE 9')", element).ToString();

            if (ie11Result.Equals("true"))
            {
                currentIEVersion = "IE11";
            }
            else if (!ie10Result.Equals("-1"))
            {
                currentIEVersion = "IE10";
            }
            else if (!ie9Result.Equals("-1"))
            {
                currentIEVersion = "IE9";
            }

            return currentIEVersion;
        }

        // This functions checks if registry entry is present for IE11 on system otherwise set accordingly
        public static Boolean CheckIE11RegistryPresence()
        {
            Boolean regFlag = false;

            try
            {
                string bit64keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BFCACHE";  // FEATURE_BFCACHE subkey may or may not be present, and should be created if it is not present

                string bit32keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BFCACHE"; // FEATURE_BFCACHE subkey may or may not be present, and should be created if it is not present

                string bit64_32_ValueName = "iexplore.exe";

                RegistryKey bit64IE11 = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BFCACHE");

                RegistryKey bit32IE11 = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BFCACHE");

                if (Environment.Is64BitOperatingSystem)
                {
                    if (Registry.GetValue(bit64keyName, bit64_32_ValueName, null) == null)
                    {
                        // Create 64 bit registry for IE 11 with ValueName = iexplore.exe and Value = 0       

                        bit64IE11.SetValue(bit64_32_ValueName, 0);

                        regFlag = true;
                    }
                    else
                    {
                        regFlag = true;                 // Registry Entry already present
                    }

                }
                else
                {
                    if (Registry.GetValue(bit32keyName, bit64_32_ValueName, null) == null)
                    {
                        // Create 64 bit registry for IE 11 with ValueName = iexplore.exe and Value = 0

                        bit32IE11.SetValue(bit64_32_ValueName, 0);

                        regFlag = true;
                    }
                    else
                    {
                        regFlag = true;                 // Registry Entry already present
                    }
                }
                return regFlag;
            }
            catch (Exception e)
            {
                Logger.log.Error(e);
                return regFlag;
            }
        }

        // This function performs
        // down on page

        public static Boolean ScrollDown(IWebDriver driver)
        {
            Boolean scrollStatDown = false;

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)", "");
                scrollStatDown = true;

            }
            catch (Exception e)
            {
                Logger.log.Error(e);
                scrollStatDown = false;
            }

            return scrollStatDown;
        }

        // This function performs scroll up on page
        public static Boolean ScrollUp(IWebDriver driver)
        {
            Boolean scrollStatUp = false;

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollTo(0, 0)", "");
                scrollStatUp = true;

            }
            catch (Exception e)
            {
                Logger.log.Error(e);
                scrollStatUp = false;
            }

            return scrollStatUp;
        }

        // This functions does file delete operation
        public static Boolean DeleteFile(String path)
        {
            try
            {
                if (System.IO.File.Exists(@"D:\Automation Documentation.doc"))
                {
                    System.IO.File.Delete(@"D:\Atomation Project.docx");
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Logger.log.Error(e);
                return false;
            }
        }

        // This function is used to set the implicit wait
        public static Boolean SetImplicitWait(IWebDriver driver, int timeOutValue, String durationType)
        {
            if (durationType.ToLower().Equals("millisecond"))
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeOutValue);
                return true;
            }
            else if (durationType.ToLower().Equals("second"))
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeOutValue);
                return true;
            }
            else if (durationType.ToLower().Equals("minute"))
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeOutValue);
                return true;
            }
            else if (durationType.ToLower().Equals("hour"))
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeOutValue);
                return true;
            }
            else if (durationType.ToLower().Equals("day"))
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeOutValue);
                return true;
            }
            else
            {
                return false;
            }
        }

        // This function returns thumb position of scrollbar
        public static int GetScrollTop(IWebDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            int position = Convert.ToInt32(js.ExecuteScript("return document.body.scrollTop"));
            return position;
        }

        // This function returns page width
        public static int GetWindowWidth(IWebDriver driver)
        {
            long windowWidth = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.offsetWidth");
            return (int)windowWidth;
        }

        // This function returns page height
        public static int GetWindowHeight(IWebDriver driver)
        {
            long windoHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return  document.body.parentNode.scrollHeight");
            return (int)windoHeight;
        }

        // This function returns Viewport width
        public static int GetViewportWidth(IWebDriver driver)
        {
            long viewportWidth = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.clientWidth");
            return (int)viewportWidth;
        }

        // This function returns Viewport height
        public static int GetViewportHeight(IWebDriver driver)
        {
            long viewportHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight");
            return (int)viewportHeight;
        }

        // This function returns Scroll height
        public static int GetScrollHeight(IWebDriver driver)
        {
            long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight");

            return (int)scrollHeight;
        }

        // This function handles alert on site
        public static void AlertHandling(IWebDriver driver, String popUpType, String action, String valueForPromptpopUp)
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                if (alert != null)
                {
                    if (popUpType.ToLower().Equals("alert"))
                        alert.Accept();
                    else if (popUpType.ToLower().Equals("confirm"))
                    {
                        if (action.ToLower().Equals("accept"))
                            alert.Accept();
                        else if (action.ToLower().Equals("dismiss"))
                            alert.Dismiss();
                    }
                    else if (popUpType.ToLower().Equals("prompt"))
                    {
                        alert.SendKeys(valueForPromptpopUp);
                        if (action.ToLower().Equals("accept"))
                            alert.Accept();
                        else if (action.ToLower().Equals("dismiss"))
                            alert.Dismiss();
                    }
                    else
                    {
                        Logger.log.Error("Pop up is not identified" + " at line:");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.log.Error(e);
            }
        }

        public static void SwitchToWebTab(IWebDriver driver, string browserType)
        {

            IWait<IWebDriver> iWait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

            iWait.Until(d => driver.FindElement(By.CssSelector("body")));

            driver.FindElement(By.CssSelector("body")).SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.NumberPad2);

            if (browserType.Equals("Firefox"))
            {
                driver.SwitchTo().Window(driver.WindowHandles.Last());
            }
            else if (browserType.Equals("Chrome"))
            {
                driver.SwitchTo().Window(driver.WindowHandles.Last());
            }
            else if (browserType.Equals("IE10"))
            {
                driver.SwitchTo().Window(driver.WindowHandles.Last());
            }
            Thread.Sleep(2000);
        }

        public static string getRunningBrowser(IWebDriver driver, IWebElement element)
        {
            string result, currentBrowser = null, ie10Result = null, ie11Result = null, ie9Result = null;

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            result = (string)js.ExecuteScript("return navigator.userAgent", element);

            ie11Result = (string)js.ExecuteScript("return !((window.ActiveXObject) && " + "ActiveXObject" + ")" + "", element).ToString().ToLower();

            ie10Result = (string)js.ExecuteScript("return navigator.appVersion.indexOf('MSIE 10')", element).ToString();

            ie9Result = (string)js.ExecuteScript("return navigator.appVersion.indexOf('MSIE 9')", element).ToString();

            if (result.Contains("Chrome"))
            {
                currentBrowser = "Chrome";
            }
            else if (result.Contains("Firefox"))
            {
                currentBrowser = "Firefox";
            }
            else if (!ie9Result.Equals("-1"))
            {
                currentBrowser = "IE9";
            }
            else if (!ie10Result.Equals("-1"))
            {
                currentBrowser = "IE10";
            }
            else if (ie11Result.Equals("true"))
            {
                currentBrowser = "IE11";
            }

            Console.WriteLine("Running Browser" + currentBrowser);

            return currentBrowser;
        }

        // This function takes the screenshot
        public static Boolean TakeScreenshot(IWebDriver driver, String browserType, String functinaNameWhereExceptionOccur)
        {
            Boolean flag = false;
            try
            {
                if (browserType.ToLower().Equals("firefox"))
                {
                    #region taking screenshot for firefox browser
                    Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                    ss.SaveAsFile("..\\debug\\Excpetion_Screenshots\\" + functinaNameWhereExceptionOccur + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                    flag = true;
                    #endregion
                }
                else if (browserType.ToLower().Equals("other"))
                {
                    #region takeing screen shot for other type of browser
                    Bitmap stitchedImage = null;
                    Screenshot screenshot = null;
                    int totalWidth = GetWindowWidth(driver);
                    int totalHeight = GetWindowHeight(driver);

                    // Get the Size of the Viewport

                    int viewportWidth = GetViewportWidth(driver);
                    int viewportHeight = GetViewportHeight(driver);

                    // Split the Screen in multiple Rectangles
                    List<Rectangle> rectangles = new List<Rectangle>();

                    // Loop until the Total Height is reached
                    for (int i = 0; i < totalHeight; i += viewportHeight)
                    {
                        int newHeight = viewportHeight;
                        // Fix if the Height of the Element is too big
                        if (i + viewportHeight > totalHeight)
                        {
                            newHeight = totalHeight - i;     //absolute function
                        }

                        // Loop until the Total Width is reached
                        for (int ii = 0; ii < totalWidth; ii += viewportWidth)
                        {
                            int newWidth = viewportWidth;
                            // Fix if the Width of the Element is too big
                            if (ii + viewportWidth > totalWidth)
                            {
                                newWidth = totalWidth - ii;
                            }

                            // Create and add the Rectangle
                            Rectangle currRect = new Rectangle(ii, i, newWidth, newHeight);  //Rectangle(x,y,width,height)
                            rectangles.Add(currRect);
                        }
                    }

                    // Build the Image
                    stitchedImage = new Bitmap(totalWidth, totalHeight);
                    // Get all Screenshots and stitch them together
                    Rectangle previous = Rectangle.Empty;
                    foreach (var rectangle in rectangles)
                    {
                        // Calculate the Scrolling (if needed)
                        if (previous != Rectangle.Empty)
                        {
                            int xDiff = rectangle.Right - previous.Right;   //rectangle.right returns The x-coordinate of the right side of the rectangle.
                            int yDiff = rectangle.Bottom - previous.Bottom;  //he y-coordinate of the bottom of the rectangle.

                            // Scroll
                            //selenium.RunScript(String.Format("window.scrollBy({0}, {1})", xDiff, yDiff));
                            ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("window.scrollBy({0}, {1})", xDiff, yDiff));
                            System.Threading.Thread.Sleep(200);
                        }

                        // Take Screenshot
                        screenshot = ((ITakesScreenshot)driver).GetScreenshot();

                        // Build an Image out of the Screenshot
                        Image screenshotImage;
                        using (MemoryStream memStream = new MemoryStream(screenshot.AsByteArray))
                        {
                            screenshotImage = Image.FromStream(memStream);
                        }

                        // Calculate the Source Rectangle
                        Rectangle sourceRectangle = new Rectangle(viewportWidth - rectangle.Width, viewportHeight - rectangle.Height, rectangle.Width, rectangle.Height);

                        // Copy the Image
                        using (Graphics g = Graphics.FromImage(stitchedImage))
                        {
                            g.DrawImage(screenshotImage, rectangle, sourceRectangle, GraphicsUnit.Pixel);
                        }
                        previous = rectangle;
                    }

                    stitchedImage.Save("..\\debug\\Excpetion_Screenshots\\" + functinaNameWhereExceptionOccur + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    flag = true;
                    #endregion
                }

            }
            catch (Exception e)
            {
                Logger.log.Error(e);
                return false;
            }
            return flag;
        }

        public static int lastId = 0;
        //public static ;

        static int generateId()
        {
            return Interlocked.Increment(ref lastId);
        }
        public static void takeAScreenshot(IWebDriver driver)
        {
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot;

            string scrPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName + "\\Screenshots\\";
            string currentFeatureName = FeatureContext.Current.FeatureInfo.Title;
            String stepName = ScenarioStepContext.Current.StepInfo.Text;

            try
            {
                if (!Directory.Exists(scrPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(scrPath);
                }

                screenshot = screenshotDriver.GetScreenshot();
                String screenshotId = generateId().ToString();
                screenshot.SaveAsFile(scrPath + currentFeatureName + "_Screenshot_" + screenshotId, ScreenshotImageFormat.Png);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

        }

        // This function set the fluentwait
        public DefaultWait<IWebDriver> FluentTimeout(IWebDriver driver, String durationType, int timeoutValue, int pollingTimeValue)
        {
            // Waiting 30 seconds for an element to be present on the page, checking
            // for its presence once every 5 seconds.

            if (durationType.ToLower().Equals("millisecond"))
            {
                DefaultWait<IWebDriver> wait = SetMillisecondFluentTimeOut(driver, timeoutValue, pollingTimeValue);
                return wait;
            }
            else if (durationType.ToLower().Equals("second"))
            {
                DefaultWait<IWebDriver> wait = SetSecondFluentTimeOut(driver, timeoutValue, pollingTimeValue);
                return wait;
            }
            else if (durationType.ToLower().Equals("minute"))
            {
                DefaultWait<IWebDriver> wait = SetMinuteFluentTimeOut(driver, timeoutValue, pollingTimeValue);
                return wait;
            }
            else if (durationType.ToLower().Equals("hour"))
            {
                DefaultWait<IWebDriver> wait = SetHourFluentTimeOut(driver, timeoutValue, pollingTimeValue);
                return wait;
            }
            else if (durationType.ToLower().Equals("day"))
            {
                DefaultWait<IWebDriver> wait = SetDayrFluentTimeOut(driver, timeoutValue, pollingTimeValue);
                return wait;
            }
            else
            {
                return null;
            }

        }

        public DefaultWait<IWebDriver> SetMillisecondFluentTimeOut(IWebDriver driver, int timeoutValue, int pollingTimeValue)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver);
            wait.Timeout = TimeSpan.FromMilliseconds(timeoutValue);
            wait.PollingInterval = TimeSpan.FromMilliseconds(pollingTimeValue);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            return wait;
        }

        public DefaultWait<IWebDriver> SetSecondFluentTimeOut(IWebDriver driver, int timeoutValue, int pollingTimeValue)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver);
            wait.Timeout = TimeSpan.FromSeconds(timeoutValue);
            wait.PollingInterval = TimeSpan.FromMilliseconds(pollingTimeValue);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            return wait;
        }

        public DefaultWait<IWebDriver> SetMinuteFluentTimeOut(IWebDriver driver, int timeoutValue, int pollingTimeValue)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver);
            wait.Timeout = TimeSpan.FromMinutes(timeoutValue);
            wait.PollingInterval = TimeSpan.FromSeconds(pollingTimeValue);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            return wait;
        }

        public DefaultWait<IWebDriver> SetHourFluentTimeOut(IWebDriver driver, int timeoutValue, int pollingTimeValue)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver);
            wait.Timeout = TimeSpan.FromHours(timeoutValue);
            wait.PollingInterval = TimeSpan.FromMinutes(pollingTimeValue);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            return wait;
        }

        public DefaultWait<IWebDriver> SetDayrFluentTimeOut(IWebDriver driver, int timeoutValue, int pollingTimeValue)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver);
            wait.Timeout = TimeSpan.FromDays(timeoutValue);
            wait.PollingInterval = TimeSpan.FromHours(pollingTimeValue);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            return wait;
        }

        // This function returns the SHA hash value for given file
        public static String CalculateSHA1(String path)
        {
            string SHACode = null;
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                using (var stream = System.IO.File.OpenRead(path))
                {
                    SHACode = BitConverter.ToString(sha1.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
            return SHACode;
        }

        // This function downloads the provided file
        public static void Download(String Url, String path, String filename)
        {
            WebClient client = new WebClient();
            client.DownloadFile(Url, path + filename);
        }

        // This function returns the status code of provided URL
        public static String GetStatusCode(Uri Url)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Url);
                webRequest.Timeout = 60000;
                webRequest.AllowAutoRedirect = true;
                HttpWebResponse wResp = (HttpWebResponse)webRequest.GetResponse();
                Thread.Sleep(2000);
                string wRespStatusCode = wResp.StatusCode.ToString();
                wResp.Close();
                wResp.Dispose();
                return wRespStatusCode;
            }
            catch (WebException we)
            {
                string wRespStatusCode = ((HttpWebResponse)we.Response).StatusCode.ToString();              // Need to check this line of code
                Thread.Sleep(2000);
                we.Response.Close();
                we.Response.Dispose();
                return wRespStatusCode;
            }
        }

        // This function uploads the provided file
        public static void Uploadfile(IWebElement fileInput, String uploadPath)
        {
            String fullPath = Path.GetFullPath(uploadPath);
            fileInput.SendKeys(fullPath);
            Thread.Sleep(1000);
        }

        // This function performs image comparision
        public static Boolean ImageComparision(Bitmap downloadedImage, Bitmap systemImage)
        {
            Boolean flag = true;

            if (downloadedImage.Width == systemImage.Width && downloadedImage.Height == systemImage.Height)
            {
                for (int row = 0; row < downloadedImage.Width; row++)
                {
                    for (int j = 0; j < downloadedImage.Height; j++)
                    {
                        String img1_ref = downloadedImage.GetPixel(row, j).ToString();
                        String img2_ref = systemImage.GetPixel(row, j).ToString();
                        if (img1_ref != img2_ref)
                        {
                            flag = false;
                            break;
                        }

                    }

                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        // This function highlights the element when exception occurs
        public static void HighlighElement(IWebDriver driver, String functionNameWhereExceptionOccur)
        {
            // Need to write code to read Xpath from XML file
            String xpath = "//*[@id='tsf']/div[2]/div[3]/cnter";

            Boolean flag = true;

            ReadOnlyCollection<IWebElement> element1 = null;

            // Set the lower limit of Xpath
            int indexOfLastSlash = -1;

            while (flag)
            {
                element1 = (ReadOnlyCollection<IWebElement>)driver.FindElements(By.XPath(xpath));

                // Check if it is a valid element for given xpath
                if (element1.Count != 0)
                {
                    IWebElement ele = driver.FindElement(By.XPath(xpath));

                    IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;

                    // Highlight the element with red border
                    executor.ExecuteScript("arguments[0].style.border='1px solid  red'", ele);

                    // Set the flag false tpo break the while loop
                    flag = false;
                }
                else
                {
                    indexOfLastSlash = xpath.LastIndexOf('/');

                    if (indexOfLastSlash > 1)
                    {
                        // Set the new xpath till the last slash index
                        xpath = xpath.Substring(0, indexOfLastSlash);
                    }
                    else
                    {
                        flag = false;
                    }
                }
            } // close of while

            TakeScreenshot(driver, "firefox", functionNameWhereExceptionOccur);
        }

        //This function will wait for the jQuery function execution
        public static Boolean IsJqueryActive(IWebDriver driver)
        {
            
            return true;
        }

        //This function will wait for the javascript function execution
        public static Boolean IsJavaScriptActive(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);


            Boolean javaScriptIsComplete = false;
            for (int i = 1; i <= 60; i++)
            {
                String state = ((IJavaScriptExecutor)driver).ExecuteScript(@"return document.readyState").ToString();
                javaScriptIsComplete = state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) || state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase);

                if (javaScriptIsComplete)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
            return javaScriptIsComplete;
            
            return true;
        }

        // This function generates new Guid

        public static string GetGuid()
        {
            Guid g;

            // Create and display the value of  GUID.
            g = Guid.NewGuid();

            string output = g.ToString().Replace("-", "");

            return output;
        }

        public static string GenerateRandomGuid(int length)
        {
            Random random = new Random();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

        public static void SetWindowPosition(IWebDriver driver, int xCordinate, int yCordinate)
        {
            driver.Manage().Window.Position = new System.Drawing.Point(xCordinate, yCordinate);
        }

        public static void SetWindowSize(IWebDriver driver, int width, int height)
        {
            driver.Manage().Window.Size = new Size(width, height);
        }

        public static void OpenTabAndNaviagate(IWebDriver driver, string Url, out string mainWindowID, out string newWindowID)
        {
            mainWindowID = driver.WindowHandles[0].ToString();
            var mainWindowHandle = driver.WindowHandles;

            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format("window.open('{0}', '_blank');", Url));
            var allWindowHandles = driver.WindowHandles;

            var newWindowHandle = allWindowHandles.Except(mainWindowHandle).Single();
            newWindowID = newWindowHandle.ToString();
        }

        public static void SwitchTab(IWebDriver driver, string windowID, int windpwPosition)
        {
            driver.SwitchTo().Window(windowID);
            driver.FindElement(By.TagName("body")).SendKeys(Keys.Control + "1");
        }

        public static void OpenNewTab(IWebDriver driver)
        {
            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
            executor.ExecuteScript("window.open();");

        }

        public static string GetDate(DateTime dateTime, string format = "dd/MM/yyyy")
        {
            return string.Format("{0:" + format + "}", dateTime);
        }

        public static string GetTime(DateTime dateTime, string format = "hh:mm")
        {
            return string.Format("{0:" + format + "}", dateTime);
        }

        public static string GetMeridiam(DateTime dateTime, string meridiam = "")
        {
            return string.Format("{0:tt}", dateTime);
        }

        public static void MoveToElement(IWebDriver driver, IWebElement element)
        {
            Actions action = new Actions(driver);
            action.MoveToElement(element).Build().Perform();
        }

        public static int getRandom(int maxLength)
        {
            Random rndGenerate = new Random();

            return rndGenerate.Next(1, maxLength);
        }

        public static string RemoveExtraWhiteSpaces(string text)
        {
            StringBuilder result = new StringBuilder();
            string[] value = text.Trim().Split(' ');
            foreach (string v in value)
            {
                if (!string.IsNullOrWhiteSpace(v))
                    result.Append(v.Trim() + " ");
            }

            return result.ToString().Trim();
        }

        public static void ClickElementScrollToView(IWebElement webElement, IWebDriver driver)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;

            Thread.Sleep(1000);
            js.ExecuteScript("arguments[0].scrollIntoView(true);", webElement);
            js.ExecuteScript("window.scrollBy(0,-400)", "");
            Thread.Sleep(1000);
            webElement.Click();
        }

        public static void ScrollToView(IWebElement webElement, IWebDriver driver)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            Thread.Sleep(1000);
            js.ExecuteScript("arguments[0].scrollIntoView(true);", webElement);
            Thread.Sleep(5000);
            js.ExecuteScript("window.scrollBy(0,-400)", "");
            Thread.Sleep(3000);
        }

        public static void ScrollUpOrDown(int pixels, IWebDriver driver)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;

            Thread.Sleep(1000);
            js.ExecuteScript("window.scrollBy(0," + pixels + ")", "");
            Thread.Sleep(1000);
        }



        public static void ScrollToViewElement(IWebElement webElement, IWebDriver driver)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            Thread.Sleep(1000);
            js.ExecuteScript("arguments[0].scrollIntoView(true);", webElement);
            Thread.Sleep(1000);
        }

        #region SQL
        
        /// <summary>
        /// Start the service specified by serviceName.
        /// </summary>
        /// <param name="serviceName">Name of service which we want to start.</param>
        /// <param name="serverName">Machine name on which service is present.</param>
        /// <returns></returns>
       

       

        /// <summary>
        /// Stop the service specified by serviceName.
        /// </summary>
        /// <param name="serviceName">Name of service which we want to Stop.</param>
        /// <param name="machineName">Machine name on which service is present.</param>
        /// <returns></returns>
       
        #endregion

    }
}
