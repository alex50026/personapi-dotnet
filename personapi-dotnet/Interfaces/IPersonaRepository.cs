using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Interfaces
{
    public interface IPersonaRepository
    {
        Task<IEnumerable<Persona>> GetAllAsync();
        Task<Persona?> GetByIdAsync(long cc);
        Task<bool> ExistsAsync(long cc);
        Task AddAsync(Persona persona);
        Task UpdateAsync(Persona persona);
        /// <summary>Elimina la persona junto con sus teléfonos y estudios, en una sola transacción.</summary>
        Task DeleteAsync(long cc);
    }
}
