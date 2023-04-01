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
    public class usuarioController : ControllerBase
    {

        private readonly EquiposContext _equipoContext;


        public usuarioController(EquiposContext equipoContext)
        {
            _equipoContext = equipoContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoEquipos = (from e in _equipoContext.usuarios
                                  join f in _equipoContext.carreras
                                  on e.carrera_id equals f.carrera_id
                                       select new
                                       {
                                           e.usuario_id,
                                           e.nombre,
                                           e.tipo,
                                           e.documento,
                                           e.carnet,
                                           f.nombre_carrera
                                       }).ToList();

            if (listadoEquipos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipos);

        }
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] usuario estados)
        {
            try
            {

                _equipoContext.usuarios.Add(estados);
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
        public IActionResult actualizar(int id, [FromBody] usuario equipoMod)
        {

            usuario? existente = (from e in _equipoContext.usuarios where e.usuario_id == id select e).FirstOrDefault();

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
            usuario? existente = _equipoContext.usuarios.Find(id);


            var listadoEquipos = (from e in _equipoContext.usuarios
                                  select e).FirstOrDefault();

            if (existente == null)
            {
                return NotFound();

            }
            //_equipoContext.equipos.Attach(existente);


            _equipoContext.usuarios.Attach(existente);
            _equipoContext.usuarios.Remove(existente);
            _equipoContext.SaveChanges();


            return Ok(existente);

        }
    }
}
