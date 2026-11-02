using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    public class TelefonoController : Controller
    {
        private readonly ITelefonoRepository _repo;

        public TelefonoController(ITelefonoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            var telefono = await _repo.GetByIdAsync(id);
            if (telefono == null) return NotFound();
            return View(telefono);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Num,Oper,Duenio")] Telefono telefono)
        {
            if (!ModelState.IsValid) return View(telefono);
            try
            {
                await _repo.AddAsync(telefono);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "No se pudo guardar: el número ya existe o no hay una persona con esa cédula.");
                return View(telefono);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var telefono = await _repo.GetByIdAsync(id);
            if (telefono == null) return NotFound();
            return View(telefono);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Num,Oper,Duenio")] Telefono telefono)
        {
            if (id != telefono.Num) return NotFound();
            if (!ModelState.IsValid) return View(telefono);
            try
            {
                await _repo.UpdateAsync(telefono);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "No se pudo guardar: no hay una persona con esa cédula.");
                return View(telefono);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var telefono = await _repo.GetByIdAsync(id);
            if (telefono == null) return NotFound();
            return View(telefono);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
