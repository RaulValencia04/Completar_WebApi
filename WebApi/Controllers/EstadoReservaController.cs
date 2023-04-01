using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoReservaController : ControllerBase
    {
        private readonly EquiposContext _equipoContext;


        public EstadoReservaController(EquiposContext equipoContext)
        {
            _equipoContext = equipoContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoEquipos = (from e in _equipoContext.estados_Reserva
                                select e).ToList();

            if (listadoEquipos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipos);

        }
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] Estados_reserva estados)
        {
            try
            {

                _equipoContext.estados_Reserva.Add(estados);
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
        public IActionResult actualizar(int id, [FromBody] Estados_reserva equipoMod)
        {

            Estados_reserva? existente = (from e in _equipoContext.estados_Reserva where e.estado_res_id == id select e).FirstOrDefault();

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
            Estados_reserva? existente = _equipoContext.estados_Reserva.Find(id);


            var listadoEquipos = (from e in _equipoContext.estados_Reserva
                                  select e).FirstOrDefault();

            if (existente == null)
            {
                return NotFound();

            }
            //_equipoContext.equipos.Attach(existente);


            _equipoContext.estados_Reserva.Attach(existente);
            _equipoContext.estados_Reserva.Remove(existente);
            _equipoContext.SaveChanges();


            return Ok(existente);

        }

    }
}
