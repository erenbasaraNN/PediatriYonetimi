﻿namespace PediatriYonetimi.Models;

public class Duyuru
{
    public int Id { get; set; }
    public string Baslik { get; set; }
    public string Icerik { get; set; }
    public DateTime YayinTarihi { get; set; }
}
