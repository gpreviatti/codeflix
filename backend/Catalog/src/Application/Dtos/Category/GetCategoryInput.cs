using Application.Messages;
using MediatR;

namespace Application.Dtos.Category;

public class GetCategoryInput : IRequest<BaseResponse<CategoryOutput>>
{
    public Guid Id { get; set; }

    public GetCategoryInput(Guid id)
    {
        Id = id;
    }
}
