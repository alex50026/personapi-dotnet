using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.ViewModels;


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
        public async Task<ActionResult<IEnumerable<ProfesionViewModel>>> GetAll()
        {
            var lista = await _repo.GetAllAsync();
            return Ok(lista.Select(ProfesionViewModel.FromEntity));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProfesionViewModel>> GetById(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p is null) return NotFound();
            return Ok(ProfesionViewModel.FromEntity(p));
        }

        [HttpPost]
        public async Task<ActionResult<ProfesionViewModel>> Create(ProfesionViewModel vm)
        {
            if (await _repo.ExistsAsync(vm.Id!.Value))
                return Conflict(new { message = "Ya existe una profesión con ese Id." });

            try
            {
                await _repo.AddAsync(vm.ToEntity());
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "No se pudo guardar la profesión." });
            }
            return CreatedAtAction(nameof(GetById), new { id = vm.Id }, vm);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ProfesionViewModel vm)
        {
            if (id != vm.Id)
                return BadRequest(new { message = "El Id de la ruta no coincide con el del cuerpo." });
            if (!await _repo.ExistsAsync(id))
                return NotFound();

            try
            {
                await _repo.UpdateAsync(vm.ToEntity());
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "No se pudo actualizar la profesión." });
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _repo.ExistsAsync(id))
                return NotFound();
            if (await _repo.HasEstudiosAsync(id))
                return Conflict(new { message = "No se puede eliminar: hay estudios asociados a esta profesión." });

            try
            {
                await _repo.DeleteAsync(id);
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "No se pudo eliminar la profesión." });
            }
            return NoContent();
        }
    }
}
