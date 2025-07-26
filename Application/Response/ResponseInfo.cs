namespace Application.Response;

public class ResponseInfo
{
    public string? Title { get; set; }
    public string? ErrorDescription { get; set; }
    public int HttpStatus { get; set; }
}