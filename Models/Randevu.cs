using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PediatriYonetimi.Models
{
    public class Randevu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Identity column
        public int Id { get; set; }

        [Required]
        public int RandevuMusaitlikDurumuId { get; set; }

        [Required]
        [StringLength(450)]
        public string AsistanId { get; set; } = string.Empty;

        public RandevuMusaitlikDurumu? RandevuMusaitlikDurumu { get; set; }
        public Kullanici? Asistan { get; set; }
    }
}
