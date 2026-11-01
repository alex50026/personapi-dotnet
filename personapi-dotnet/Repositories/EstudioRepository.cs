using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories
{
    public class EstudioRepository : IEstudioRepository
    {
        private readonly persona_dbContext _context;

        public EstudioRepository(persona_dbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Estudio>> GetAllAsync()
        {
            return await _context.Estudios.AsNoTracking().ToListAsync();
        }

        public async Task<Estudio?> GetByPersonProfessionAsync(long id, int professionId)
        {
            return await _context.Estudios
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.CcPer == id && e.IdProf == professionId);
        }

        public async Task AddAsync(Estudio estudio)
        {
            _context.Estudios.Add(estudio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Estudio estudio)
        {
            _context.Estudios.Update(estudio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id, int idprof)
        {
            var estudio = await _context.Estudios
                .FirstOrDefaultAsync(e => e.CcPer == id && e.IdProf == idprof);
            if (estudio != null)
            {
                _context.Estudios.Remove(estudio);
                await _context.SaveChangesAsync();
            }
        }
    }
}