using MediatR;
using Microsoft.AspNetCore.Http;

namespace FileService.Application.Commands;
public class UploadFileCommand : IRequest<string>
{
    public IFormFile File { get; set; } = null!;

    public UploadFileCommand() { }

    public UploadFileCommand(IFormFile file)
    {
        File = file;
    }
}
