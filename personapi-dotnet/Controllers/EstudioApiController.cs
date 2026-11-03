using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.ViewModels;

namespace personapi_dotnet.Controllers
{
    [ApiController]
    [Route("api/estudio")]
    public class EstudioApiController : ControllerBase
    {
        private readonly IEstudioRepository _repo;
        private readonly IPersonaRepository _personas;
        private readonly IProfesionRepository _profesiones;

        public EstudioApiController(IEstudioRepository repo, IPersonaRepository personas, IProfesionRepository profesiones)
        {
            _repo = repo;
            _personas = personas;
            _profesiones = profesiones;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstudioViewModel>>> GetAll()
        {
            var estudios = await _repo.GetAllAsync();
            return Ok(estudios.Select(EstudioViewModel.FromEntity));
        }

        [HttpGet("{ccPer:long}/{idProf:int}")]
        public async Task<ActionResult<EstudioViewModel>> GetById(long ccPer, int idProf)
        {
            var estudio = await _repo.GetByPersonProfessionAsync(ccPer, idProf);
            if (estudio == null) return NotFound();
            return Ok(EstudioViewModel.FromEntity(estudio));
        }

        [HttpPost]
        public async Task<ActionResult<EstudioViewModel>> Create(EstudioViewModel vm)
        {
            if (await _repo.ExistsAsync(vm.CcPer, vm.IdProf))
                return Conflict($"Ya existe un estudio para la persona {vm.CcPer} y la profesión {vm.IdProf}.");

            if (!await _personas.ExistsAsync(vm.CcPer))
                ModelState.AddModelError(nameof(EstudioViewModel.CcPer), "No existe una persona con esa cédula.");
            if (await _profesiones.GetByIdAsync(vm.IdProf) == null)
                ModelState.AddModelError(nameof(EstudioViewModel.IdProf), "No existe una profesión con ese id.");
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            await _repo.AddAsync(vm.ToEntity());
            return CreatedAtAction(nameof(GetById), new { ccPer = vm.CcPer, idProf = vm.IdProf }, vm);
        }

        [HttpPut("{ccPer:long}/{idProf:int}")]
        public async Task<IActionResult> Update(long ccPer, int idProf, EstudioViewModel vm)
        {
            if (ccPer != vm.CcPer || idProf != vm.IdProf)
                return BadRequest("La llave de la ruta no coincide con la del cuerpo.");
            if (!await _repo.ExistsAsync(ccPer, idProf)) return NotFound();
            await _repo.UpdateAsync(vm.ToEntity());
            return NoContent();
        }

        [HttpDelete("{ccPer:long}/{idProf:int}")]
        public async Task<IActionResult> Delete(long ccPer, int idProf)
        {
            if (!await _repo.ExistsAsync(ccPer, idProf)) return NotFound();
            await _repo.DeleteAsync(ccPer, idProf);
            return NoContent();
        }
    }
}
