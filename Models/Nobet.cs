namespace PediatriYonetimi.Models;
public class Nobet
{
    public int Id { get; set; }
    public DateTime NobetTarihi { get; set; }
    public string KullaniciId { get; set; } // Asistan ID
    public int BolumId { get; set; } // Nöbetin tutulduğu bölüm

    // İlişkiler
    public Kullanici? Asistan { get; set; }
    public Bolum? Bolum { get; set; }
}
