namespace PediatriYonetimi.Models;

public class RandevuMusaitlikDurumu
{
    public int Id { get; set; }
    public string OgretimUyesiId { get; set; } // Öğretim üyesi ID
    public DateTime BaslangicSaati { get; set; }
    public DateTime BitisSaati { get; set; }

    // İlişkiler
    public Kullanici OgretimUyesi { get; set; }
}
