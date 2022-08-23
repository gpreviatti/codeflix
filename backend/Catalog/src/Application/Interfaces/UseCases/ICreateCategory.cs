using Application.Dtos.Category;

namespace Application.Interfaces.UseCases;

public interface ICreateCategory
{
    public Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken);
}
