using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.ViewModels;

namespace personapi_dotnet.Controllers
{
    [ApiController]
    [Route("api/persona")]
    public class PersonaApiController : ControllerBase
    {
        private readonly IPersonaRepository _repo;

        public PersonaApiController(IPersonaRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonaViewModel>>> GetAll()
        {
            var personas = await _repo.GetAllAsync();
            return Ok(personas.Select(PersonaViewModel.FromEntity));
        }

        [HttpGet("{cc:long}")]
        public async Task<ActionResult<PersonaViewModel>> GetById(long cc)
        {
            var persona = await _repo.GetByIdAsync(cc);
            if (persona == null) return NotFound();
            return Ok(PersonaViewModel.FromEntity(persona));
        }

        [HttpPost]
        public async Task<ActionResult<PersonaViewModel>> Create(PersonaViewModel vm)
        {
            if (await _repo.ExistsAsync(vm.Cc))
                return Conflict($"Ya existe una persona con cédula {vm.Cc}.");
            await _repo.AddAsync(vm.ToEntity());
            return CreatedAtAction(nameof(GetById), new { cc = vm.Cc }, vm);
        }

        [HttpPut("{cc:long}")]
        public async Task<IActionResult> Update(long cc, PersonaViewModel vm)
        {
            if (cc != vm.Cc) return BadRequest("La cédula de la ruta no coincide con la del cuerpo.");
            if (!await _repo.ExistsAsync(cc)) return NotFound();
            await _repo.UpdateAsync(vm.ToEntity());
            return NoContent();
        }

        [HttpDelete("{cc:long}")]
        public async Task<IActionResult> Delete(long cc)
        {
            if (!await _repo.ExistsAsync(cc)) return NotFound();
            await _repo.DeleteAsync(cc);
            return NoContent();
        }
    }
}
