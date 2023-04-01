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
    public class tipoController : ControllerBase
    {
        private readonly EquiposContext _equipoContext;


        public tipoController(EquiposContext equipoContext)
        {
            _equipoContext = equipoContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<tipo_equipo> listadoEquipos = (from e in _equipoContext.tipo_equipo
                                           where e.estado == "A"
                                           select e).ToList();

            if (listadoEquipos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipos);

        }
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] tipo_equipo marca)
        {
            try
            {


                _equipoContext.tipo_equipo.Add(marca);
                _equipoContext.SaveChanges();
                return Ok(marca);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] tipo_equipo equipoMod)
        {

            tipo_equipo? existente = (from e in _equipoContext.tipo_equipo where e.id_tipo_equipo == id && e.estado == "A" select e).FirstOrDefault();

            if (existente == null)
            {
                return NotFound();
            }
            equipoMod.estado = "A";


            _equipoContext.Entry(existente).State = EntityState.Modified;
            _equipoContext.SaveChanges();

            return Ok(existente);


        }



        [HttpPut]
        [Route("delete/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            tipo_equipo? existente = _equipoContext.tipo_equipo.Find(id);

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
