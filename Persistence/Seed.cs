using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence
{
  public class Seed
  {
    public static async Task SeedData(DataContext context)
    {
      if (!context.Articles.Any())
      {
        var articles = new List<Article>
        {
          new Article
          {
            Name = "Past Article 1",
            ArticleDate = DateTime.Now.AddMonths(-2),
            Description = "Article 2 months ago",
          },
          new Article
          {
            Name = "Past Article 2",
            ArticleDate = DateTime.Now.AddMonths(-1),
            Description = "Article 1 month ago"
          },
          new Article
          {
            Name = "Future Article 1",
            ArticleDate = DateTime.Now.AddMonths(1),
            Description = "Article 1 month in future"
          },
          new Article
          {
            Name = "Future Article 2",
            ArticleDate = DateTime.Now.AddMonths(2),
            Description = "Article 2 months in future"
          },
          new Article
          {
            Name = "Future Article 3",
            ArticleDate = DateTime.Now.AddMonths(3),
            Description = "Article 3 months in future"
          },
          new Article
          {
            Name = "Future Article 4",
            ArticleDate = DateTime.Now.AddMonths(4),
            Description = "Article 4 months in future"
          },
          new Article
          {
            Name = "Future Article 5",
            ArticleDate = DateTime.Now.AddMonths(5),
            Description = "Article 5 months in future"
          },
          new Article
          {
            Name = "Future Article 6",
            ArticleDate = DateTime.Now.AddMonths(6),
            Description = "Article 6 months in future"
          },
        };

        await context.Articles.AddRangeAsync(articles);
        await context.SaveChangesAsync();
      }
    }
  }
}