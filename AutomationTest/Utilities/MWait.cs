using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AutomationTest.Utilities
{
    public class MWait
    {
        public static void InvisibilityOfSearchLoader(IWebElement element, int timeout)
        {
            String valueOfAriaHidden = null;
            Boolean flag = false;

            for (int i = 1; i <= timeout; i = i + 2)
            {
                Logger.log.Info("Waiting for Invisibility of Progress Bar");
                Console.WriteLine("Waiting for Invisibility of Progress Bar");

                valueOfAriaHidden = element.GetAttribute("aria-hidden").ToLower();

                if (valueOfAriaHidden.Equals("true"))
                {
                    flag = true;
                    break;
                }

                Thread.Sleep(1000);
            }

            if (!flag)
                Assert.Fail("Time Out...... Progress Bar is still displayed, after time out");
        }

       
        public static void InvisibilityOfSpinner(IWebDriver driver)
        {
            String valueOfAriaHidden = null;
            IWebElement progessBar = null;
            Boolean flag = false;

            for (int i = 1; i <= 60; i = i + 2)
            {

                Console.WriteLine("Waiting for Invisibility of Overlay");

                progessBar = driver.FindElement(By.ClassName("overlay"));
                valueOfAriaHidden = progessBar.GetAttribute("aria-hidden");

                if (valueOfAriaHidden.Equals("true"))
                {
                    flag = true;
                    break;
                }

                Thread.Sleep(2000);
            }

            if (!flag)
                Assert.Fail("Time Out...... Overlay is displayed after time out.");
        }

        public static void InvisibilityOfPopup(IWebDriver driver)
        {

            String valueOfClassAttr = null;
            IWebElement progessBar = null;
            Boolean flag = false;

            for (int i = 1; i <= 60; i = i + 2)
            {

                Logger.log.Info("Waiting for Invisibility of Pop up");

                progessBar = driver.FindElement(By.TagName("body"));
                valueOfClassAttr = progessBar.GetAttribute("class");

                if (!valueOfClassAttr.Contains("modal-open"))
                {
                    flag = true;
                    break;
                }
                Thread.Sleep(2000);
            }

            if (!flag)
                Assert.Fail("Time Out...... Pop up is displayed after time out.");
        }

        public static void VisibilityOfLeftMenu(IWebDriver driver)
        {
            String valueOfClassAttr = null;
            IWebElement body = null;
            Boolean flag = false;

            for (int i = 1; i <= 60; i = i + 2)
            {

                Logger.log.Info("Waiting for visibility of Left Menu");

                body = driver.FindElement(By.TagName("body"));
                valueOfClassAttr = body.GetAttribute("class");

                if (valueOfClassAttr.Equals(String.Empty))
                {
                    flag = true;
                    break;
                }

                Thread.Sleep(2000);
            }

            if (!flag)
            {
                Logger.log.Warn("Time Out...... Left Menu is not opened");
                Assert.Fail();
            }

            Thread.Sleep(200);
        }

        public static void InvisibilityOfLeftMenu(IWebDriver driver)
        {
            String valueOfClassAttr = null;
            IWebElement body = null;
            Boolean flag = false;

            for (int i = 1; i <= 60; i = i + 2)
            {

                Logger.log.Info("Waiting for Invisibility of Left Menu");

                body = driver.FindElement(By.TagName("body"));
                valueOfClassAttr = body.GetAttribute("class");

                if (valueOfClassAttr.Equals("aside-offscreen"))
                {
                    flag = true;
                    break;
                }

                Thread.Sleep(2000);
            }

            if (!flag)
            {
                Logger.log.Warn("Time Out...... Left Menu is closed");
                Assert.Fail();
            }

            Thread.Sleep(200);
        }

        public static void UntillSectionExpand(IWebDriver driver, IWebElement element)
        {
            String attributeValue = null;
            Boolean flag = false;

            for (int i = 1; i <= 60; i = i + 2)
            {
                Console.WriteLine("Waiting untill section expand");
                attributeValue = element.GetAttribute("class");

                if (attributeValue.Contains("in"))
                {
                    flag = true;
                    break;
                }

                Thread.Sleep(2000);
            }

            if (!flag)
                Assert.Fail("Time Out...... Section is not expanded.");
        }

        public static void UntillSectionCollapsed(IWebDriver driver, IWebElement element)
        {   
            String attributeValue = null;
            Boolean flag = false;

            for (int i = 1; i <= 60; i = i + 2)
            {
                Console.WriteLine("Waiting untill section collapsed");
                attributeValue = element.GetAttribute("class");

                if (!attributeValue.Equals("in"))
                {
                    flag = true;
                    break;
                }

                Thread.Sleep(2000);
            }

            if (!flag)
                Assert.Fail("Time Out...... Section is not collapsed.");
        }

        public static void UntillChildElementDisplay(IWebElement parentElement, By childElement, int timeOut)
        {

            List<IWebElement> childElements = parentElement.FindElements(childElement).ToList();
            Boolean isChildElementDisplayed = false;

            for (int i = 1; i <= timeOut; i = i + 2)
            {
                if (childElements.Count > 0)
                {
                    isChildElementDisplayed = true;
                    break;
                }

                Console.WriteLine("Waiting for child elements");

                childElements = parentElement.FindElements(childElement).ToList();
                Thread.Sleep(2000);
            }

            if (!isChildElementDisplayed)
                Assert.Fail("Time Out...... Overlay is displayed after time out.");
        }


        //This function will wait for the jQuery function execution
        public static Boolean isJqueryActive(IWebDriver driver)
        {
            Boolean ajaxIsComplete = false;
            for (int i = 1; i <= 60; i++)
            {
                ajaxIsComplete = (bool)(driver as IJavaScriptExecutor).ExecuteScript("return window.jQuery.active == 0");
                Console.WriteLine("ajaxIsComplete :: " + ajaxIsComplete);
                if (ajaxIsComplete)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
            return ajaxIsComplete;
        }

        // This function performs scroll up on page
        public static Boolean scrollUp(IWebDriver driver)
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

        public static string getGuid()
        {
            Guid g;

            // Create and display the value of  GUID.
            g = Guid.NewGuid();

            string output = g.ToString().Replace("-", "");

            Logger.log.Info("GUID :: " + output);

            return output;
        }

        //This function will wait for the javascript function execution
        public static Boolean isJavaScriptActive(IWebDriver driver)
        {
            Boolean javaScriptIsComplete = false;
            for (int i = 1; i <= 60; i++)
            {
                String state = ((IJavaScriptExecutor)driver).ExecuteScript(@"return document.readyState").ToString();
                javaScriptIsComplete = state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) || state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase);

                Logger.log.Info("Java Script Complete :: " + javaScriptIsComplete);
                Console.WriteLine("Java Script Complete :: " + javaScriptIsComplete);

                if (javaScriptIsComplete)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
            return javaScriptIsComplete;
        }
    }
}
