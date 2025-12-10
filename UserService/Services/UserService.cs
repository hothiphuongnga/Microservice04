using AutoMapper;
using UserService.Dtos;
using UserService.Models;
using UserService.Repositories;
using UserService.Repositories.Base;
using UserService.Services.Base;
namespace UserService.Services{


public interface IUserService : IServiceBase<User, UserDto>
{

}

public class UserServicee : ServiceBase<User, UserDto>, IUserService
{
    private readonly IUserRepository _userRepository;

    public UserServicee(IUnitOfWork uow, IMapper mapper, IUserRepository userRepository) 
        : base(uow, mapper, userRepository)
    {
        _userRepository = userRepository;
    }

   
}
}
