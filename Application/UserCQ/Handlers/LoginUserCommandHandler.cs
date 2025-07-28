using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UserCQ.Handlers;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, BaseResponse<RefreshTokenViewModel>>
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public LoginUserCommandHandler(IAuthService authService, IMapper mapper, UserManager<User> userManager)
    {
        _authService = authService;
        _mapper = mapper;
        _userManager = userManager;
    }
    
    public async Task<BaseResponse<RefreshTokenViewModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userExist = await _userManager.FindByEmailAsync(request.Email);

            if (userExist == null)
            {
                return BaseResponseExtensions.Fail<RefreshTokenViewModel>("Credenciais inválidas", "Email ou senha incorretos",
                    401);
            }

            var validPassword = await _userManager.CheckPasswordAsync(userExist, request.Password);

            if (!validPassword)
            {
                return BaseResponseExtensions.Fail<RefreshTokenViewModel>("Credenciais inválidas", "Email ou senha incorretos",
                    401);
            }
            
            var roles = await _userManager.GetRolesAsync(userExist);
            var role = roles.FirstOrDefault();
            
            var refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(userExist);
            refreshTokenVM.TokenJwt = await _authService.GenerateJwt(userExist.Id, role);

            return BaseResponseExtensions.Sucess<RefreshTokenViewModel>(refreshTokenVM);
        }   
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}