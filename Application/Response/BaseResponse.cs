namespace Application.Response;

public class BaseResponse<T>
{
    public ResponseInfo? ResponseInfo { get; set; }
    public T? Value { get; set; }
}