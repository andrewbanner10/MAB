using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestProject1.Code.Helpers;

namespace TestProject1.Code.PageObjects
{
    public class ResultsPageObjects
    {
        private readonly IWebDriver _webDriver;
        private readonly WebDriverWait _wait;
        private readonly NetworkCapture _networkCapture;

        public const int DefaultWaitInSeconds = 10;
        private IList<string> pageResults = new List<string>();

        public By PageLoading = By.CssSelector(".loading-spinner js-product-list-spinner");
        public By ResultsLoaded => By.CssSelector(".product-list js-product-list");
        public By GetMortgageResults => By.CssSelector("[data-product-code]");

        public By GetInitialTermMonths => By.ClassName("js-product-result-initialTerm");
        public By GetInitalInterestRate => By.ClassName("js-product-result-initialRate");

        public By GetProductFees => By.ClassName("js-product-result-productFee");

        public By GetInitialMonthlyCost => By.ClassName("js-product-result-initialMonthlyPayment");

        public By GetTotalCost => By.ClassName("js-product-result-totalPayable");
        public By PageHeader => By.CssSelector(".quick-quote-results__heading");

        public By UpdateResultsTermsInput => By.Id("mortgageTerm");
        public By UpdateResultsDepositInput => By.Id("deposit");
        public By UpdateResultsIncomeInput => By.Id("income");
        public By UpdateResultsPropertyValueInput => By.Id("propertyValue");
        public By MortgagePurposeValue => By.Id("mortgagePurpose");

        public By FixedTermFilter => By.Id("fixedTerm");
        public By SortByFilter => By.Id("sortBy");

        public By NextPageButton => By.ClassName("js-next");

        public By UpdateResultsButton => By.CssSelector("button[type='submit'].btn.btn--secondary.btn--outline.js-submit-form-button");


        public ResultsPageObjects(IWebDriver webDriver, NetworkCapture networkCapture)
        {
            _webDriver = webDriver;
            _wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(DefaultWaitInSeconds));
            _networkCapture = networkCapture;
        }

        public void GetListOfMortgageResults()
        {
            ProductListLoaded();

            var listOfResults = _webDriver.FindElements(GetMortgageResults);
            Assert.That(listOfResults.Count > 0);

            // Create a fresh list for the raw text values
            var initialTextValues = new List<string>();

            foreach (var result in listOfResults)
            {
                Assert.That(result.Text, Is.Not.Null);
                initialTextValues.Add(result.Text); // Extract the string safely
            }

            // Save the raw strings instead of the web elements
            pageResults = initialTextValues;
        }

        public void PageHeaderShown()
        {
            Assert.That(_webDriver.FindElement(PageHeader).Text == "Here's what your next mortgage could look like");
        }

        public void UpdateResultsField(string fieldToUpdate, string value)
        {
            switch (fieldToUpdate.ToUpper())
            {
                case "TERM":
                    UpdateResultsTermsInput.WaitAndSendKeys(_webDriver, value + Keys.Tab);
                    IWebElement TermElement = _webDriver.FindElement(UpdateResultsTermsInput);
                    Assert.That(TermElement.GetAttribute("value") == value);
                    break;

                case "PROPERTYVALUE":
                    UpdateResultsPropertyValueInput.WaitAndSendKeys(_webDriver, value + Keys.Tab);
                    IWebElement propertyValueElement = _webDriver.FindElement(UpdateResultsPropertyValueInput);
                    Assert.That(propertyValueElement.GetAttribute("value") == value);
                    break;

                case "DEPOSIT":
                    UpdateResultsDepositInput.WaitAndSendKeys(_webDriver, value + Keys.Tab);
                    IWebElement depositElement = _webDriver.FindElement(UpdateResultsDepositInput);
                    Assert.That(depositElement.GetAttribute("value") == value);
                    break;

                case "INCOME":
                    UpdateResultsIncomeInput.WaitAndSendKeys(_webDriver, value + Keys.Tab);
                    IWebElement incomeElement = _webDriver.FindElement(UpdateResultsIncomeInput);
                    Assert.That(incomeElement.GetAttribute("value") == value);
                    break;

                case "MORTGAGEPURPOSE":
                    break;

                default:
                    break;
            }
        }

        public void ProductListLoaded()
        {
            PageLoading.WaitForInvisibility(_webDriver);
        }

        public void ClickUpdateResultsButton()
        {
            UpdateResultsButton.WaitAndClick(_webDriver);
        }


        public void CheckUpdatedResults()
        {
            ProductListLoaded();

            // Grab the brand new, updated elements from the DOM
            var listOfResults = _webDriver.FindElements(GetMortgageResults);

            for (int i = 0; i < listOfResults.Count; i++)
            {
                //check list of results differ (should be as different values)
                Assert.That(listOfResults[i].Text, Is.Not.EqualTo(pageResults[i]));
            }
        }

        public void UpdateFixedTermValue(string value)
        {
            ProductListLoaded();

            IWebElement dropdownElement = _webDriver.FindElement(FixedTermFilter);

            SelectElement selectFixedTerm = new SelectElement(dropdownElement);

            selectFixedTerm.SelectByText(value);
        }

