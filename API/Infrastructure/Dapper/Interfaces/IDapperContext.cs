namespace Infrastructure.Dapper.Interfaces;

public interface IDapperContext
{
    public Task<T?> FirstOrDefault<T>(IQueryObject queryObject);
    public Task<List<T>> ListOrDefault<T>(IQueryObject queryObject);
    public Task Command(IQueryObject queryObject);
    public Task<T> CommandWithResponse<T>(IQueryObject queryObject);
    public Task<List<TResult>> QueryWithJoin<T1, T2, TResult>(
        IQueryObject queryObject,
        Func<T1, T2, TResult> map,
        string splitOn = "Id"); 
}