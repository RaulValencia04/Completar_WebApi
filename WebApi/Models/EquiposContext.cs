using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace WebApi.Models
{
	public class EquiposContext :DbContext
	{
		public EquiposContext( DbContextOptions<EquiposContext> options ) : base(options)
        {

		}
		public DbSet<Equipos> Equipos { get; set; }

        public DbSet<Marcas> Marcas { get; set; }

        public DbSet<Estados_equipo> Estados_equipo { get; set; }

		public DbSet<tipo_equipo> tipo_equipo { get; set; }

		public DbSet<Carreras> carreras { get; set; }

        public DbSet<facultades> facultades { get; set; }

        public DbSet<Estados_reserva> estados_Reserva { get; set; }

        public DbSet<usuario> usuarios { get; set; }

        public DbSet<reservas> reservas { get; set; }






    }
}

