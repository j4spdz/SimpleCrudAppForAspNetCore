using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Articles
{
  public class List
  {
    public class Query : IRequest<List<Article>> { }

    public class Handler : IRequestHandler<Query, List<Article>>
    {
      private readonly DataContext _context;
      public Handler(DataContext context)
      {
        _context = context;
      }

      public async Task<List<Article>> Handle(Query request, CancellationToken cancellationToken)
      {
        return await _context.Articles
          .OrderBy(x => x.ArticleDate)
          .ToListAsync();
      }
    }
  }
}