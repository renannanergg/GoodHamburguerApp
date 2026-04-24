

namespace GoodHamburguerApp.Application.DTOs
{
    public class ApiResponse<T> where T : class
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public ApiResponse(T data, string message = null)
        {
            Success = true;
            Data = data;
            Message = message;
        }
    }
}
