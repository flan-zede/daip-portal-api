namespace api.Wrappers
{
    public class QueryParam
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public int Size { get; set; }
        public string? Search { get; set; }
        public string[]? SearchKeys { get; set; }
    }
}
