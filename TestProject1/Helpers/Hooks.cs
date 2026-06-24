using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;
using Reqnroll.BoDi;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestProject1.Code.Helpers;
using TestProject1.Specs.Drivers;

namespace TestProject1.Specs.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly BrowserDriver _browserDriver;
        private readonly ScenarioContext _scenarioContext;
        private string _scenarioFolder;

        public Hooks(BrowserDriver browserDriver, ScenarioContext scenarioContext)
        {
            _browserDriver = browserDriver;
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario(Order = 0)]
        public void RegisterNetworkCapture(IObjectContainer container)
        {
            container.RegisterInstanceAs(new NetworkCapture());
        }

        [BeforeScenario(Order = 1)]
        public async Task StartNetworkCapture(NetworkCapture networkCapture)
        {
            await networkCapture.SetupNetworkLoggingAsync(_browserDriver.Current);
        }

        [AfterScenario(Order = 0)]
        public async Task StopNetworkCapture(NetworkCapture networkCapture)
        {
            await networkCapture.StopMonitoringAsync();
        }

        [BeforeScenario]
        public void SetupDirectoryStructure()
        {
            // Get Path to Desktop/TestRuns
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string mainTestRunsFolder = Path.Combine(desktopPath, "TestRuns");

            // Create TestRuns folder if it doesn't exist
            if (!Directory.CreateDirectory(mainTestRunsFolder).Exists)
            {
                Directory.CreateDirectory(mainTestRunsFolder);
            }

            //  Create scenario-specific folder name: "ScenarioName_yyyyMMdd_HHmmss"
            string rawTitle = _scenarioContext.ScenarioInfo.Title;
            string safeScenarioTitle = string.Concat(rawTitle.Split(Path.GetInvalidFileNameChars()));
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            _scenarioFolder = Path.Combine(mainTestRunsFolder, $"{safeScenarioTitle}_{timestamp}");


            // Actually create the folder on the Desktop
            Directory.CreateDirectory(_scenarioFolder);
        }

        [AfterScenario(Order = 1)]
        public void LogResultAndCaptureScreenshot()
        {
            string logFilePath = Path.Combine(_scenarioFolder, "TestResult.txt");

            // Scenario FAILED
            if (_scenarioContext.TestError != null)
            {
                // Write the failure log
                string failureMessage = $"STATUS: FAILED{Environment.NewLine}" +
                                        $"Timestamp: {DateTime.Now}{Environment.NewLine}" +
                                        $"Error Details: {_scenarioContext.TestError.Message}{Environment.NewLine}" +
                                        $"Stack Trace: {_scenarioContext.TestError.StackTrace}";

                File.WriteAllText(logFilePath, failureMessage);

                //  Capture the screenshot inside the scenario folder
                try
                {
                    IWebDriver driver = _browserDriver.Current;
                    ITakesScreenshot ts = (ITakesScreenshot)driver;
                    Screenshot screenshot = ts.GetScreenshot();

                    string screenshotPath = Path.Combine(_scenarioFolder, "FailureScreenshot.png");
                    screenshot.SaveAsFile(screenshotPath);

                    Console.WriteLine($"Scenario failed. Logs and screenshot saved to: {_scenarioFolder}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Screenshot Error]: Could not capture page. Details: {ex.Message}");
                }
            }
            // Scenario PASSED
            else
            {
                string successMessage = $"STATUS: PASSED{Environment.NewLine}" +
                                        $"Timestamp: {DateTime.Now}{Environment.NewLine}" +
                                        $"All assertions passed successfully.";

                File.WriteAllText(logFilePath, successMessage);
                Console.WriteLine($"Scenario passed. Log saved to: {_scenarioFolder}");
            }
        }
    }
}