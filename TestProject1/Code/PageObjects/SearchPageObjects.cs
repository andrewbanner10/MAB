using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using Reqnroll;
using TestProject1.Code.Helpers;

namespace TestProject1.Code.PageObjects
{
    public class SearchPageObjects
    {
        private readonly IWebDriver _webDriver;
        private readonly WebDriverWait _wait;
        public const int DefaultWaitInSeconds = 10;

        // Locators converted to 'By' objects
        public By PropertyValueInput => By.Name("propertyValue");
        public By MortgageTermInput => By.Name("mortgageTerm");
        public By DepositInput => By.Name("deposit");
        public By IncomeOfAllApplicantsInput => By.Name("income");
        public By GetResultsButton => By.ClassName("calculate-button");

        public SearchPageObjects(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            _wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(DefaultWaitInSeconds));
        }
        public void EnterMortgageCalculatorDetails(Table table)
        {
            var dictionary = Utils.ToDictionary(table);
 
            PropertyValueInput.WaitAndSendKeys(_webDriver,dictionary["propertyValue"]);
            MortgageTermInput.WaitAndSendKeys(_webDriver,dictionary["term"]);
            DepositInput.WaitAndSendKeys(_webDriver,dictionary["deposit"]);
            IncomeOfAllApplicantsInput.WaitAndSendKeys(_webDriver,dictionary["income"]);


            GetResultsButton.WaitAndClick(_webDriver);

        }
    }
}