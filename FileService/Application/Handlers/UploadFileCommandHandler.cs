using FileService.Application.Commands;
using FileService.Infrastructure.Abstractions;
using MediatR;

namespace FileService.Application.Handlers;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, string>
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly ILogger<UploadFileCommandHandler> _logger;

    public UploadFileCommandHandler(IBlobStorageService blobStorageService, ILogger<UploadFileCommandHandler> logger)
    {
        _blobStorageService = blobStorageService;
        _logger = logger;
    }

    public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var url = await _blobStorageService.UploadAsync(request.File);
        _logger.LogInformation("File uploaded successfully. URL: {Url}", url);
        return url;
    }
}
