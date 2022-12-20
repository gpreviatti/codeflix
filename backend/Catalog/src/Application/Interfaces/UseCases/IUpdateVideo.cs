using Application.Dtos.Video;
using MediatR;

namespace Application.Interfaces.UseCases;
public interface IUpdateVideo : IRequestHandler<UpdateVideoInput, VideoOutput>{ }
