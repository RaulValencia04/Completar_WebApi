﻿using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
	public class Marcas
	{
        [Key]

        public int id_marcas { get; set; }

        public string? nombre_marca { get; set; }

        public string? estados { get; set; }
    }
}

