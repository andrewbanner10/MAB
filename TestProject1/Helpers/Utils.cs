using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Data;

namespace TestProject1.Code.Helpers
{
    public static class Utils
    {
        private const int DefaultWaitSeconds = 10;

        // Extension method for wait then click
        public static void WaitAndClick(this By locator, IWebDriver driver, int timeoutInSeconds = DefaultWaitSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

            var element = wait.Until(d =>
            {
                try
                {
                    var el = d.FindElement(locator);
                    if (el != null && el.Displayed && el.Enabled)
                    {
                        // 1. Grab the current position of the element
                        var initialLocation = el.Location;

                        // 2. Wait a tiny fraction of a second (50ms) to check if it's moving
                        System.Threading.Thread.Sleep(50);

                        // 3. Grab the position again
                        var currentLocation = el.Location;

                        // 4. Only return the element if it has completely stopped moving
                        if (initialLocation.X == currentLocation.X && initialLocation.Y == currentLocation.Y)
                        {
                            return el;
                        }
                    }
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null; // Handle if the element re-renders mid-animation
                }
            });

            element.Click();
        }

        public static void WaitAndSendKeys(this By locator, IWebDriver driver, string text, int timeoutInSeconds = DefaultWaitSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            var element = wait.Until(d =>
            {
                var el = d.FindElement(locator);
                return (el != null && el.Displayed) ? el : null;
            });
            element.Clear(); // Good practice to clear before typing
            element.SendKeys(text);
        }

        /// <summary>
        /// Waits for an element (like a loading spinner) to either become invisible or disappear from the DOM completely.
        /// </summary>
        public static bool WaitForInvisibility(this By locator, IWebDriver driver, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

            try
            {
                // STEP 1: Wait explicitly for the element to exist and be visible first
                // This stops the race condition where the script moves too fast before the spinner even renders
                // This is natively supported by Selenium 4+ out of the box
                wait.Until(d => d.FindElement(locator).Displayed);

                // STEP 2: Now wait for it to completely disappear (either display:none or removed from DOM)
                return wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
            }
            catch (WebDriverTimeoutException)
            {
                // If it never appears, or appears and never leaves within the timeout
                return false;
            }
        }



        //convert table to dictionary
        public static Dictionary<string, string> ToDictionary(Table table)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var row in table.Rows)
            {
                dictionary.Add(row[0], row[1]);
            }
            return dictionary;
        }

        //scrolls to the bottom of page (could also add scroll to a certain element)
        public static void ScrollToBottomOfPage(IWebDriver driver)
        { 
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                ((IJavaScriptExecutor)driver).ExecuteScript(
                "window.scrollTo(0, document.documentElement.scrollHeight);");
                wait.Until(driver =>
                     {
                var js = (IJavaScriptExecutor)driver;
                var scrollTop = Convert.ToInt64(js.ExecuteScript("return window.scrollY;"));
                var viewportHeight = Convert.ToInt64(js.ExecuteScript("return window.innerHeight;"));
                var scrollHeight = Convert.ToInt64(js.ExecuteScript("return document.documentElement.scrollHeight;"));
                return scrollTop + viewportHeight >= scrollHeight - 2;
                     });
        }


    }
}