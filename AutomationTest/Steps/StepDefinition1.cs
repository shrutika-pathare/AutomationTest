using AutomationTest.Utilities;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AutomationTest.Steps
{
    [Binding]

    public class StepDefination
    {
        readonly IWebDriver driver;

        [When(@"User navigates to google")]
    public void WhenUserNavigatesToGoogle()
    {
              Objects.objGooglePO.navigateToGoogle();
        }

        [When(@"Search Test in search text field")]
    public void WhenSearchTestInSearchTextField()
    {
              Objects.objGooglePO.searchText();
        }

        [When(@"User clicks on First link")]
    public void WhenUserClicksOnFirstLink()
    {
              Objects.objGooglePO.searchText();
        }

        [Then(@"User should be able to navigate to the result page")]
    public void ThenUserShouldBeAbleToNavigateToTheResultPage()
    {
            Objects.objGooglePO.resultPage();
        }

    }
}
