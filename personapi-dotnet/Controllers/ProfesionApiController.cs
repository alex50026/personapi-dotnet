using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    [ApiController]
    [Route("api/profesion")]
    public class ProfesionApiController : ControllerBase
    {
        private readonly IProfesionRepository _repo;

        public ProfesionApiController(IProfesionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profesion>>> GetAll()
        {
            return Ok(await _repo.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Profesion>> GetById(int id)
        {
            var profesion = await _repo.GetByIdAsync(id);
            if (profesion == null) return NotFound();
            return Ok(profesion);
        }

        [HttpPost]
        public async Task<ActionResult<Profesion>> Create(Profesion profesion)
        {
            await _repo.AddAsync(profesion);
            return CreatedAtAction(nameof(GetById), new { id = profesion.Id }, profesion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Profesion profesion)
        {
            if (id != profesion.Id) return BadRequest();
            await _repo.UpdateAsync(profesion);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
