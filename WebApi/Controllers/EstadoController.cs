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
    public class EstadoController : ControllerBase
    {
        private readonly EquiposContext _equipoContext;


        public EstadoController(EquiposContext equipoContext)
        {
            _equipoContext = equipoContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Estados_equipo> listadoEquipos = (from e in _equipoContext.Estados_equipo
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
        public IActionResult GuardarEquipo([FromBody] Estados_equipo estados)
        {
            try
            {

                _equipoContext.Estados_equipo.Add(estados);
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
        public IActionResult actualizar(int id, [FromBody] Estados_equipo equipoMod)
        {

            Estados_equipo? existente = (from e in _equipoContext.Estados_equipo where e.id_estados_equipo == id && e.estado == "A" select e).FirstOrDefault();

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
            Estados_equipo? existente = _equipoContext.Estados_equipo.Find(id);

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
