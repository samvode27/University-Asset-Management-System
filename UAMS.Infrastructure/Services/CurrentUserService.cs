using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UAMS.Application.Interfaces.Services;

namespace UAMS.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor =
            httpContextAccessor
            ?? throw new ArgumentNullException(
                nameof(httpContextAccessor));
    }


    public Guid? UserId
    {
        get
        {
            var value = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(value, out var id)
                ? id
                : null;
        }
    }


    public string? Username =>
        _httpContextAccessor
            .HttpContext?
            .User?
            .FindFirstValue(ClaimTypes.Name);


    public bool IsAuthenticated =>
        _httpContextAccessor
            .HttpContext?
            .User?
            .Identity?
            .IsAuthenticated == true;
}