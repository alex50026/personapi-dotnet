using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.ViewModels;

namespace personapi_dotnet.Controllers
{
    public class EstudioController : Controller
    {
        private readonly IEstudioRepository _repo;
        private readonly IPersonaRepository _personas;
        private readonly IProfesionRepository _profesiones;

        public EstudioController(IEstudioRepository repo, IPersonaRepository personas, IProfesionRepository profesiones)
        {
            _repo = repo;
            _personas = personas;
            _profesiones = profesiones;
        }

        public async Task<IActionResult> Index()
        {
            var estudios = await _repo.GetAllAsync();
            return View(estudios.Select(EstudioViewModel.FromEntity));
        }

        public async Task<IActionResult> Details(long id, int idprof)
        {
            var estudio = await _repo.GetByPersonProfessionAsync(id, idprof);
            if (estudio == null) return NotFound();
            return View(EstudioViewModel.FromEntity(estudio));
        }

        public IActionResult Create()
        {
            return View(new EstudioViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EstudioViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (!await _personas.ExistsAsync(vm.CcPer))
                    ModelState.AddModelError(nameof(EstudioViewModel.CcPer), "No existe una persona con esa cédula.");
                if (await _profesiones.GetByIdAsync(vm.IdProf) == null)
                    ModelState.AddModelError(nameof(EstudioViewModel.IdProf), "No existe una profesión con ese id.");
                if (await _repo.ExistsAsync(vm.CcPer, vm.IdProf))
                    ModelState.AddModelError(string.Empty, "Ya existe un estudio para esa persona y esa profesión.");
            }
            if (!ModelState.IsValid) return View(vm);

            await _repo.AddAsync(vm.ToEntity());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long id, int idprof)
        {
            var estudio = await _repo.GetByPersonProfessionAsync(id, idprof);
            if (estudio == null) return NotFound();
            return View(EstudioViewModel.FromEntity(estudio));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, int idprof, EstudioViewModel vm)
        {
            if (id != vm.CcPer || idprof != vm.IdProf) return NotFound();
            if (!ModelState.IsValid) return View(vm);
            if (!await _repo.ExistsAsync(id, idprof)) return NotFound();

            await _repo.UpdateAsync(vm.ToEntity());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(long id, int idprof)
        {
            var estudio = await _repo.GetByPersonProfessionAsync(id, idprof);
            if (estudio == null) return NotFound();
            return View(EstudioViewModel.FromEntity(estudio));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id, int idprof)
        {
            await _repo.DeleteAsync(id, idprof);
            return RedirectToAction(nameof(Index));
        }
    }
}
