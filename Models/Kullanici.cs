using Microsoft.AspNetCore.Identity;
namespace PediatriYonetimi.Models;

public class Kullanici : IdentityUser
{
    public string Ad { get; set; }
    public string Soyad { get; set; }
    public string Telefon { get; set; }
    public string Adres { get; set; }

    // Kullanıcının Rolü (Admin, Asistan, Öğretim Üyesi)
    public string Rol { get; set; }

    // İlişkiler
    public ICollection<Nobet>? Nobetler { get; set; }
    public ICollection<Randevu>? Randevular { get; set; }
}
