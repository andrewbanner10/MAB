
using Reqnroll;
using TestProject1.Code.PageObjects;
using TestProject1.Specs.Drivers;

namespace TestProject1.Steps
{
    [Binding]
    public sealed class Common
    {
        private readonly CommonObjects _commonObjects;

        public Common(BrowserDriver browserDriver)
        {
            _commonObjects = new CommonObjects(browserDriver.Current);
        }

        [Given(@"the user loads the webpage ""([^""]*)""")]
        public void GivenTheUserLoadsTheWebpage(string url)
        {
            _commonObjects.GoToUrl(url);
            _commonObjects.CheckForCookies();
        }
    }
}
