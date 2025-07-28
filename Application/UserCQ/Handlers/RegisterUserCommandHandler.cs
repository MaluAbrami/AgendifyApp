using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using Application.Utils;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UserCQ.Handlers;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, BaseResponse<RefreshTokenViewModel>>
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public RegisterUserCommandHandler(IAuthService authService, IMapper mapper, UserManager<User> userManager)
    {
        _authService = authService;
        _mapper = mapper;
        _userManager = userManager;
    }
    
    public async Task<BaseResponse<RefreshTokenViewModel>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var isUniqueEmail = await _authService.UniqueEmail(request.Email);

            if (isUniqueEmail is ValidationFieldUserEnum.EmailUnavailable)
            {
                return BaseResponseExtensions.Fail<RefreshTokenViewModel>("Email indisponível",
                    "O email informado já está em uso", 400);
            }

            var user = _mapper.Map<User>(request);
            user.RefreshToken = await _authService.GenerateRefreshToken();

            var result = await _userManager.CreateAsync(user, request.Password);
            
            if (!result.Succeeded)
            {
                return BaseResponseExtensions.Fail<RefreshTokenViewModel>("Erro ao registrar usuário", string.Join("; ", result.Errors.Select(e => e.Description)), 400);
            }
            
            await _userManager.AddToRoleAsync(user, request.Role.ToString());

            var refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
            refreshTokenVM.TokenJwt = await _authService.GenerateJwt(user.Email!, UserRoles.User);

            return BaseResponseExtensions.Sucess<RefreshTokenViewModel>(refreshTokenVM);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}