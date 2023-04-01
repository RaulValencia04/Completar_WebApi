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
    public class MarcaController : ControllerBase
    {
        private readonly EquiposContext _equipoContext;


        public MarcaController(EquiposContext equipoContext)
        {
            _equipoContext = equipoContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Marcas> listadoEquipos = (from e in _equipoContext.Marcas
                                            where e.estados == "A"
                                            select e).ToList();

            if (listadoEquipos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipos);

        }
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] Marcas marca)
        {
            try
            {
 

                _equipoContext.Marcas.Add(marca);
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
        public IActionResult actualizar(int id, [FromBody] Marcas equipoMod)
        {

            Marcas? existente = (from e in _equipoContext.Marcas where e.id_marcas == id && e.estados == "A" select e).FirstOrDefault();

            if (existente == null)
            {
                return NotFound();
            }
            equipoMod.estados = "A";


            _equipoContext.Entry(existente).State = EntityState.Modified;
            _equipoContext.SaveChanges();

            return Ok(existente);


        }



        [HttpPut]
        [Route("delete/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            Marcas? existente = _equipoContext.Marcas.Find(id);

            if (existente == null)
            {
                return NotFound();

            }
            //_equipoContext.equipos.Attach(existente);


            existente.estados = "I";
            _equipoContext.Entry(existente).State = EntityState.Modified;


            return Ok(existente);

        }

    }
}
