using System.Text.Json;
using OpenQA.Selenium;

namespace TestProject1.Code.Helpers
{
    public class NetworkCapture
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private INetwork? _network;
        private TaskCompletionSource<string>? _captureTcs;

        public string LastJsonPayload { get; private set; } = string.Empty;
        public string? LastGetResultsUrl { get; private set; }

        public async Task SetupNetworkLoggingAsync(IWebDriver driver)
        {
            await StopMonitoringAsync();

            LastJsonPayload = string.Empty;
            LastGetResultsUrl = null;
            _captureTcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            _network = driver.Manage().Network;
            _network.NetworkResponseReceived += OnResponseReceived;
            await _network.StartMonitoring();
        }

        public async Task StopMonitoringAsync()
        {
            if (_network == null)
            {
                return;
            }

            _network.NetworkResponseReceived -= OnResponseReceived;
            await _network.StopMonitoring();
            _network = null;
        }

        private void OnResponseReceived(object? sender, NetworkResponseReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.ResponseUrl))
            {
                return;
            }

            if (!e.ResponseUrl.Contains("getresults", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            Console.WriteLine($"Found getresults response: {e.ResponseUrl} (status {e.ResponseStatusCode})");

            LastGetResultsUrl = e.ResponseUrl;

            var body = e.ResponseBody;
            if (string.IsNullOrEmpty(body) && e.ResponseContent != null)
            {
                body = e.ResponseContent.ReadAsString();
            }

            if (string.IsNullOrEmpty(body))
            {
                Console.WriteLine("Warning: getresults response had no body.");
                return;
            }

            LastJsonPayload = body;
            _captureTcs?.TrySetResult(body);
        }

        public bool WaitForGetResultsResponse(TimeSpan timeout)
        {
            if (_captureTcs == null)
            {
                return false;
            }

            if (_captureTcs.Task.IsCompleted)
            {
                return _captureTcs.Task.Status == TaskStatus.RanToCompletion;
            }

            return _captureTcs.Task.Wait(timeout);
        }

        public MortgageApiResponse? GetParsedResponse()
        {
            if (string.IsNullOrEmpty(LastJsonPayload))
            {
                return null;
            }

            return JsonSerializer.Deserialize<MortgageApiResponse>(LastJsonPayload, JsonOptions);
        }

        //Gets the required data from the getresults
        public (int TotalPages, int TotalCount, int PageItemCount) ExtractPayloadMetrics()
        {
            var parsed = GetParsedResponse();
            if (parsed == null)
            {
                Console.WriteLine("Warning: No JSON payload has been captured yet.");
                return (0, 0, 0);
            }

            var totalPages = parsed.FormModel?.TotalPages ?? 0;
            var pageItemCount = parsed.ResultsViewModel?.Results?.Count ?? 0;
            var totalCount = totalPages > 0 && pageItemCount > 0
                ? totalPages * pageItemCount
                : pageItemCount;

            Console.WriteLine(
                $"[API Metrics] Total Pages: {totalPages} | Estimated Total Matches: {totalCount} | Current Page Items: {pageItemCount}");

            return (totalPages, totalCount, pageItemCount);
        }
    }
}
