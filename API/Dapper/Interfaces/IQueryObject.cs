namespace Infrastructure.Dapper.Interfaces;

public interface IQueryObject
{
    public string Sql { get; set; }
    public object? Params { get; set; }
}