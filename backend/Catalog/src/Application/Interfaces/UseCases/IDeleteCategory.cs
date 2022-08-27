using Application.Dtos.Category;
using MediatR;

namespace Application.Interfaces.UseCases;

public interface IDeleteCategory : IRequestHandler<DeleteCategoryInput> { }
