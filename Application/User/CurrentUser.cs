using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User
{
  public class CurrentUser
  {
    public class Query : IRequest<User> { }

    public class Handler : IRequestHandler<Query, User>
    {
      private readonly UserManager<AppUser> _userManager;
      private readonly IJwtGenerator _jwtGenerator;
      private readonly ICurrentUserService _currentUserService;

      public Handler(
        UserManager<AppUser> userManager,
        IJwtGenerator jwtGenerator,
        ICurrentUserService currentUserService)
      {
        _currentUserService = currentUserService;
        _jwtGenerator = jwtGenerator;
        _userManager = userManager;
      }

      public async Task<User> Handle(Query request, CancellationToken cancellationToken)
      {
        var user = await _userManager.FindByNameAsync(_currentUserService.GetCurrentUsername());
        return new User
        {
          DisplayName = user.DisplayName,
          Username = user.UserName,
          Token = _jwtGenerator.CreateToken(user),
          Image = null
        };
      }
    }
  }
}