using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories
{
    public class TelefonoRepository : ITelefonoRepository
    {
        private readonly persona_dbContext _context;

        public TelefonoRepository(persona_dbContext context)
        {
            _context = context;
        }

        public async Task<List<Telefono>> GetAllAsync() =>
            await _context.Telefonos
                .AsNoTracking()
                .Include(t => t.DuenioNavigation)
                .OrderBy(t => t.Num)
                .ToListAsync();

        public async Task<Telefono?> GetByIdAsync(string num) =>
            await _context.Telefonos
                .AsNoTracking()
                .Include(t => t.DuenioNavigation)
                .FirstOrDefaultAsync(t => t.Num == num);

        public async Task AddAsync(Telefono telefono)
        {
            _context.Telefonos.Add(telefono);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Telefono telefono)
        {
            _context.Telefonos.Update(telefono);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string num)
        {
            var entity = await _context.Telefonos.FindAsync(num);
            if (entity is null) return;
            _context.Telefonos.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string num) =>
            await _context.Telefonos.AnyAsync(t => t.Num == num);

        public async Task<List<Persona>> GetPersonasAsync() =>
            await _context.Personas.AsNoTracking().OrderBy(p => p.Nombre).ThenBy(p => p.Apellido).ToListAsync();

        public async Task<bool> PersonaExistsAsync(long cc) =>
            await _context.Personas.AnyAsync(p => p.Cc == cc);
    }
}
