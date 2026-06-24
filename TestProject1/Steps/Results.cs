
using Reqnroll;
using TestProject1.Code.PageObjects;
using TestProject1.Specs.Drivers;
using TestProject1.Code.Helpers;

namespace TestProject1.Steps
{
    [Binding]
    public class Results
    {
        private readonly ResultsPageObjects _resultsPageObjects;

        // 1. Inject BrowserDriver and NetworkCapture into the step file
        public Results(BrowserDriver browserDriver, NetworkCapture networkCapture)
        {
            _resultsPageObjects = new ResultsPageObjects(browserDriver.Current, networkCapture);
        }

        [Then(@"the user sees mortgage options with data")]
        public void ThenTheUserSeesMortgageOptionsWithData()
        {
            _resultsPageObjects.GetListOfMortgageResults();
        }

        [Given(@"the user is on the results page")]
        public void GivenTheUserIsOnTheResultsPage()
        {
            _resultsPageObjects.PageHeaderShown();
        }

        [When(@"the user updates the '([^']*)' value to '([^']*)'")]
        public void WhenTheUserUpdatesTheValueTo(string inputToUpdate, string valueToUpdate)
        {
            _resultsPageObjects.UpdateResultsField(inputToUpdate, valueToUpdate);
        }

        [When(@"presses update results button")]
        public void WhenPressesUpdateResultsButton()
        {
            _resultsPageObjects.ClickUpdateResultsButton();
        }

        [Then(@"the results are updated")]
        public void ThenTheResultsAreUpdated()
        {
            _resultsPageObjects.CheckUpdatedResults();
        }

        [When(@"the user updates the fixed term value to '([^']*)'")]
        public void WhenTheUserUpdatesTheFixedTermValueTo(string value)
        {
            _resultsPageObjects.UpdateFixedTermValue(value);
        }

        [Then(@"the fixed term results are updated and contain '([^']*)'")]
        public void ThenTheResultsAreUpdatedAndContain(int value)
        {
            _resultsPageObjects.CheckFixedTermFilterUpdates(value);
        }


        [When(@"the user updates the sort by value to '([^']*)'")]
        public void WhenTheUserUpdatesTheSortByValueTo(string sort)
        {
            _resultsPageObjects.UpdateSortBy(sort);
        }

        [Then(@"the results are updated and sorted by '([^']*)'")]
        public void ThenTheResultsAreUpdatedAndSortedBy(string sort)
        {
            _resultsPageObjects.CheckSortingFilterCorrect(sort);
        }

        [Then(@"the user is given the correct amount of results")]
        public void ThenTheUserIsGivenTheCorrectAmountOfResults()
        {
            _resultsPageObjects.GetCountAndCompareUIAPI();
        }
    }

}
