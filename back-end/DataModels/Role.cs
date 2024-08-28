using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace back_end.DataModels
{
	public class Role
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int RoleID { get; set; }

		[Required(ErrorMessage = "Il nome del ruolo è obbligatorio")]
		[MaxLength(50, ErrorMessage = "Il nome del ruolo non può superare i 50 " +
			"caratteri")]
		public string Name { get; set; }

        [MaxLength(200, ErrorMessage = "La descrizione non può superare i 200 " +
			"caratteri.")]
        public string? Description { get; set; }

		// propietà di navigazione
		public List<User> Users { get; set; } = new List<User>();
    }
}

