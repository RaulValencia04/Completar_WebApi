using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equipoController : ControllerBase
    {
        private readonly EquiposContext _equipoContext;


        public equipoController(EquiposContext equipoContext)
        {
            _equipoContext = equipoContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoEquipos = (from e in _equipoContext.Equipos
                                            join t in _equipoContext.Estados_equipo
                                                on e.estado_equipo_id equals t.id_estados_equipo
                                            join m in _equipoContext.Marcas
                                                on e.marca_id equals m.id_marcas
                                            join tp in _equipoContext.tipo_equipo
                                                on e.tipo_equipo_id equals tp.id_tipo_equipo
                                            where e.estado == "A"
                                            select new
                                            {
                                                e.id_equipos,
                                                e.descripcion,
                                                e.anio_compra,
                                                marca = m.nombre_marca,
                                                e.modelo,
                                                e.costo,
                                                e.vida_util,
                                                e.estado,
                                                tipo = tp.descripcion,
                                                estadoE = t.descripcion

                                            }).ToList();

            if (listadoEquipos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipos);

        }

        [HttpGet]
        [Route("getbyid")]
        public IActionResult Get(int id)
        {
            Equipos? equipo = _equipoContext.Equipos.Find(id);
            if (equipo == null) { return NotFound(); }
            return Ok(equipo);

        }




        [HttpGet]
        [Route("find")]
        public IActionResult FindByDescription(string filtro)
        {
            Equipos? equipo = (from e in _equipoContext.Equipos
                               where (e.descripcion.Contains(filtro))
                               && e.estado == "A"
                               select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();

            }


            return Ok(equipo);

        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] Equipos equipo)
        {
            try
            {
                equipo.estado = "A";
                _equipoContext.Equipos.Add(equipo);
                _equipoContext.SaveChanges();
                return Ok(equipo);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] Equipos equipoMod)
        {

            Equipos? existente = (from e in _equipoContext.Equipos where e.id_equipos == id && e.estado == "A" select e).FirstOrDefault();

            if (existente == null)
            {
                return NotFound();
            }
            equipoMod.estado = "A";

            existente.nombre = equipoMod.nombre;
            existente.descripcion = equipoMod.descripcion;

            _equipoContext.Entry(existente).State = EntityState.Modified;
            _equipoContext.SaveChanges();

            return Ok(existente);


        }

        [HttpPut]
        [Route("delete/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            Equipos? existente = _equipoContext.Equipos.Find(id);

            if (existente == null)
            {
                return NotFound();

            }
            //_equipoContext.equipos.Attach(existente);


            existente.estado = "I";
            _equipoContext.Entry(existente).State = EntityState.Modified;


            return Ok(existente);

        }


    }
}
