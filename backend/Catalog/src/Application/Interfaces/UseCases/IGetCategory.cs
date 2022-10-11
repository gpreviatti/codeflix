using Application.Dtos.Category;
using Application.Messages;
using MediatR;

namespace Application.Interfaces.UseCases;

public interface IGetCategory : IRequestHandler<GetCategoryInput, BaseResponse<CategoryOutput>> { }
