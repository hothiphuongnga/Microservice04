using AutoMapper;
using UserService.Dtos;
using UserService.Models;
using UserService.Repositories;
using UserService.Repositories.Base;
using UserService.Services.Base;
using UserService.Services.Helper;
namespace UserService.Services
{


    public interface IUserService : IServiceBase<User, UserDto>
    {
        Task<ResponseEntity> RegisterAsync(UserRegisterDTO userRegisterDto);
        Task<ResponseEntity> LoginAsync(UserLoginDTO userLoginDto);
    }

    public class UserServicee : ServiceBase<User, UserDto>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtAuthService _jwtAuthService;

        public UserServicee(IUnitOfWork uow, IMapper mapper, IUserRepository userRepository, IJwtAuthService jwtAuthService)
            : base(uow, mapper, userRepository)
        {
            _userRepository = userRepository;
            _jwtAuthService = jwtAuthService;
        }

        public async Task<ResponseEntity> RegisterAsync(UserRegisterDTO userRegisterDto)
        {

            var existingUser = await _userRepository.CheckExistAsync(userRegisterDto.Username, userRegisterDto.Email);
            if (existingUser != null)
            {
                return ResponseEntity.Fail("Username or Email already exists", 400);
            }
            // map có sẵn trong base
            var user = _map.Map<User>(userRegisterDto);
            user.PasswordHash = PasswordHelper.HashPassword(userRegisterDto.Password);
            // thêm role
            var userRole = new UserRole
            {
                RoleId = 2, // mặc định role user
                UserId = user.Id
            };
            user.UserRoles.Add(userRole);

            // gọi repo lưu
            await _userRepository.AddAsync(user);

            await _uow.SaveChangesAsync();
            var res = _map.Map<UserDto>(user);
            return ResponseEntity.Ok(res);
        }

        // login
        public async Task<ResponseEntity> LoginAsync(UserLoginDTO userLoginDto)
        {
            var user = await _userRepository.SingleOrDefaultAsync(x => x.Username == userLoginDto.Username);
            if (user == null || !PasswordHelper.VerifyPassword(userLoginDto.Password, user.PasswordHash))
            {
                return ResponseEntity.Fail("Invalid username or password", 401);
            }

            var token = _jwtAuthService.GenerateToken(user);
            return ResponseEntity.Ok(token, "Login successful");
        }
    }
}
