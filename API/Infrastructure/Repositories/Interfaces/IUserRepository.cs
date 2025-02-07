namespace Infrastructure.Models.InterfacesRepositories;

public interface IUserRepository
{
    public Task<User> AddUser(string email, string hashedPassword, string role);
    public Task<User> GetUserById(int id);
    public Task<User> GetUserByEmail(string email);
}