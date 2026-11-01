using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    public class EstudioController : Controller
    {
        private readonly IEstudioRepository _repo;

        public EstudioController(IEstudioRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllAsync());
        }

        public async Task<IActionResult> Details(int id, int idprof)
        {
            var estudio = await _repo.GetByPersonProfessionAsync(id,idprof);
            if (estudio == null) return NotFound();
            return View(estudio);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Estudio estudio)
        {
            ModelState.Remove(nameof(Estudio.CcPerNavigation));
            ModelState.Remove(nameof(Estudio.IdProfNavigation));
            if (!ModelState.IsValid) return View(estudio);
            await _repo.AddAsync(estudio);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long id, int idprof)
        {
            var estudio = await _repo.GetByPersonProfessionAsync(id, idprof);
            if (estudio == null) return NotFound();
            return View(estudio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, int idprof, Estudio estudio)
        {
            ModelState.Remove(nameof(Estudio.CcPerNavigation));
            ModelState.Remove(nameof(Estudio.IdProfNavigation));
            if (id != estudio.CcPer || idprof != estudio.IdProf) return NotFound();
            if (!ModelState.IsValid) return View(estudio);
            await _repo.UpdateAsync(estudio);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(long id, int idprof)
        {
            var estudio = await _repo.GetByPersonProfessionAsync(id, idprof);
            if (estudio == null) return NotFound();
            return View(estudio);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Estudio estudio)
        {
            await _repo.DeleteAsync(estudio.CcPer, estudio.IdProf);
            return RedirectToAction(nameof(Index));
        }
    }
}
