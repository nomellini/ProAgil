using System.Net.Http.Headers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.API.Dtos;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.API.Controllers
{

  [Route("api/[controller]")]
  [ApiController]
  public class EventoController : Controller
  {
    private readonly IProAgilRepository _repo;
    private readonly IMapper _mapper;

    public EventoController(IProAgilRepository repo, IMapper mapper)
    {
      this._repo = repo;
      this._mapper = mapper;
    }


    [HttpPost("upload")]
    public async Task<IActionResult> upload()
    {
      try
      {
        var file = Request.Form.Files[0];
        var folderName = Path.Combine("Resources", "images");
        var PathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        if (file.Length > 0)
        {
          var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
          var fullPath = Path.Combine(PathToSave, filename.Replace("\"", " ").Trim());
          using (var stream = new FileStream(fullPath, FileMode.Create))
          {
            await file.CopyToAsync(stream);
          }
        }

        return Ok();
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {ex.Message}");
      }


    }



    [HttpGet]
    public async Task<IActionResult> Get()
    {
      try
      {
        var eventos = await _repo.GetAllEventoAsync(true);
        var results = _mapper.Map<EventoDto[]>(eventos);

        return Ok(results);
      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou ");
      }
    }


    [HttpGet("{EventoId}")]
    public async Task<IActionResult> Get(int EventoId)
    {
      try
      {
        var evento = await _repo.GetEventoById(EventoId, true);
        var results = _mapper.Map<EventoDto>(evento);
        return Ok(results);
      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
      }

    }



    [HttpGet("getByTema/{tema}")]
    public async Task<IActionResult> Get(string tema)
    {
      try
      {
        var eventos = await _repo.GetAllEventoAsyncByTema(tema, true);
        var results = _mapper.Map<EventoDto[]>(eventos);
        return Ok(results);
      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
      }

    }

    [HttpPost]
    public async Task<IActionResult> Post(EventoDto model)
    {
      try
      {

        var evento = _mapper.Map<Evento>(model);

        _repo.Add(evento);

        if (await _repo.SaveChangeAsync())
        {
          return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
        }
      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
      }

      return BadRequest();

    }



    [HttpPut("{EventoId}")]
    public async Task<IActionResult> Put(int EventoId, EventoDto model)
    {
      try
      {

        var Evento = await _repo.GetEventoById(EventoId, false);
        if (Evento == null)
          return NotFound();

        _mapper.Map(model, Evento);

        _repo.Update(Evento);

        if (await _repo.SaveChangeAsync())
        {
          return Created($"/api/evento/{model.Id}", model);
        }
      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
      }

      return BadRequest();

    }

    [HttpDelete("{EventoId}")]
    public async Task<IActionResult> Delete(int EventoId)
    {
      try
      {

        var Evento = await _repo.GetEventoById(EventoId, false);
        if (Evento == null)
          return NotFound();

        _repo.Delete(Evento);

        if (await _repo.SaveChangeAsync())
        {
          return Ok();
        }
      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
      }

      return BadRequest();

    }


  }
}