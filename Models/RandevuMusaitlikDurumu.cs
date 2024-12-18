namespace PediatriYonetimi.Models;

public class RandevuMusaitlikDurumu
{
    public int Id { get; set; }
    public string? OgretimUyesiId { get; set; } // Öğretim Üyesi ID
    public DateTime BaslangicSaati { get; set; } // Müsaitlik başlangıç saati
    public DateTime BitisSaati { get; set; }    // Müsaitlik bitiş saati

    // İlişkiler
    public Kullanici? OgretimUyesi { get; set; }
    public ICollection<Randevu>? Randevular { get; set; } // İlgili randevular
}
