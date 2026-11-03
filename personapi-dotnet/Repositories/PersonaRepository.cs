using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly persona_dbContext _context;

        public PersonaRepository(persona_dbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Persona>> GetAllAsync()
        {
            return await _context.Personas.AsNoTracking().ToListAsync();
        }

        public async Task<Persona?> GetByIdAsync(long cc)
        {
            return await _context.Personas
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Cc == cc);
        }

        public async Task<bool> ExistsAsync(long cc)
        {
            return await _context.Personas.AnyAsync(p => p.Cc == cc);
        }

        public async Task AddAsync(Persona persona)
        {
            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Persona persona)
        {
            _context.Personas.Update(persona);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(long cc)
        {
            var persona = await _context.Personas.FindAsync(cc);
            if (persona == null) return;

            var telefonos = await _context.Telefonos.Where(t => t.Duenio == cc).ToListAsync();
            var estudios = await _context.Estudios.Where(e => e.CcPer == cc).ToListAsync();

            _context.Telefonos.RemoveRange(telefonos);
            _context.Estudios.RemoveRange(estudios);
            _context.Personas.Remove(persona);
            await _context.SaveChangesAsync();
        }
    }
}
