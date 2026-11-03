using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories
{
    public class ProfesionRepository : IProfesionRepository
    {
        private readonly persona_dbContext _context;

        public ProfesionRepository(persona_dbContext context)
        {
            _context = context;
        }

        public async Task<List<Profesion>> GetAllAsync() =>
            await _context.Profesions.AsNoTracking().OrderBy(p => p.Id).ToListAsync();

        public async Task<Profesion?> GetByIdAsync(int id) =>
            await _context.Profesions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddAsync(Profesion profesion)
        {
            _context.Profesions.Add(profesion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Profesion profesion)
        {
            _context.Profesions.Update(profesion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Profesions.FindAsync(id);
            if (entity is null) return;
            _context.Profesions.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _context.Profesions.AnyAsync(p => p.Id == id);

        public async Task<bool> HasEstudiosAsync(int id) =>
            await _context.Estudios.AnyAsync(e => e.IdProf == id);
    }
}
