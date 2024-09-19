﻿using System;
using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
	public class RoomRequest
	{
        [Required(ErrorMessage = "Il numero di stanza è obbligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Il numero della stanza deve " +
            "essere un numero positivo")]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Il tipo di stanza è obbligatorio")]
        [MaxLength(50, ErrorMessage = "Il tipo di stanza non può superare i " +
            "50 caratteri")]
        public string RoomType { get; set; }

        [Required(ErrorMessage = "La capacità è obbligatoria")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Il prezzo è obbligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "Il prezzo deve essere un " +
            "numero positivo")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La descrizione è obbligatoria")]
        [MaxLength(500, ErrorMessage = "La descrizione non deve superare i 500 " +
            "caratteri")]
        public string Description { get; set; }

        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "La dimensione è obbligatoria")]
        [MaxLength(50, ErrorMessage = "La dimensione non può superare i 50 caratteri")]
        public string size { get; set; }

        public List<int>? AmenitiesIds { get; set; }
    }
}

