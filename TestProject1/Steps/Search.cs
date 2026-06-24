using System;
using Reqnroll;
using TestProject1.Code.PageObjects;
using TestProject1.Specs.Drivers;
using FluentAssertions;
using FluentAssertions.Execution;

namespace TestProject1
{
    [Binding]
    public sealed class Search
    {
        private readonly SearchPageObjects _searchPage;

        public Search(BrowserDriver browserDriver)
        {
            _searchPage = new SearchPageObjects(browserDriver.Current);
        }

        [When(@"the user enters the following mortgage data")]
        public void WhenTheUserEntersTheFollowingMortgageData(Table table)
        {
            _searchPage.EnterMortgageCalculatorDetails(table);
        }



    }
}
