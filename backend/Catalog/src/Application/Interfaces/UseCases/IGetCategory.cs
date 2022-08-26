using Application.Dtos.Category;
using MediatR;

namespace Application.Interfaces.UseCases;

public interface IGetCategory : IRequestHandler<GetCategoryInput, GetCategoryOutput>
{
    public Task<GetCategoryOutput> Handle(GetCategoryInput input, CancellationToken cancellationToken);
}
