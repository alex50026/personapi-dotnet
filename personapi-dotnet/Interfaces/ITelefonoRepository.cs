using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Interfaces
{
    public interface ITelefonoRepository
    {
        Task<List<Telefono>> GetAllAsync();
        Task<Telefono?> GetByIdAsync(string num);
        Task AddAsync(Telefono telefono);
        Task UpdateAsync(Telefono telefono);
        Task DeleteAsync(string num);
        Task<bool> ExistsAsync(string num);

        Task<List<Persona>> GetPersonasAsync();
        Task<bool> PersonaExistsAsync(long cc);
    }
}
