using MediatR;

namespace Application.Dtos.Category;

public class DeleteCategoryInput : IRequest
{
    public Guid Id { get; set; }

    public DeleteCategoryInput(Guid id) => Id = id;
}
