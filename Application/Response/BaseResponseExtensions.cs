namespace Application.Response;

public class BaseResponseExtensions
{
    public static BaseResponse<T> Fail<T>(string title, string errorDescription, int httpStatus) =>
        new BaseResponse<T>
        {
            ResponseInfo = new ResponseInfo()
            {
                Title = title,
                ErrorDescription = errorDescription,
                HttpStatus = httpStatus
            },
            Value = default!
        };

    public static BaseResponse<T> Sucess<T>(T value) =>
        new BaseResponse<T>
        {
            ResponseInfo = null,
            Value = value
        };
}