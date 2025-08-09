using OrchestratorService.Dtos;

namespace OrchestratorService.Clients;

public interface IAuthServiceClient
{
    Task<UserDto?> GetUserByIdAsync(Guid userId);
}
