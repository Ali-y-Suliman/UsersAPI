namespace Users.Models
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; }
        public Boolean Success { get; set; }
        public int StatusCode { get; set; }

        public ServiceResponse(bool success, int statusCode, T? data, string message)
        {
            Success = success;
            StatusCode = statusCode;
            Data = data;
            Message = message;
        }
    }
}