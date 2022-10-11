using Application.Dtos.Category;
using Application.Messages;
using MediatR;

namespace Application.Interfaces.UseCases;

public interface IListCategories : IRequestHandler<ListCategoriesInput, BasePaginResponse<List<CategoryOutput>>> { }
