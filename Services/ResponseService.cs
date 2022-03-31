using api.Wrappers;

namespace api.Services
{
    public class ResponseService
    {
        public static Response<List<T>> Format<T>(int length, QueryParam query, List<T> data)
        {
            var res = new Response<List<T>>();
            res.Length = length;
            res.Search = query.Search;
            res.Page = new Page(query.Index, query.Size);
            res.Data = data;
            return res;
        }
    }
}