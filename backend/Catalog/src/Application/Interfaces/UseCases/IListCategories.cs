using Application.Dtos.Category;
using MediatR;

namespace Application.Interfaces.UseCases;

public interface IListCategories : IRequestHandler<ListCategoriesInput, ListCategoriesOutput> { }
