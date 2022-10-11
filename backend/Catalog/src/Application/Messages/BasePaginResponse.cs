namespace Application.Messages;
public class BasePaginResponse<TData> : BaseResponse<TData> where TData : class
{
    public BasePaginResponse(TData data, int page, int per_Page, int filtred, int total) : base(data)
    {
        Meta = new(page, per_Page, filtred, total);
    }

    public Meta Meta { get; set; }
}

public class Meta
{
    public Meta(int page, int per_Page, int filtred, int total)
    {
        Page = page;
        Per_Page = per_Page;
        Filtred = filtred;
        Total = total;
    }

    public int Page { get; set; }
    public int Per_Page { get; set; }
    public int Filtred { get; set; }
    public int Total { get; set; }
}
