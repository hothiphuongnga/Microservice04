using AutoMapper;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User,UserDto>()
            .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles));
        
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles));

        // Role
        CreateMap<Role,RoleDto>()
            .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles));
        
        CreateMap<RoleDto, Role>()
            .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles));

        // UserRole
        CreateMap<UserRole,UserRoleDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
        
        CreateMap<UserRoleDto, UserRole>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
    }
}
