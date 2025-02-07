using Infrastructure.Dapper;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Models;
using Infrastructure.Models.InterfacesRepositories;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDapperContext dapperContext;

    public UserRepository(IDapperContext _dapperContext)
    {
        dapperContext = _dapperContext;
    }
    
    public async Task<User> AddUser(string email, string password_Hash, string role)
    {
        var queryObject = new QueryObject(
            "INSERT INTO USERS(email, password_Hash, role) VALUES (@email, @password_Hash, @role) RETURNING id as \"UserId\", email as \"Email\", password_Hash as \"PasswordHash\"",
            new {email, password_Hash, role });
        return await dapperContext.CommandWithResponse<User>(queryObject);
    }

    public async Task<User?> GetUserById(int id)
    {
        var queryObject = new QueryObject(
            "SELECT id as \"UserId\", email as \"Email\", password_Hash as \"PasswordHash\", role as \"Role\" FROM USERS WHERE id = @id", new {id});
        return await dapperContext.FirstOrDefault<User>(queryObject);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var queryObject = new QueryObject(
            $"SELECT id as \"UserId\", email as \"Email\", password_Hash as \"PasswordHash\", role as \"Role\" FROM USERS WHERE email = @email", new {email});
        return await dapperContext.FirstOrDefault<User>(queryObject);
    }
}