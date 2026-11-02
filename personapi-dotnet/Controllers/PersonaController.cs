using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.ViewModels;

namespace personapi_dotnet.Controllers
{
    public class PersonaController : Controller
    {
        private readonly IPersonaRepository _repo;

        public PersonaController(IPersonaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var personas = await _repo.GetAllAsync();
            return View(personas.Select(PersonaViewModel.FromEntity));
        }

        public async Task<IActionResult> Details(long id)
        {
            var persona = await _repo.GetByIdAsync(id);
            if (persona == null) return NotFound();
            return View(PersonaViewModel.FromEntity(persona));
        }

        public IActionResult Create()
        {
            return View(new PersonaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonaViewModel vm)
        {
            if (await _repo.ExistsAsync(vm.Cc))
                ModelState.AddModelError(nameof(PersonaViewModel.Cc), "Ya existe una persona con esa cédula.");
            if (!ModelState.IsValid) return View(vm);
            await _repo.AddAsync(vm.ToEntity());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long id)
        {
            var persona = await _repo.GetByIdAsync(id);
            if (persona == null) return NotFound();
            return View(PersonaViewModel.FromEntity(persona));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, PersonaViewModel vm)
        {
            if (id != vm.Cc) return NotFound();
            if (!ModelState.IsValid) return View(vm);
            if (!await _repo.ExistsAsync(id)) return NotFound();
            await _repo.UpdateAsync(vm.ToEntity());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(long id)
        {
            var persona = await _repo.GetByIdAsync(id);
            if (persona == null) return NotFound();
            return View(PersonaViewModel.FromEntity(persona));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
