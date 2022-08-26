using MediatR;

namespace Application.Dtos.Category;

public class GetCategoryInput : IRequest<GetCategoryOutput>
{
    public Guid Id { get; set; }

    public GetCategoryInput(Guid id)
    {
        Id = id;
    }
}
