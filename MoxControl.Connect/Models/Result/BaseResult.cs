namespace MoxControl.Connect.Models.Result
{
    public class BaseResult
    {
        public BaseResult(bool success, string? errorMessage = null)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
