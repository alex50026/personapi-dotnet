using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Interfaces
{
    public interface IEstudioRepository
    {
        Task<IEnumerable<Estudio>> GetAllAsync();
        Task<Estudio?> GetByPersonProfessionAsync(long id, int professionId);
        Task AddAsync(Estudio estudio);
        Task UpdateAsync(Estudio estudio);
        Task DeleteAsync(long id, int idprof);
    }
}
