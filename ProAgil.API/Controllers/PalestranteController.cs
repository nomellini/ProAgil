using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Repository;

namespace ProAgil.API.Controllers
{

  [Route("api/[controller]")]
  [ApiController]
  public class PalestranteController : Controller
  {
    private IProAgilRepository _repo;

    public PalestranteController(IProAgilRepository repo)
    {
      this._repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      try
      {
        var results = await _repo.GetAllPalestrantes(false);
        return Ok(results);
      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
      }
    }

  }
}