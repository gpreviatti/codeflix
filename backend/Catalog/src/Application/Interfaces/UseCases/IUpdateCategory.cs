using Application.Dtos.Category;
using Application.Messages;
using MediatR;

namespace Application.Interfaces.UseCases;

public interface IUpdateCategory : IRequestHandler<UpdateCategoryInput, BaseResponse<CategoryOutput>> { }
