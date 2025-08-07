using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UserCQ.Handlers;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, BaseResponse<UserViewModel>>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    
    public async Task<BaseResponse<UserViewModel>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);
        if (user == null)
            return BaseResponseExtensions.Fail<UserViewModel>("Usuário não encontrado",
                "Não foi encontrado nenhum usuário com o id informado", 404);
        
        var updatedUser = _mapper.Map(request, user);
        
        var result = await _userManager.UpdateAsync(updatedUser);
        if (!result.Succeeded)
            return BaseResponseExtensions.Fail<UserViewModel>("Erro ao atualizar usuário",
                "Não foi possível atualizar os dados informados do usuário", 400);

        var userVM = _mapper.Map<UserViewModel>(user);
        return BaseResponseExtensions.Sucess<UserViewModel>(userVM);
    }
}