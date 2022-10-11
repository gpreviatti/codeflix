namespace Application.Messages;
public class BaseResponse<TData> where TData : class
{
    public BaseResponse(TData data) => Data = data;

    public TData Data { get; set; }
}
