using System.Data;
using Dapper;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Dapper.Interfaces.Settings;
using Infrastructure.Dapper.Settings;
using Npgsql;

namespace Infrastructure.Dapper;

public class DapperContext:IDapperContext
{
    private IDapperSettings _dapperSettings;

    public DapperContext (IDapperSettings dapperSettings)
    {
        _dapperSettings = dapperSettings;
    }


    public async Task<T?> FirstOrDefault<T>(IQueryObject queryObject)
    {
        return await Execute(query => query.QueryFirstOrDefaultAsync(queryObject.Sql, queryObject.Params));
    }

    public async Task<List<T>> ListOrDefault<T>(IQueryObject queryObject)
    {
        return (await Execute(query => query.QueryAsync<T>(queryObject.Sql, queryObject.Params))).ToList();
    }

    public async Task Command(IQueryObject queryObject)
    {
        await Execute(query => query.QueryAsync(queryObject.Sql, queryObject.Params));
    }

    public async Task<T> CommandWithResponse<T>(IQueryObject queryObject)
    {
        return await Execute(query => query.QueryFirstAsync<T>(queryObject.Sql, queryObject.Params));
    }


    public async Task<List<TResult>> QueryWithJoin<T1, T2, TResult>(
        IQueryObject queryObject,
        Func<T1, T2, TResult> map,
        string splitOn = "Id")
    {
        return (await Execute(async query =>
        {
            var result = await query.QueryAsync(
                queryObject.Sql,
                map,
                queryObject.Params,
                splitOn: splitOn
            );

            return result.ToList();
        }));
    }

    private async Task<T> Execute<T>(Func<IDbConnection, Task<T>> query)
    {
        await using var connect = new NpgsqlConnection(_dapperSettings.ConnectionString);
        await connect.OpenAsync();
        var temp = await query(connect);
        await connect.CloseAsync();
        return temp;
    }
}





























