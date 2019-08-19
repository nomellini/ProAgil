using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Repository.Data;

namespace ProAgil.Repository
{


  public class ProAgilRepository : IProAgilRepository
  {
    private readonly ProAgilContext _context;

    // Gerais

    public void Add<T>(T entity) where T : class
    {
      _context.Add(entity);
    }

    public void Update<T>(T entity) where T : class
    {
      _context.Update(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
      _context.Remove(entity);

    }
    public async Task<bool> SaveChangeAsync()
    {
      return (await _context.SaveChangesAsync()) > 0;
    }


    // Eventos

    public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
    {
      IQueryable<Evento> query = _context.Eventos
        .Include(c => c.Lotes)
        .Include(c => c.RedesSociais);

      if (includePalestrantes)
      {
        query = query.Include(p => p.PalestranteEventos)
            .ThenInclude(p => p.Palestrante);
      }

      query = query.OrderByDescending(c => c.DataEvento);

      return await query.ToArrayAsync();


    }

    public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes)
    {
      IQueryable<Evento> query = _context.Eventos
            .Include(c => c.Lotes)
            .Include(c => c.RedesSociais);

      if (includePalestrantes)
      {
        query = query.Include(p => p.PalestranteEventos)
            .ThenInclude(p => p.Palestrante);
      }

      query = query.OrderByDescending(c => c.DataEvento)
        .Where(c => c.Tema.ToLower().Contains(tema.ToLower()));

      return await query.ToArrayAsync();
    }

    public async Task<Evento> GetEventoById(int EventoId, bool includePalestrantes)
    {
      IQueryable<Evento> query = _context.Eventos
            .Include(c => c.Lotes)
            .Include(c => c.RedesSociais);

      if (includePalestrantes)
      {
        query = query
            .Include(p => p.PalestranteEventos)
            .ThenInclude(p => p.Palestrante);
      }

      query = query.OrderByDescending(c => c.DataEvento)
        .Where(c => c.Id == EventoId);

      return await query.FirstOrDefaultAsync();

    }

    public async Task<Palestrante> GetPalestranteAsync(int PalestranteId, bool includeEventos = false)
    {

      IQueryable<Palestrante> query = _context.Palestrantes
        .Include(c => c.RedesSociais);

      if (includeEventos)
      {
        query.Include(pe => pe.PalestranteEventos)
            .ThenInclude(p => p.Evento);
      }

      query = query.OrderBy(p => p.Nome)
        .Where(p => p.Id == PalestranteId);

      return await query.FirstOrDefaultAsync();

    }

    public async Task<Palestrante[]> GetPalestranteByNameAsync(string name, bool includeEventos)
    {
      IQueryable<Palestrante> query = _context.Palestrantes
        .Include(c => c.RedesSociais);

      if (includeEventos)
      {
        query
            .Include(pe => pe.PalestranteEventos)
            .ThenInclude(p => p.Evento);
      }

      query = query
        .Where(p => p.Nome.ToLower().Contains(name.ToLower()))
        .OrderBy(p => p.Nome);

      return await query.ToArrayAsync();
    }


    // Palestrantes

    public ProAgilRepository(ProAgilContext context)
    {
      this._context = context;
    }

  }
}