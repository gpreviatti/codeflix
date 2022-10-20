﻿using Application.Dtos.Genre;
using Application.Interfaces.UseCases;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.Genre;
public class DeleteGenre : IDeleteGenre
{
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGenre(
        IGenreRepository genreRepository,
        IUnitOfWork unitOfWork
    )
    {
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteGenreInput input, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.Get(input.Id, cancellationToken);

        await _genreRepository.Delete(genre, cancellationToken);
        
        await _unitOfWork.Commit(cancellationToken);
        
        return Unit.Value;
    }
}
