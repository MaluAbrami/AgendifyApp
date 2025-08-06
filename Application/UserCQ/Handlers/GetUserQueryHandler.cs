using Application.Response;
using Application.UserCQ.Querys;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UserCQ.Handlers;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, BaseResponse<UserViewModel>>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<UserViewModel>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return BaseResponseExtensions.Fail<UserViewModel>("Usuário não encontrado",
                "Não foi encontrado nenhum usuário com o id informado", 404);
        
        var userVM = _mapper.Map<UserViewModel>(user);
        
        return BaseResponseExtensions.Sucess(userVM);
    }
}