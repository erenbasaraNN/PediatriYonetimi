using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PediatriYonetimi.Models;

namespace PediatriYonetimi.Controllers
{
    public class CommonController : Controller
    {
        private readonly PediatriContext _context;

        public CommonController(PediatriContext context)
        {
            _context = context;
        }
        [Route("AcilDuyuruGoruntule")]
        public async Task<IActionResult> AcilDuyuruGoruntule()
        {
            var duyurular = await _context.AcilDurumlar.OrderByDescending(d => d.Tarih).ToListAsync();
            return View("AcilDuyuruGoruntule", duyurular);
        }

    }
}
