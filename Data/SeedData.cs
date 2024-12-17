using Microsoft.AspNetCore.Identity;
using PediatriYonetimi.Models;

namespace PediatriYonetimi.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Kullanici>>();

            // Roller tanımlanıyor
            string[] roles = { "Admin", "Asistan", "OgretimUyesi" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            //CLAUDE AI ILE OLUŞTURULMUŞTUR.
            // Asistan kullanıcıları ekleniyor
            string[] asistanEmails = {
                "ahmet.yilmaz@pediatri.com",
                "mehmet.demir@pediatri.com",
                "ayse.kaya@pediatri.com",
                "fatma.sen@pediatri.com",
                "ali.kurt@pediatri.com",
                "elif.celik@pediatri.com",
                "mustafa.aydin@pediatri.com",
                "zeynep.ozturk@pediatri.com",
                "hasan.erdogan@pediatri.com",
                "selin.arslan@pediatri.com",
                "ibrahim.yildiz@pediatri.com",
                "esra.sahin@pediatri.com",
                "emre.alan@pediatri.com",
                "derya.korkmaz@pediatri.com",
                "burak.tas@pediatri.com"
            };

             string[] asistanAd = {
                "Ahmet", "Mehmet", "Ayşe", "Fatma", "Ali",
                "Elif", "Mustafa", "Zeynep", "Hasan", "Selin",
                "İbrahim", "Esra", "Emre", "Derya", "Burak"
            };

             string[] asistanSoyad = {
                "Yılmaz", "Demir", "Kaya", "Şen", "Kurt",
                "Çelik", "Aydın", "Öztürk", "Erdoğan", "Arslan",
                "Yıldız", "Şahin", "Alan", "Korkmaz", "Taş"
            };

            for (int i = 0; i < asistanEmails.Length; i++)
            {
                if (await userManager.FindByEmailAsync(asistanEmails[i]) == null)
                {
                    var asistan = new Kullanici
                    {
                        UserName = asistanEmails[i],
                        Email = asistanEmails[i],
                        Ad = asistanAd[i],
                        Soyad = asistanSoyad[i],
                        Adres = $"{asistanAd[i]} {asistanSoyad[i]} Sokak No: {new Random().Next(1, 100)}, İstanbul",
                        Telefon = $"0{new Random().Next(500, 599)}{new Random().Next(100, 999)} {new Random().Next(10, 99)} {new Random().Next(10, 99)}",
                        Rol = "Asistan"
                    };

                    string password = "Asistan123!";
                    var result = await userManager.CreateAsync(asistan, password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(asistan, "Asistan");
                    }
                }
            }
            // Öğretim Üyesi kullanıcıları ekleniyor
            string[] ogretimUyesiEmails = {
                "prof.dr.mehmet.yilmaz@pediatri.com",
                "doc.dr.ayse.demir@pediatri.com",
                "prof.dr.ali.sen@pediatri.com"
            };

            string[] ogretimUyesiAd = {
                "Mehmet", "Ayşe", "Ali"
            };
            
            string[] ogretimUyesiSoyad = {
                "Yılmaz", "Demir", "Şen"
            };

            string[] unvanlar = {
                "Prof. Dr.", "Doç. Dr.", "Prof. Dr."
            };

            for (int i = 0; i < ogretimUyesiEmails.Length; i++)
            {
                if (await userManager.FindByEmailAsync(ogretimUyesiEmails[i]) == null)
                {
                    var ogretimUyesi = new Kullanici
                    {
                        UserName = ogretimUyesiEmails[i],
                        Email = ogretimUyesiEmails[i],
                        Ad = $"{unvanlar[i]} {ogretimUyesiAd[i]}",
                        Soyad = ogretimUyesiSoyad[i],
                        Adres = $"{ogretimUyesiAd[i]} {ogretimUyesiSoyad[i]} Caddesi No: {new Random().Next(1, 50)}, Ankara",
                        Telefon = $"0{new Random().Next(500, 599)}{new Random().Next(100, 999)} {new Random().Next(10, 99)} {new Random().Next(10, 99)}",
                        Rol = "OgretimUyesi"
                    };

                    string password = "Ogretim123!";
                    var result = await userManager.CreateAsync(ogretimUyesi, password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(ogretimUyesi, "OgretimUyesi");
                    }
                }
            }
            // Admin kullanıcı ekleniyor
            string adminEmail = "admin@pediatri.com";
            string adminPassword = "Admin123!";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new Kullanici
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Ad = "Admin",
                    Soyad = "Kullanıcı",
                    Adres = "Server",
                    Telefon = "Server",
                    Rol = "Admin"
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
