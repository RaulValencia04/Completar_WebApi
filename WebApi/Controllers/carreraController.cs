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
    public class carreraController : ControllerBase
    {
        private readonly EquiposContext _equipoContext;


        public carreraController(EquiposContext equipoContext)
        {
            _equipoContext = equipoContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoEquipos = (from e in _equipoContext.carreras
                                             join f in _equipoContext.facultades
                                             on e.facultad_id equals f.facultad_id
                                               select new
                                               {
                                                   e.carrera_id,
                                                   e.nombre_carrera,
                                                   f.nombre_facultad
                                               }).ToList();

            if (listadoEquipos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipos);

        }
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] Carreras estados)
        {
            try
            {

                _equipoContext.carreras.Add(estados);
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
        public IActionResult actualizar(int id, [FromBody] Carreras equipoMod)
        {

            Carreras? existente = (from e in _equipoContext.carreras where e.carrera_id == id  select e).FirstOrDefault();

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
            Carreras? existente = _equipoContext.carreras.Find(id);


            var listadoEquipos = (from e in _equipoContext.carreras
                                  join f in _equipoContext.facultades
                                  on e.facultad_id equals f.facultad_id
                                  select e).FirstOrDefault();

            if (existente == null)
            {
                return NotFound();

            }
            //_equipoContext.equipos.Attach(existente);


            _equipoContext.carreras.Attach(existente);
            _equipoContext.carreras.Remove(existente);
            _equipoContext.SaveChanges();


            return Ok(existente);

        }
    }
}
