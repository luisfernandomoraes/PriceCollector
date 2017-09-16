using System.Net;

namespace PriceCollector.WebAPI.Responses
{
    public class ApiResponse<T> where T : class
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public T Result { get; set; }
    }
}