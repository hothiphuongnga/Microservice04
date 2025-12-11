using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;
using UserService.Repositories.Base;

namespace UserService.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    // kiểm tra username hoặc email đã tồn tại chưa
    Task<User> CheckExistAsync(string username, string email);

}
public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(UserDbServiceContext context) : base(context)
    {
    }
    public async Task<User> CheckExistAsync(string username, string email)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username || u.Email == email);
    }
}
