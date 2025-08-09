using FileService.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IMediator _mediator;

    public FileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] UploadFileCommand command)
    {
        if (command.File == null)
            return BadRequest("File is null");

        var url = await _mediator.Send(command);
        return Ok(url);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFile([FromQuery] string fileUrl)
    {
        await _mediator.Send(new DeleteFileCommand(fileUrl));
        return NoContent();
    }
}
