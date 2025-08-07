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

        CreateMap<User, UserViewModel>();
        
        //Mapeamento que ignora os valores nulos que vierem do update, assim garante que apenas vai dar update nos dados que foram realmente passados no endpoint
        CreateMap<UpdateUserCommand, User>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
    }
    
    private DateTime AddTenDays => DateTime.Now.AddDays(10);
}