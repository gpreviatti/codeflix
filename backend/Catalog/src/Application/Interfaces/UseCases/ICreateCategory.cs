using Application.Dtos.Category;
using MediatR;

namespace Application.Interfaces.UseCases;

public interface ICreateCategory : IRequestHandler<CreateCategoryInput, CreateCategoryOutput>
{
    public Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken);
}
