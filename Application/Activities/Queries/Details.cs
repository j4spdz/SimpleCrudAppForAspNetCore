using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;

namespace Application.Activities.Queries
{
  public class Details
  {
    public class Query : IRequest<Activity>
    {
      public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Activity>
    {
      private readonly IApplicationDbContext _context;
      public Handler(IApplicationDbContext context)
      {
        _context = context;
      }

      public async Task<Activity> Handle(Query request, CancellationToken cancellationToken)
      {
        var activity = await _context.Activities.FindAsync(request.Id);

        if (activity == null)
          throw new RestException(HttpStatusCode.NotFound, new
          {
            activity = "Not found"
          });

        return activity;
      }
    }
  }
}