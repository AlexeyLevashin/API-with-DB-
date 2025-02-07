using Infrastructure.Dapper.Interfaces;

namespace Infrastructure.Dapper;

public class QueryObject:IQueryObject
{
    public string Sql { get; set; }
    public object? Params { get; set; }

    public QueryObject(string sql, object? parametrs = null)
    {
        if (string.IsNullOrEmpty(sql))
        {
            throw new ArgumentException("Sql cannot be null or empty.");
        }
        Sql = sql;
        Params = parametrs;
    }
}