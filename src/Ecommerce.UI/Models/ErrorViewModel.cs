namespace Ecommerce.UI.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string Message { get; set; }
        public string Title { get; set; }
        public int ErrorCode { get; set; }
    }
}