using Application.Dtos.Category;
using MediatR;

namespace Application.Interfaces.UseCases;

public interface ICreateCategory : IRequestHandler<CreateCategoryInput, CategoryOutput> { }
