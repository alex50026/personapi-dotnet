using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.ViewModels;

namespace personapi_dotnet.Controllers
{
        public class ProfesionController : Controller
    {
        private readonly IProfesionRepository _repo;

        public ProfesionController(IProfesionRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _repo.GetAllAsync();
            return View(lista.Select(ProfesionViewModel.FromEntity));
        }

        public async Task<IActionResult> Details(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p is null) return NotFound();
            return View(ProfesionViewModel.FromEntity(p));
        }

        public IActionResult Create() => View(new ProfesionViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProfesionViewModel vm)
        {
            if (vm.Id.HasValue && await _repo.ExistsAsync(vm.Id.Value))
                ModelState.AddModelError(nameof(vm.Id), "Ya existe una profesión con ese Id.");

            if (!ModelState.IsValid) return View(vm);

            try
            {
                await _repo.AddAsync(vm.ToEntity());
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo guardar la profesión. Intente de nuevo.");
                return View(vm);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p is null) return NotFound();
            return View(ProfesionViewModel.FromEntity(p));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProfesionViewModel vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!await _repo.ExistsAsync(id)) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            try
            {
                await _repo.UpdateAsync(vm.ToEntity());
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo actualizar la profesión. Intente de nuevo.");
                return View(vm);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p is null) return NotFound();
            return View(ProfesionViewModel.FromEntity(p));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p is null) return NotFound();

            // Política de borrado: no se elimina una profesión que tenga estudios asociados.
            if (await _repo.HasEstudiosAsync(id))
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar: hay estudios asociados a esta profesión. Elimine primero esos estudios.");
                return View("Delete", ProfesionViewModel.FromEntity(p));
            }

            try
            {
                await _repo.DeleteAsync(id);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo eliminar la profesión.");
                return View("Delete", ProfesionViewModel.FromEntity(p));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
