using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly EquiposContext _equipoContext;


        public reservasController(EquiposContext equipoContext)
        {
            _equipoContext = equipoContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoEquipos = (from e in _equipoContext.reservas
                                  join f in _equipoContext.usuarios
                                    on e.usuario_id equals f.usuario_id
                                  join es in _equipoContext.estados_Reserva
                                    on e.estado_reserva_id equals es.estado_res_id
                                  select new{
                                      e.reserva_id,
                                      e.equipo_id,
                                      e.fecha_salida,
                                      e.hora_salida,
                                      e.fecha_retorno,
                                      e.hora_retorno,
                                      f.nombre,
                                      es.estado

                                  }).ToList();
                                

            if (listadoEquipos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipos);

        }
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] reservas estados)
        {
            try
            {

                _equipoContext.reservas.Add(estados);
                _equipoContext.SaveChanges();
                return Ok(estados);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] reservas equipoMod)
        {

            reservas? existente = (from e in _equipoContext.reservas where e.reserva_id == id select e).FirstOrDefault();

            if (existente == null)
            {
                return NotFound();
            }


            _equipoContext.Entry(existente).State = EntityState.Modified;
            _equipoContext.SaveChanges();

            return Ok(existente);


        }



        [HttpPut]
        [Route("delete/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            reservas? existente = _equipoContext.reservas.Find(id);


            var listadoEquipos = (from e in _equipoContext.reservas
                                  select e).FirstOrDefault();

            if (existente == null)
            {
                return NotFound();

            }
            //_equipoContext.equipos.Attach(existente);


            _equipoContext.reservas.Attach(existente);
            _equipoContext.reservas.Remove(existente);
            _equipoContext.SaveChanges();


            return Ok(existente);

        }
    }
}
