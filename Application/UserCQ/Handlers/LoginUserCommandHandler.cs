using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using Application.Utils;
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
                return new BaseResponse<RefreshTokenViewModel>
                {
                    ResponseInfo = new ResponseInfo()
                    {
                        Title = "Usuário não encontrado",
                        ErrorDescription = $"Não há nenhum usuário cadastrado com o email {request.Email}",
                        HttpStatus = 404
                    },
                    Value = null
                };
            }

            var validPassword = await _userManager.CheckPasswordAsync(userExist, request.Password);

            if (!validPassword)
            {
                return new BaseResponse<RefreshTokenViewModel>
                {
                    ResponseInfo = new ResponseInfo()
                    {
                        Title = "Senha incorreta",
                        ErrorDescription = "A senha informada está incorreta, tente novamente",
                        HttpStatus = 404
                    },
                    Value = null
                };
            }
            
            var refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(userExist);
            refreshTokenVM.TokenJwt = await _authService.GenerateJwt(userExist.Email!, UserRoles.User);

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