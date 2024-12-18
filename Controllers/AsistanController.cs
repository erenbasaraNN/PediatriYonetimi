using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PediatriYonetimi.Models;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace PediatriYonetimi.Controllers
{
    [Authorize]
    public class AsistanController : Controller
    {
        private readonly PediatriContext _context;
        private readonly ILogger<AsistanController> _logger;

        public AsistanController(PediatriContext context, ILogger<AsistanController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        [Route("Randevu/MusaitlikListesi")]
        public async Task<IActionResult> MusaitlikListesi()
        {
            try
            {
                var musaitlikler = await _context.RandevuMusaitlikleri
                                                 .Include(m => m.OgretimUyesi)
                                                 .ToListAsync();
                return View("Randevu/MusaitlikListesi", musaitlikler);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Müsaitlik Listesi");
                return View("Error");
            }
        }

        [Route("Randevu/Al/{id}")]
        public async Task<IActionResult> RandevuAl(int id)
        {
            try
            {
                var musaitlik = await _context.RandevuMusaitlikleri
                                              .FirstOrDefaultAsync(m => m.Id == id);

                if (musaitlik == null)
                {
                    TempData["ErrorMessage"] = "Müsaitlik bulunamadı.";
                    return RedirectToAction("MusaitlikListesi");
                }

                var randevu = new Randevu
                {
                    RandevuMusaitlikDurumuId = id,
                    AsistanId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                ViewBag.MusaitlikId = id;
                return View("Randevu/RandevuAl", randevu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching RandevuAl page");
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Randevu/Al/{id}")]
        public async Task<IActionResult> RandevuAl(int id, Randevu randevu)
        {
            try
            {
                randevu.AsistanId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                randevu.RandevuMusaitlikDurumuId = id;
                randevu.Id = 0;
                _context.Randevular.Add(randevu);

                // Log entity state
                _logger.LogInformation($"Entity State: Id={randevu.Id}, AsistanId={randevu.AsistanId}, MusaitlikId={randevu.RandevuMusaitlikDurumuId}");

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Randevu başarıyla alındı.";
                return RedirectToAction("RandevuListesi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Randevu");
                TempData["ErrorMessage"] = "Randevu kaydedilemedi.";
                return View("Randevu/RandevuAl", randevu);
            }
        }



        [Route("Randevu/Listesi")]
        public async Task<IActionResult> RandevuListesi()
        {
            try
            {
                var asistanId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var randevular = await _context.Randevular
                    .Include(r => r.RandevuMusaitlikDurumu)
                    .ThenInclude(m => m.OgretimUyesi) // Include OgretimUyesi
                    .Where(r => r.AsistanId == asistanId)
                    .ToListAsync();

                return View("Randevu/RandevuListesi", randevular);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Randevu Listesi");
                return View("Error");
            }
        }
    }
}
