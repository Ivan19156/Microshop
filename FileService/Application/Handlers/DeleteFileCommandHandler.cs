namespace FileService.Application.Handlers;

using FileService.Application.Commands;
using FileService.Infrastructure.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand,Unit>
{
    private readonly IBlobStorageService _blobStorageService;

    public DeleteFileCommandHandler(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    public async Task<Unit> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        await _blobStorageService.DeleteAsync(request.FileUrl);
        return Unit.Value;
    }
}
