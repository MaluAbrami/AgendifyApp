using System.Text.Json.Serialization;
using Application.Response;
using MediatR;

namespace Application.UserCQ.Commands;

public class DeleteUserCommand : IRequest<BaseResponse<DeleteUserCommand>>
{
    [JsonIgnore]
    public string Id { get; set; }
}