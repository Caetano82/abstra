using Abstra.Domain.DTOs;

namespace Abstra.Domain.Contracts;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}
