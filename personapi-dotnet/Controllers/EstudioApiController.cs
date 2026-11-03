using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.controllers
{
    [ApiController]
    [Route("api/estudio")]
    public class EstudioApiController : ControllerBase
    {
        private readonly IEstudioRepository _repo;

        public EstudioApiController(IEstudioRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudio>>> GetAll()
        {
            return Ok(await _repo.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Estudio>> GetByPersonProfessionAsync(int id, int idprof)
        {
            var estudio = await _repo.GetByPersonProfessionAsync(id, idprof);
            if (estudio == null) return NotFound();
            return Ok(estudio);
        }

        [HttpPost]
        public async Task<ActionResult<Estudio>> Create(Estudio estudio)
        {
            await _repo.AddAsync(estudio);
            return CreatedAtAction(nameof(GetByPersonProfessionAsync), new { id = estudio.CcPer, idprof = estudio.IdProf }, estudio);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, int idprof , Estudio estudio)
        {
            if (id != estudio.CcPer || idprof != estudio.IdProf) return BadRequest();
            await _repo.UpdateAsync(estudio );
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, int idprof)
        {
            await _repo.DeleteAsync(id, idprof);
            return NoContent();
        }
    }
}

