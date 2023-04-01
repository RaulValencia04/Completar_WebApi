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
    public class facultadController : ControllerBase
    {

        private readonly EquiposContext _equipoContext;


        public facultadController(EquiposContext equipoContext)
        {
            _equipoContext = equipoContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<facultades> listadoEquipos = (from e in _equipoContext.facultades
                                                   select e).ToList();

            if (listadoEquipos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipos);

        }
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] facultades estados)
        {
            try
            {

                _equipoContext.facultades.Add(estados);
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
        public IActionResult actualizar(int id, [FromBody] facultades equipoMod)
        {

            facultades? existente = (from e in _equipoContext.facultades where e.facultad_id == id  select e).FirstOrDefault();

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
            facultades? existente = _equipoContext.facultades.Find(id);

            if (existente == null)
            {
                return NotFound();

            }
            //_equipoContext.equipos.Attach(existente);


            _equipoContext.facultades.Attach(existente);
            _equipoContext.facultades.Remove(existente);
            _equipoContext.SaveChanges();


            return Ok(existente);

        }
    }
}
