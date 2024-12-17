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
