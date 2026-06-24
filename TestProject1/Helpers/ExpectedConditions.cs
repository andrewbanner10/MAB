using System;
using OpenQA.Selenium;

namespace TestProject1.Code.Helpers
{
    public static class ExpectedConditions
    {

        // Native replacement for checking if an element is visible on screen.

        public static Func<IWebDriver, IWebElement> ElementIsVisible(By locator)
        {
            return (driver) =>
            {
                try
                {
                    var element = driver.FindElement(locator);
                    return element.Displayed ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }

        //replacement for waiting until an element is completely hidden or gone.

        public static Func<IWebDriver, bool> InvisibilityOfElementLocated(By locator)
        {
            return (driver) =>
            {
                try
                {
                    var element = driver.FindElement(locator);
                    return !element.Displayed;
                }
                catch (NoSuchElementException)
                {
                    return true; // Already gone
                }
                catch (StaleElementReferenceException)
                {
                    return true; // Stale means it was removed from DOM
                }
            };
        }

        // replacement for checking if an element is clickable.

        public static Func<IWebDriver, IWebElement> ElementToBeClickable(By locator)
        {
            return (driver) =>
            {
                var element = ElementIsVisible(locator)(driver);
                try
                {
                    if (element != null && element.Enabled)
                    {
                        return element;
                    }
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }
    }
}