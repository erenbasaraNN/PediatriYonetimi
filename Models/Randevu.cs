namespace PediatriYonetimi.Models;

public class Randevu
{
    public int Id { get; set; }
    public DateTime Tarih { get; set; }
    public string AsistanId { get; set; } // Randevu alan asistan
    public string OgretimUyesiId { get; set; } // Randevu yapılan öğretim üyesi

    // İlişkiler
    public Kullanici Asistan { get; set; }
    public Kullanici OgretimUyesi { get; set; }
}
