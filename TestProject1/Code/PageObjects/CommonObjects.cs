using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using TestProject1.Code.Helpers; 

namespace TestProject1.Code.PageObjects
{
    public class CommonObjects
    {
        private readonly IWebDriver _webDriver;
        private readonly WebDriverWait _wait;
        public const int DefaultWaitInSeconds = 10;

        // 2. Change this from an IWebElement to a By locator
        public By AcceptPrivacyLocator => By.Id("onetrust-accept-btn-handler");

        public CommonObjects(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            _wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(DefaultWaitInSeconds));
        }

        public void GoToUrl(string url)
        {
            _webDriver.Url = url;
        }

        public void CheckForCookies()
        {
            // 3. Now you can call the extension method directly on the By locator
            AcceptPrivacyLocator.WaitAndClick(_webDriver);
        }
    }
}