using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PediatriYonetimi.Models;

namespace PediatriYonetimi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly PediatriContext _context;

        public AdminController(PediatriContext context)
        {
            _context = context;
        }

        // Ana sayfa
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Kullanici/Listesi")]
        public async Task<IActionResult> KullaniciListesi()
        {
            var kullanicilar = await _context.Users.ToListAsync();
            return View("Kullanici/KullaniciListesi", kullanicilar);
        }

        [Route("Kullanici/Ekle")]
        public IActionResult KullaniciEkle()
        {
            ViewBag.Roller = new List<string> { "Admin", "Asistan", "OgretimUyesi" };
            return View("Kullanici/KullaniciEkle");
        }

        [HttpPost]
        [Route("Kullanici/Ekle")]
        public async Task<IActionResult> KullaniciEkle(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                kullanici.UserName = kullanici.Email; // Identity için gerekli
                var userManager = HttpContext.RequestServices.GetService<UserManager<Kullanici>>();
                var result = await userManager.CreateAsync(kullanici, "Default123!"); // Default þifre
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(kullanici, kullanici.Rol);
                    return RedirectToAction("KullaniciListesi");
                }
                ModelState.AddModelError("", "Kullanýcý eklenirken hata oluþtu.");
            }
            ViewBag.Roller = new List<string> { "Admin", "Asistan", "OgretimUyesi" };
            return View("Kullanici/KullaniciEkle", kullanici);
        }

        [Route("Kullanici/Duzenle/{id}")]
        public async Task<IActionResult> KullaniciDuzenle(string id)
        {
            var kullanici = await _context.Users.FindAsync(id);
            if (kullanici == null) return NotFound();

            ViewBag.Roller = new List<string> { "Admin", "Asistan", "OgretimUyesi" };
            return View("Kullanici/KullaniciDuzenle", kullanici);
        }

        [HttpPost]
        [Route("Kullanici/Duzenle/{id}")]
        public async Task<IActionResult> KullaniciDuzenle(string id, Kullanici kullanici)
        {
            if (id != kullanici.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var userManager = HttpContext.RequestServices.GetService<UserManager<Kullanici>>();
                var existingUser = await userManager.FindByIdAsync(id);

                if (existingUser != null)
                {
                    existingUser.Ad = kullanici.Ad;
                    existingUser.Soyad = kullanici.Soyad;
                    existingUser.Telefon = kullanici.Telefon;
                    existingUser.Adres = kullanici.Adres;
                    existingUser.Rol = kullanici.Rol;

                    var result = await userManager.UpdateAsync(existingUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("KullaniciListesi");
                    }
                    ModelState.AddModelError("", "Kullanýcý güncellenirken hata oluþtu.");
                }
            }

            ViewBag.Roller = new List<string> { "Admin", "Asistan", "OgretimUyesi" };
            return View("Kullanici/KullaniciDuzenle", kullanici);
        }

        [Route("Kullanici/Sil/{id}")]
        public async Task<IActionResult> KullaniciSil(string id)
        {
            var userManager = HttpContext.RequestServices.GetService<UserManager<Kullanici>>();
            var kullanici = await userManager.FindByIdAsync(id);
            if (kullanici != null)
            {
                await userManager.DeleteAsync(kullanici);
            }
            return RedirectToAction("KullaniciListesi");
        }


        // Bölüm Listesi
        [Route("Bolum/Listesi")]
        public async Task<IActionResult> BolumListesi()
        {
            var bolumler = await _context.Bolumler.ToListAsync();
            return View("Bolum/BolumListesi", bolumler);
        }

        // Bölüm Ekle
        [Route("Bolum/Ekle")]
        public IActionResult BolumEkle()
        {
            return View("Bolum/BolumEkle");
        }

        [HttpPost]
        [Route("Bolum/Ekle")]
        public async Task<IActionResult> BolumEkle(Bolum bolum)
        {
            if (ModelState.IsValid)
            {
                _context.Bolumler.Add(bolum);
                await _context.SaveChangesAsync();
                return RedirectToAction("BolumListesi");
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

            return View("Bolum/BolumEkle", bolum);
        }

        // Bölüm Düzenle
        [Route("Bolum/Duzenle/{id}")]
        public async Task<IActionResult> BolumDuzenle(int id)
        {
            var bolum = await _context.Bolumler.FindAsync(id);
            if (bolum == null) return NotFound();
            return View("Bolum/BolumDuzenle", bolum);
        }

        [HttpPost]
        [Route("Bolum/Duzenle/{id}")]
        public async Task<IActionResult> BolumDuzenle(int id, Bolum bolum)
        {
            if (id != bolum.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(bolum);
                await _context.SaveChangesAsync();
                return RedirectToAction("BolumListesi");
            }
            return View("Bolum/BolumDuzenle", bolum);
        }

        [Route("Nobet/Listesi")]
        public async Task<IActionResult> NobetListesi()
        {
            var nobetler = await _context.Nobetler
                                 .Include(n => n.Asistan)
                                 .Include(n => n.Bolum)
                                 .ToListAsync();
            return View("Nobet/NobetListesi", nobetler);
        }

        [Route("Nobet/Ekle")]
        public IActionResult NobetEkle()
        {
            ViewBag.Bolumler = _context.Bolumler.ToList(); // Bölümleri yükle
            ViewBag.Asistanlar = _context.Users.Where(u => u.Rol == "Asistan").ToList(); // Asistanlarý yükle
            return View("Nobet/NobetEkle");
        }


        [HttpPost]
        [Route("Nobet/Ekle")]
        public async Task<IActionResult> NobetEkle(Nobet nobet)
        {
            if (ModelState.IsValid)
            {
                _context.Nobetler.Add(nobet);
                await _context.SaveChangesAsync();
                return RedirectToAction("NobetListesi");
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
            return View("Nobet/NobetEkle", nobet);
        }

        [Route("Nobet/Duzenle/{id}")]
        public async Task<IActionResult> NobetDuzenle(int id)
        {
            var nobet = await _context.Nobetler.FindAsync(id);
            if (nobet == null) return NotFound();

            ViewBag.Bolumler = _context.Bolumler.ToList();
            ViewBag.Asistanlar = _context.Users.Where(u => u.Rol == "Asistan").ToList();
            return View("Nobet/NobetDuzenle", nobet);
        }

        [HttpPost]
        [Route("Nobet/Duzenle/{id}")]
        public async Task<IActionResult> NobetDuzenle(int id, Nobet nobet)
        {
            if (id != nobet.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(nobet);
                await _context.SaveChangesAsync();
                return RedirectToAction("NobetListesi");
            }
            return View("Nobet/NobetDuzenle", nobet);
        }

        [Route("Nobet/Sil/{id}")]
        public async Task<IActionResult> NobetSil(int id)
        {
            var nobet = await _context.Nobetler.FindAsync(id);
            if (nobet == null) return NotFound();

            _context.Nobetler.Remove(nobet);
            await _context.SaveChangesAsync();
            return RedirectToAction("NobetListesi");
        }

        // Duyuru Listesi
        [Route("Duyuru/Listesi")]
        public async Task<IActionResult> DuyuruListesi()
        {
            var duyurular = await _context.Duyurular.ToListAsync();
            return View("Duyuru/DuyuruListesi", duyurular);
        }

        [Route("Duyuru/Ekle")]
        public IActionResult DuyuruEkle()
        {
            return View("Duyuru/DuyuruEkle");
        }

        [HttpPost]
        [Route("Duyuru/Ekle")]
        public async Task<IActionResult> DuyuruEkle(Duyuru duyuru)
        {
            if (ModelState.IsValid)
            {
                duyuru.YayinTarihi = DateTime.Now; // Duyuru tarihi otomatik atanýr
                _context.Duyurular.Add(duyuru);
                await _context.SaveChangesAsync();
                return RedirectToAction("DuyuruListesi");
            }
            return View("Duyuru/DuyuruEkle", duyuru);
        }

        [Route("Duyuru/Sil/{id}")]
        public async Task<IActionResult> DuyuruSil(int id)
        {
            var duyuru = await _context.Duyurular.FindAsync(id);
            if (duyuru != null)
            {
                _context.Duyurular.Remove(duyuru);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("DuyuruListesi");
        }
        [Route("AcilDuyuru/Listesi")]
        public async Task<IActionResult> AcilDuyuruListesi()
        {
            var duyurular = await _context.AcilDurumlar.OrderByDescending(d => d.Tarih).ToListAsync();
            return View("AcilDuyuru/AcilDuyuruListesi", duyurular);
        }

        [Route("AcilDuyuru/Ekle")]
        public IActionResult AcilDuyuruEkle()
        {
            return View("AcilDuyuru/AcilDuyuruEkle");
        }

        [HttpPost]
        [Route("AcilDuyuru/Ekle")]
        public async Task<IActionResult> AcilDuyuruEkle(Acildurum duyuru)
        {
            if (ModelState.IsValid)
            {
                duyuru.Tarih = DateTime.Now; // Duyuru tarihi otomatik atanýr
                _context.AcilDurumlar.Add(duyuru);
                await _context.SaveChangesAsync();
                return RedirectToAction("AcilDuyuruListesi");
            }
            return View("AcilDuyuru/AcilDuyuruEkle", duyuru);
        }

        [Route("AcilDuyuru/Sil/{id}")]
        public async Task<IActionResult> AcilDuyuruSil(int id)
        {
            var duyuru = await _context.AcilDurumlar.FindAsync(id);
            if (duyuru != null)
            {
                _context.AcilDurumlar.Remove(duyuru);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("AcilDuyuruListesi");
        }

    }
}
