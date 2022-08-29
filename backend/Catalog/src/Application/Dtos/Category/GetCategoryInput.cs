using MediatR;

namespace Application.Dtos.Category;

public class GetCategoryInput : IRequest<CategoryOutput>
{
    public Guid Id { get; set; }

    public GetCategoryInput(Guid id)
    {
        Id = id;
    }
}
