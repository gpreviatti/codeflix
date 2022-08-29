using Application.Dtos.Category;
using MediatR;

namespace Application.Interfaces.UseCases;

public interface IGetCategory : IRequestHandler<GetCategoryInput, CategoryOutput> { }
