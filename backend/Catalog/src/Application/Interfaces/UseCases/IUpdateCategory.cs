using Application.Dtos.Category;
using MediatR;

namespace Application.Interfaces.UseCases;

public interface IUpdateCategory : IRequestHandler<UpdateCategoryInput, CategoryOutput> { }
