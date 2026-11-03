using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.ViewModels;

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
        public async Task<ActionResult<IEnumerable<TelefonoViewModel>>> GetAll()
        {
            var lista = await _repo.GetAllAsync();
            return Ok(lista.Select(TelefonoViewModel.FromEntity));
        }

        [HttpGet("{num}")]
        public async Task<ActionResult<TelefonoViewModel>> GetById(string num)
        {
            var t = await _repo.GetByIdAsync(num);
            if (t is null) return NotFound();
            return Ok(TelefonoViewModel.FromEntity(t));
        }

        [HttpPost]
        public async Task<ActionResult<TelefonoViewModel>> Create(TelefonoViewModel vm)
        {
            if (await _repo.ExistsAsync(vm.Num.Trim()))
                return Conflict(new { message = "Ya existe un teléfono con ese número." });

            if (!await _repo.PersonaExistsAsync(vm.Duenio!.Value))
            {
                ModelState.AddModelError(nameof(vm.Duenio), "La persona dueña no existe.");
                return ValidationProblem(ModelState);
            }

            try
            {
                await _repo.AddAsync(vm.ToEntity());
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "No se pudo guardar el teléfono." });
            }
            return CreatedAtAction(nameof(GetById), new { num = vm.Num.Trim() }, vm);
        }

        [HttpPut("{num}")]
        public async Task<IActionResult> Update(string num, TelefonoViewModel vm)
        {
            if (num != vm.Num)
                return BadRequest(new { message = "El número de la ruta no coincide con el del cuerpo." });
            if (!await _repo.ExistsAsync(num))
                return NotFound();

            if (!await _repo.PersonaExistsAsync(vm.Duenio!.Value))
            {
                ModelState.AddModelError(nameof(vm.Duenio), "La persona dueña no existe.");
                return ValidationProblem(ModelState);
            }

            try
            {
                await _repo.UpdateAsync(vm.ToEntity());
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "No se pudo actualizar el teléfono." });
            }
            return NoContent();
        }

        [HttpDelete("{num}")]
        public async Task<IActionResult> Delete(string num)
        {
            if (!await _repo.ExistsAsync(num))
                return NotFound();

            try
            {
                await _repo.DeleteAsync(num);
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "No se pudo eliminar el teléfono." });
            }
            return NoContent();
        }
    }
}
