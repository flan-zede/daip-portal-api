using api.Wrappers;

namespace api.Services
{
    public class QueryParamService
    {
        public static QueryParam Control(QueryParam query)
        {
            string[] keys = {};
            query.Index = query.Index < 1 ? 1 : query.Index;
            query.Size = query.Size < 1 ? 10 : query.Size;
            query.SearchKeys = query.Search != null ? query.Search.Split(" ", StringSplitOptions.RemoveEmptyEntries) : keys;
            return query;
        }
    }
}