using MediatR;
namespace FileService.Application.Commands;

public record DeleteFileCommand(string FileUrl) : IRequest<Unit>;
