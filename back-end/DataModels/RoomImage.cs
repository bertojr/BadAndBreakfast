using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace back_end.DataModels
{
	public class RoomImage
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ImageID { get; set; }

		[Required(ErrorMessage = "Il percorso dell'immagine è obbligatorio")]
		[Url(ErrorMessage = "L'url dell'immagine non è valido")]
		public string Url { get; set; }

		public string? AltText { get; set; }

		[ForeignKey("RoomID")]
        [JsonIgnore]
        public Room Room { get; set; }
    }
}

