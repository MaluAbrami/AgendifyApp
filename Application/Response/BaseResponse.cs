namespace Application.Response;

public class BaseResponse<T> : BaseResponseExtensions
{
    public ResponseInfo? ResponseInfo { get; set; }
    public T? Value { get; set; }
}