using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
  public interface IProAgilRepository
  {
    void Add<T>(T entity) where T : class;
    void Update<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    Task<bool> SaveChangeAsync();

    //Eventos
    Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes);
    Task<Evento[]> GetAllEventoAsync(bool includePalestrantes);
    Task<Evento> GetEventoById(int EventoId, bool includePalestrantes);

    //Palestrante
    Task<Palestrante> GetPalestranteAsync(int PalestranteId, bool includeEventos);
    Task<Palestrante[]> GetPalestranteByNameAsync(string name, bool includeEventos);

  }
}