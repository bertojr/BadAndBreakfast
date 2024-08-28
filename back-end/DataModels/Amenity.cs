using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace back_end.DataModels
{
	public class Amenity
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AmenityID { get; set; }

        [Required(ErrorMessage = "Il nome della comodità è obbligatorio")]
        [MaxLength(50, ErrorMessage = "Il nome della comodità non può superare i 50 " +
            "caratteri")]
        public string Name { get; set; }

        [MaxLength(200, ErrorMessage = "La descrizione non può superare i 200 " +
            "caratteri.")]
        public string? Description { get; set; }

        // propietà di navigazione
        public List<Room> Rooms { get; set; } = new List<Room>();
    }
}

