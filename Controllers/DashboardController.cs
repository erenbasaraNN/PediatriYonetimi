﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PediatriYonetimi.Models;
using System.Security.Claims;

namespace PediatriYonetimi.Controllers
{
    [Authorize]
    [Route("Dashboard")]
    public class DashboardController : Controller
    {
        private readonly PediatriContext _context;

        public DashboardController(PediatriContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Main Entry: Redirects based on user role
        [Route("")]
        public IActionResult Index()
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userRole == "Asistan")
                return RedirectToAction("AsistanDashboard");
            else if (userRole == "OgretimUyesi")
                return RedirectToAction("OgretimUyesiDashboard");

            return Unauthorized();
        }

        // Dashboard for Asistan
        [Route("Asistan")]
        public async Task<IActionResult> AsistanDashboard()
        {
            var asistanId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch Randevular for the Asistan
            var randevular = await _context.Randevular
                .Include(r => r.RandevuMusaitlikDurumu)
                .Where(r => r.AsistanId == asistanId)
                .OrderBy(r => r.RandevuMusaitlikDurumu.BaslangicSaati)
                .ToListAsync();

            // Fetch Nobetler for the Asistan
            var nobetler = await _context.Nobetler
                .Include(n => n.Bolum)
                .Where(n => n.KullaniciId == asistanId)
                .OrderBy(n => n.NobetTarihi)
                .ToListAsync();

            // Fetch recent Duyurular
            var duyurular = await _context.Duyurular
                .OrderByDescending(d => d.YayinTarihi)
                .Take(5)
                .ToListAsync();

            // Fetch recent Acil Durumlar
            var acilDurumlar = await _context.AcilDurumlar
                .OrderByDescending(a => a.Tarih)
                .Take(5)
                .ToListAsync();

            ViewBag.Randevular = randevular;
            ViewBag.Nobetler = nobetler;
            ViewBag.Duyurular = duyurular;
            ViewBag.AcilDurumlar = acilDurumlar;

            return View();
        }

        // Dashboard for Öğretim Üyesi
        [Route("OgretimUyesi")]
        public async Task<IActionResult> OgretimUyesiDashboard()
        {
            var ogretimUyesiId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch RandevuMusaitlikDurumu created by this OgretimUyesi
            var musaitlikler = await _context.RandevuMusaitlikleri
                .Where(m => m.OgretimUyesiId == ogretimUyesiId)
                .OrderBy(m => m.BaslangicSaati)
                .ToListAsync();

            // Fetch Randevular associated with this OgretimUyesi
            var randevular = await _context.Randevular
                .Include(r => r.RandevuMusaitlikDurumu)
                .Where(r => r.RandevuMusaitlikDurumu.OgretimUyesiId == ogretimUyesiId)
                .OrderBy(r => r.RandevuMusaitlikDurumu.BaslangicSaati)
                .ToListAsync();

            // Fetch recent Duyurular
            var duyurular = await _context.Duyurular
                .OrderByDescending(d => d.YayinTarihi)
                .Take(5)
                .ToListAsync();

            // Fetch recent Acil Durumlar
            var acilDurumlar = await _context.AcilDurumlar
                .OrderByDescending(a => a.Tarih)
                .Take(5)
                .ToListAsync();

            ViewBag.Musaitlikler = musaitlikler;
            ViewBag.Randevular = randevular;
            ViewBag.Duyurular = duyurular;
            ViewBag.AcilDurumlar = acilDurumlar;

            return View();
        }

        [Route("AsistanCalendarData")]
        public async Task<IActionResult> AsistanCalendarData()
        {
            var asistanId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var randevular = await _context.Randevular
                .Include(r => r.RandevuMusaitlikDurumu)
                .Where(r => r.AsistanId == asistanId)
                .Select(r => new
                {
                    title = "Randevu",
                    start = r.RandevuMusaitlikDurumu.BaslangicSaati,
                    end = r.RandevuMusaitlikDurumu.BitisSaati,
                    color = "#007bff"
                })
                .ToListAsync();

            var nobetler = await _context.Nobetler
                .Include(n => n.Bolum)
                .Where(n => n.KullaniciId == asistanId)
                .Select(n => new
                {
                    title = "Nöbet: " + n.Bolum.BolumAdi,
                    start = n.NobetTarihi,
                    end = n.NobetTarihi.AddDays(1),
                    color = "#28a745"
                })
                .ToListAsync();

            var events = randevular.Concat(nobetler);
            return Json(events);
        }

        // Calendar Data for Öğretim Üyesi
        [Route("OgretimUyesiCalendarData")]
        public async Task<IActionResult> OgretimUyesiCalendarData()
        {
            var ogretimUyesiId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var randevular = await _context.Randevular
                .Include(r => r.RandevuMusaitlikDurumu)
                .Where(r => r.RandevuMusaitlikDurumu.OgretimUyesiId == ogretimUyesiId)
                .Select(r => new
                {
                    title = "Randevu",
                    start = r.RandevuMusaitlikDurumu.BaslangicSaati,
                    end = r.RandevuMusaitlikDurumu.BitisSaati,
                    color = "#007bff"
                })
                .ToListAsync();

            var musaitlikler = await _context.RandevuMusaitlikleri
                .Where(m => m.OgretimUyesiId == ogretimUyesiId)
                .Select(m => new
                {
                    title = "Müsaitlik: " + m.BaslangicSaati.ToShortTimeString(),
                    start = m.BaslangicSaati,
                    end = m.BitisSaati,
                    color = "#ffc107"
                })
                .ToListAsync();

            var events = randevular.Concat(musaitlikler);
            return Json(events);
        }

    }
}