namespace EFCodeFirstSample.Models
{
    public class ErrorResponse
    {
        public string ErrorMessage { get; set; }

        public string? CorrelationId { get; set; }
    }
}
