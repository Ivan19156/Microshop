using System.Threading.Tasks;
using Models;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task RemoveAsync(RefreshToken token);
    Task SaveChangesAsync();
}
