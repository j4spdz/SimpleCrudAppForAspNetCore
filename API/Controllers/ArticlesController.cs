using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Articles;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  public class ArticlesController : BaseController
  {
    [HttpGet]
    public async Task<ActionResult<List<Article>>> List()
    {
      return await Mediator.Send(new List.Query());
    }
  }
}