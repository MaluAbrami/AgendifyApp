using Application.Response;
using Application.UserCQ.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UserCQ.Handlers;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, BaseResponse<DeleteUserCommand>>
{
    private readonly UserManager<User> _userManager;

    public DeleteUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<BaseResponse<DeleteUserCommand>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            return BaseResponseExtensions.Fail<DeleteUserCommand>("Usuário não encontrado",
                "Não foi encontrado nenhum usuário com o id informado", 404);
        
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return BaseResponseExtensions.Fail<DeleteUserCommand>("Erro ao deletar usuário",
                "Não foi possível deletar o usuário com o id informado", 400);
        
        return BaseResponseExtensions.Sucess<DeleteUserCommand>(request);
    }
}