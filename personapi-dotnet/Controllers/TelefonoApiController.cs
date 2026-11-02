using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    [ApiController]
    [Route("api/telefono")]
    public class TelefonoApiController : ControllerBase
    {
        private readonly ITelefonoRepository _repo;

        public TelefonoApiController(ITelefonoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Telefono>>> GetAll()
        {
            return Ok(await _repo.GetAllAsync());
        }

        [HttpGet("{num}")]
        public async Task<ActionResult<Telefono>> GetById(string num)
        {
            var telefono = await _repo.GetByIdAsync(num);
            if (telefono == null) return NotFound();
            return Ok(telefono);
        }

        [HttpPost]
        public async Task<ActionResult<Telefono>> Create(Telefono telefono)
        {
            try
            {
                await _repo.AddAsync(telefono);
            }
            catch (DbUpdateException)
            {
                return BadRequest("El número ya existe o no hay una persona con esa cédula.");
            }
            return CreatedAtAction(nameof(GetById), new { num = telefono.Num }, telefono);
        }

        [HttpPut("{num}")]
        public async Task<IActionResult> Update(string num, Telefono telefono)
        {
            if (num != telefono.Num) return BadRequest();
            try
            {
                await _repo.UpdateAsync(telefono);
            }
            catch (DbUpdateException)
            {
                return BadRequest("No hay una persona con esa cédula.");
            }
            return NoContent();
        }

        [HttpDelete("{num}")]
        public async Task<IActionResult> Delete(string num)
        {
            await _repo.DeleteAsync(num);
            return NoContent();
        }
    }
}
