using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PediatriYonetimi.Models;

namespace PediatriYonetimi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly PediatriContext _context;

        public AdminDashboardController(PediatriContext context)
        {
            _context = context;
        }

        [Route("")]
        public IActionResult Index()
        {
            ViewBag.AcilDuyuruCount = _context.AcilDurumlar.Count();
            ViewBag.BolumCount = _context.Bolumler.Count();
            ViewBag.DuyuruCount = _context.Duyurular.Count();
            ViewBag.KullaniciCount = _context.Users.Count();
            ViewBag.NobetCount = _context.Nobetler.Count();

            return View();
        }
    }
}
