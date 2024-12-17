
namespace PediatriYonetimi.Models;

public class Bolum
{
    public int Id { get; set; }
    public string BolumAdi { get; set; }
    public int HastaSayisi { get; set; }
    public int BosYatakSayisi { get; set; }
    public string Aciklama { get; set; }

    // İlişkiler
    public ICollection<Nobet>? Nobetler { get; set; }
}
