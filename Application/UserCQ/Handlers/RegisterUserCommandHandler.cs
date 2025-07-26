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
                return new BaseResponse<RefreshTokenViewModel>
                {
                    ResponseInfo = new ResponseInfo()
                    {
                        Title = "Email indisponível",
                        ErrorDescription = $"O email informado {request.Email} já está em uso",
                        HttpStatus = 400
                    },
                    Value = null
                };
            }

            var user = _mapper.Map<User>(request);
            user.RefreshToken = await _authService.GenerateRefreshToken();

            var result = await _userManager.CreateAsync(user, request.Password);
            
            if (!result.Succeeded)
            {
                return new BaseResponse<RefreshTokenViewModel>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Erro ao registrar usuário",
                        ErrorDescription = string.Join("; ", result.Errors.Select(e => e.Description)),
                        HttpStatus = 400
                    },
                    Value = null
                };
            }
            
            await _userManager.AddToRoleAsync(user, UserRoles.User);

            var refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
            refreshTokenVM.TokenJwt = await _authService.GenerateJwt(user.Email!, UserRoles.User);
            
            return new BaseResponse<RefreshTokenViewModel>
            {
                ResponseInfo = null,
                Value = refreshTokenVM
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}