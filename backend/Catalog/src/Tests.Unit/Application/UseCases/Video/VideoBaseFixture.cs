using Domain.Repository;

namespace Tests.Unit.Application.UseCases.Video;
public abstract class VideoBaseFixture
{
    protected readonly Mock<IVideoRepository> _videoRepositoryMock = new();
    protected readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    protected readonly Mock<IStorageService> _storageServiceMock = new();
}
