using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PediatriYonetimi.Data;
using PediatriYonetimi.Models;

var builder = WebApplication.CreateBuilder(args);

// Servisleri ekleme
builder.Services.AddDbContext<PediatriContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<Kullanici>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PediatriContext>();

builder.Services.AddControllersWithViews();


var app = builder.Build();

// Seed Data (Roller ve Admin Kullan�c�)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Seed data olu�turulurken hata olu�tu: {ex.Message}");
    }
}

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Admin",
    pattern: "{controller=Admin}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
