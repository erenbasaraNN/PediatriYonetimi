using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PediatriYonetimi.Models;
using System.Security.Claims;

namespace PediatriYonetimi.Controllers
{
    public class OgretimUyesiController : Controller
    {
        private readonly PediatriContext _context;
        public OgretimUyesiController(PediatriContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        [Route("Musaitlik/Ekle")]
        public IActionResult MusaitlikEkle()
        {
            return View("Musaitlik/MusaitlikEkle");
        }

        [HttpPost]
        [Route("Musaitlik/Ekle")]
        public async Task<IActionResult> MusaitlikEkle(RandevuMusaitlikDurumu musaitlik)
        {
            if (ModelState.IsValid)
            {
                var ogretimUyesiId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(ogretimUyesiId))
                {
                    ModelState.AddModelError("", "Giriş yapmış öğretim üyesi bulunamadı.");
                    return View(musaitlik);
                }

                musaitlik.OgretimUyesiId = ogretimUyesiId;
                _context.RandevuMusaitlikleri.Add(musaitlik);
                await _context.SaveChangesAsync();
                return RedirectToAction("MusaitlikListesi");
            }
            else
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var value = ModelState[modelStateKey];
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }
            return View("Musaitlik/MusaitlikEkle", musaitlik);
        }

        [Route("Musaitlik/Listesi")]
        public async Task<IActionResult> MusaitlikListesi()
        {
            var ogretimUyesiId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var musaitlikler = await _context.RandevuMusaitlikleri
                                        .Where(m => m.OgretimUyesiId == ogretimUyesiId)
                                        .ToListAsync();
            return View("Musaitlik/MusaitlikListesi", musaitlikler);
        }

    }
}
