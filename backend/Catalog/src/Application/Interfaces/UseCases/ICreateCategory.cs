using Application.Dtos.Category;
using Application.Messages;
using MediatR;

namespace Application.Interfaces.UseCases;

public interface ICreateCategory : IRequestHandler<CreateCategoryInput, BaseResponse<CategoryOutput>> { }
