
namespace api.Wrappers
{
    public class Response<T>
    {
        public int Length { get; set; }
        public string? Search { get; set; }
        public Page? Page { get; set; }
        public T? Data { get; set; }
    }
}
