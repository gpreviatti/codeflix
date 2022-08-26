﻿using Application.Dtos.Category;
using Application.Interfaces;
using Application.Interfaces.UseCases;
using Domain.Repository;

namespace Application.UseCases.Category;

public class CreateCategory : ICreateCategory
{
	private readonly ICategoryRepository _categoryRepository;
	private readonly IUnitOfWork _unitOfWork;

	public CreateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
	{
		_categoryRepository = categoryRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<CreateCategoryOutput> Handle(
		CreateCategoryInput input, 
		CancellationToken cancellationToken
	)
	{
		var category = new Domain.Entity.Category(
            input.Name,
            input.Description,
            input.IsActive
		);

		await _categoryRepository.Insert(category, cancellationToken);

		await _unitOfWork.Commit(cancellationToken);

		return CreateCategoryOutput.FromCategory(category);
	}
}