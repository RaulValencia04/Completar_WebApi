using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
	public class Estados_equipo
	{
        [Key]
        public int id_estados_equipo { get; set; }

        public string? descripcion { get; set; }

        public string? estado { get; set; }


    }
}

