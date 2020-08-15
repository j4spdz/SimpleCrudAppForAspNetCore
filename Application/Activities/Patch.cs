using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Domain;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Persistence;

namespace Application.Activities
{
  public class Patch
  {
    public class Command : IRequest
    {
      public Guid Id { get; set; }
      public JsonPatchDocument<Activity> PatchDoc { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
      private readonly DataContext _context;

      public Handler(DataContext context)
      {
        _context = context;
      }

      public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
      {
        var activity = await _context.Activities.FindAsync(request.Id);

        if (activity == null)
          throw new RestException(HttpStatusCode.NotFound, new
          {
            activity = "Not found"
          });

        request.PatchDoc.ApplyTo(activity, error =>
        {
          throw new RestException(HttpStatusCode.BadRequest, $"Fail to apply patch: {error.ErrorMessage}");
        });

        var success = await _context.SaveChangesAsync() > 0;

        if (success) return Unit.Value;

        throw new Exception("Problem saving changes");
      }
    }
  }
}