        public void CheckFixedTermFilterUpdates(int minMonths)
        {
            ProductListLoaded();

            var listOfResults = _webDriver.FindElements(GetInitialTermMonths);

            foreach (var result in listOfResults)
            {
                if (result.Displayed == true)
                {

                    Assert.That(Convert.ToInt32(result.Text), Is.InRange(minMonths, minMonths + 11));
                }
            }
        }

        public void UpdateSortBy(string value)
        {
            ProductListLoaded();

            IWebElement dropdownElement = _webDriver.FindElement(SortByFilter);

            SelectElement selectFixedTerm = new SelectElement(dropdownElement);
            string currentlySelectedText = selectFixedTerm.SelectedOption.Text;

            if (currentlySelectedText != value)
            {
                selectFixedTerm.SelectByText(value);
            }
        }

        public void CheckSortingFilterCorrect(string value)
        {
            ProductListLoaded();

            List<double> valuesToCheck = new List<double>();

            switch (value)
            {
                case "Total Cost":

                    var totalCost = _webDriver.FindElements(GetTotalCost);

                    Assert.That(totalCost.Count > 0);

                    foreach (var result in totalCost)
                    {

                        if (result.Displayed == true)
                        {
                            double actualValue = double.Parse(result.Text);
                            valuesToCheck.Add(actualValue);
                        }

                    }

                    for (int i = 0; i < valuesToCheck.Count - 1; i++)
                    {
                        double currentItem = valuesToCheck[i];
                        double nextItem = valuesToCheck[i + 1];

                        Assert.That(currentItem, Is.LessThanOrEqualTo(nextItem),
                            $"Row {i + 1} is out of order! Expected '{currentItem}' to be less than or equal to '{nextItem}'.");
                    }

                    break;
                case "Initial Interest Rate":

                    var rate = _webDriver.FindElements(GetInitalInterestRate);

                    Assert.That(rate.Count > 0);

                    foreach (var result in rate)
                    {

                        if (result.Displayed == true)
                        {
                            double actualValue = double.Parse(result.Text);
                            valuesToCheck.Add(actualValue);
                        }

                    }

                    for (int i = 0; i < valuesToCheck.Count - 1; i++)
                    {
                        double currentItem = valuesToCheck[i];
                        double nextItem = valuesToCheck[i + 1];

                        Assert.That(currentItem, Is.LessThanOrEqualTo(nextItem),
                            $"Row {i + 1} is out of order! Expected '{currentItem}' to be less than or equal to '{nextItem}'.");
                    }
                    break;

                case "Product Fees":

                    var fees = _webDriver.FindElements(GetProductFees);

                    Assert.That(fees.Count > 0);

                    foreach (var result in fees)
                    {

                        if (result.Displayed == true)
                        {
                            double actualValue = double.Parse(result.Text);
                            valuesToCheck.Add(actualValue);
                        }

                    }

                    for (int i = 0; i < valuesToCheck.Count - 1; i++)
                    {
                        double currentItem = valuesToCheck[i];
                        double nextItem = valuesToCheck[i + 1];

                        Assert.That(currentItem, Is.LessThanOrEqualTo(nextItem),
                            $"Row {i + 1} is out of order! Expected '{currentItem}' to be less than or equal to '{nextItem}'.");
                    }
                    break;

                case "Initial Monthly Cost":

                    var initial = _webDriver.FindElements(GetInitialMonthlyCost);

                    Assert.That(initial.Count > 0);

                    foreach (var result in initial)
                    {

                        if (result.Displayed == true)
                        {
                            double actualValue = double.Parse(result.Text);
                            valuesToCheck.Add(actualValue);
                        }

                    }

                    for (int i = 0; i < valuesToCheck.Count - 1; i++)
                    {
                        double currentItem = valuesToCheck[i];
                        double nextItem = valuesToCheck[i + 1];

                        Assert.That(currentItem, Is.LessThanOrEqualTo(nextItem),
                            $"Row {i + 1} is out of order! Expected '{currentItem}' to be less than or equal to '{nextItem}'.");
                    }
                    break;

                default: throw new ArgumentException("Value supplied not valid");
            }
        }

        public void GetCountAndCompareUIAPI()
        {
            ProductListLoaded();

            IWebElement nextButton = _webDriver.FindElement(NextPageButton);

            while (nextButton.Displayed == true)
            {
                Assert.That(
                    _networkCapture.WaitForGetResultsResponse(TimeSpan.FromSeconds(30)),
                    Is.True,
                    "Timed out waiting for the getresults API response.");

                var apiResponse = _networkCapture.GetParsedResponse();
                Assert.That(apiResponse, Is.Not.Null, "Could not parse the getresults response JSON.");

                var metrics = _networkCapture.ExtractPayloadMetrics();
                var uiDisplayedCount = _webDriver.FindElements(GetMortgageResults).Count;

                Assert.That(uiDisplayedCount, Is.GreaterThan(0), "No mortgage products were shown in the UI.");
                Assert.That(
                    uiDisplayedCount,
                    Is.EqualTo(metrics.PageItemCount),
                    $"UI shows {uiDisplayedCount} products but the API returned {metrics.PageItemCount} items on this page.");
                Assert.That(
                    metrics.TotalCount,
                    Is.GreaterThanOrEqualTo(uiDisplayedCount),
                    $"API totalCount ({metrics.TotalCount}) is less than the number of products shown ({uiDisplayedCount}).");

                nextButton.Click(); 
                nextButton = _webDriver.FindElement(NextPageButton);
            }
        }
      
    }
}