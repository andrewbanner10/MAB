namespace TestProject1.Code.Helpers
{
    public class MortgageApiResponse
    {
        public FormModel? FormModel { get; set; }
        public ResultsViewModel? ResultsViewModel { get; set; }
        public int LoanAmount { get; set; }
    }

    public class FormModel
    {
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
    }

    public class ResultsViewModel
    {
        public List<MortgageProduct>? Results { get; set; }
        public int ErrorType { get; set; }
        public string? Error { get; set; }
    }

    public class MortgageProduct
    {
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? LenderName { get; set; }
        public double InitialMonthlyPayment { get; set; }
    }
}
