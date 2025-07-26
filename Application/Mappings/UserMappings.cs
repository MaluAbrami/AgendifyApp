using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class UserMappings : Profile
{
    public UserMappings()
    {
        CreateMap<RegisterUserCommand, User>()
            .ForMember(x => x.RefreshToken, x => x.AllowNull())
            .ForMember(x => x.RefreshTokenExpirationTime, x => x.MapFrom(x => AddTenDays))
            .ForMember(x => x.PasswordHash, x => x.AllowNull());
        
        CreateMap<User, RefreshTokenViewModel>()
            .ForMember(x => x.TokenJwt, x => x.AllowNull());

        CreateMap<RefreshTokenViewModel, UserViewModel>();
    }
    
    private DateTime AddTenDays => DateTime.Now.AddDays(10);
}