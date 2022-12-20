﻿namespace Domain.Repository;
public interface IStorageService
{
    Task Delete(string filePath, CancellationToken cancellationToken);
    Task<string> Upload(
        string fileName,
        Stream fileStream,
        CancellationToken cancellationToken
    );
}
