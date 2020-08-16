using System.Linq;
using System.Security.Claims;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace API.Services
{
  public class CurrentUserService : ICurrentUserService
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUsername()
    {
      var username = _httpContextAccessor.HttpContext?.User?.Claims?
        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
      return username;
    }
  }
}