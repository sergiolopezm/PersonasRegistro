using Microsoft.AspNetCore.Mvc;
using PersonasRegistro.Application.Services;
using PersonasRegistro.Infrastructure.Repositories;
using PersonasRegistro.Shared.DTOsGenerales;
using PersonasRegistro.Shared.DTOsRequest;

namespace PersonasRegistro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly PersonaService _servicio;
        private readonly PersonaRepository _repositorio;

        public PersonaController(IConfiguration configuration)
        {
            _configuration = configuration;
            _repositorio = new PersonaRepository(_configuration.GetConnectionString("DefaultConnection"));
            _servicio = new PersonaService();
        }

        [HttpPost("registrar")]
        public IActionResult RegistrarPersona([FromBody] PersonaRequestDto request)
        {

            if (_repositorio.ExistePersona(request.DocumentoIdentidad))
            {
                return Conflict(new RespuestaDto
                {
                    Exito = false,
                    Mensaje = "Registro duplicado",
                    Detalle = $"Ya existe una persona con el documento '{request.DocumentoIdentidad}'."
                });
            }

            var respuesta = _servicio.RegistrarPersona(request);

            if (!respuesta.Exito)
                return BadRequest(respuesta);

            try
            {
                var persona = (Domain.Entities.Persona)respuesta.Resultado!;
                _repositorio.GuardarPersona(persona);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RespuestaDto
                {
                    Exito = false,
                    Mensaje = "Error al guardar en base de datos",
                    Detalle = ex.Message
                });
            }
        }
    }
}
