using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.ViewModels;

namespace personapi_dotnet.Controllers
{
    public class TelefonoController : Controller
    {
            private readonly ITelefonoRepository _repo;

        public TelefonoController(ITelefonoRepository repo)
        {
            _repo = repo;
        }

        private async Task LoadPersonasAsync(TelefonoViewModel vm)
        {
            var personas = await _repo.GetPersonasAsync();
            vm.Personas = personas.Select(p => new SelectListItem
            {
                Value = p.Cc.ToString(),
                Text = $"{p.Cc} - {p.Nombre} {p.Apellido}",
                Selected = vm.Duenio == p.Cc
            });
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _repo.GetAllAsync();
            return View(lista.Select(TelefonoViewModel.FromEntity));
        }

        public async Task<IActionResult> Details(string id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t is null) return NotFound();
            return View(TelefonoViewModel.FromEntity(t));
        }

        public async Task<IActionResult> Create()
        {
            var vm = new TelefonoViewModel();
            await LoadPersonasAsync(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TelefonoViewModel vm)
        {
            if (!string.IsNullOrWhiteSpace(vm.Num) && await _repo.ExistsAsync(vm.Num.Trim()))
                ModelState.AddModelError(nameof(vm.Num), "Ya existe un teléfono con ese número.");

            if (vm.Duenio.HasValue && !await _repo.PersonaExistsAsync(vm.Duenio.Value))
                ModelState.AddModelError(nameof(vm.Duenio), "La persona seleccionada no existe.");

            if (!ModelState.IsValid)
            {
                await LoadPersonasAsync(vm);
                return View(vm);
            }

            try
            {
                await _repo.AddAsync(vm.ToEntity());
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo guardar el teléfono. Intente de nuevo.");
                await LoadPersonasAsync(vm);
                return View(vm);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t is null) return NotFound();
            var vm = TelefonoViewModel.FromEntity(t);
            await LoadPersonasAsync(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, TelefonoViewModel vm)
        {
            if (id != vm.Num) return BadRequest();
            if (!await _repo.ExistsAsync(id)) return NotFound();

            if (vm.Duenio.HasValue && !await _repo.PersonaExistsAsync(vm.Duenio.Value))
                ModelState.AddModelError(nameof(vm.Duenio), "La persona seleccionada no existe.");

            if (!ModelState.IsValid)
            {
                await LoadPersonasAsync(vm);
                return View(vm);
            }

            try
            {
                await _repo.UpdateAsync(vm.ToEntity());
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo actualizar el teléfono. Intente de nuevo.");
                await LoadPersonasAsync(vm);
                return View(vm);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t is null) return NotFound();
            return View(TelefonoViewModel.FromEntity(t));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t is null) return NotFound();

            try
            {
                await _repo.DeleteAsync(id);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo eliminar el teléfono.");
                return View("Delete", TelefonoViewModel.FromEntity(t));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
