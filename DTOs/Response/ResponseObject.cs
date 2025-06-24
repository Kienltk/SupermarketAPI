namespace SupermarketAPI.DTOs.Response
{
    public class ResponseObject<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
    }
}