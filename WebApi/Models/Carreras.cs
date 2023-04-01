using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
	public class Carreras
	{
        [Key]

        public int carrera_id { get; set; }

        public string? nombre_carrera { get; set; }

        public int facultad_id { get; set; }
    }
}

