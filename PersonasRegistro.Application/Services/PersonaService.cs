using PersonasRegistro.Domain.Entities;
using PersonasRegistro.Shared.DTOsGenerales;
using PersonasRegistro.Shared.DTOsRequest;

namespace PersonasRegistro.Application.Services
{
    public class PersonaService
    {
        public RespuestaDto RegistrarPersona(PersonaRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DocumentoIdentidad))
                return RespuestaDto.ParametrosIncorrectos("Documento requerido", "El campo DocumentoIdentidad no puede estar vacío");

            if (!SoloLetras(dto.Nombres) || !SoloLetras(dto.Apellidos))
                return RespuestaDto.ParametrosIncorrectos("Nombres inválidos", "Solo se permiten letras en nombres y apellidos");

            if (!SoloNumeros(dto.DocumentoIdentidad))
                return RespuestaDto.ParametrosIncorrectos("Documento inválido", "El documento de identidad solo puede contener números");

            bool tieneContacto = dto.Correos.Any() || dto.Direcciones.Any();
            if (!tieneContacto)
                return RespuestaDto.ParametrosIncorrectos("Falta información de contacto", "Debe registrar al menos un correo o una dirección física");

            var persona = new Persona
            {
                DocumentoIdentidad = dto.DocumentoIdentidad,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                FechaNacimiento = dto.FechaNacimiento,
                Telefonos = dto.Telefonos.Select(t => new Telefono { Numero = t.Numero }).ToList(),
                Correos = dto.Correos.Select(c => new CorreoElectronico { Direccion = c.Direccion }).ToList(),
                Direcciones = dto.Direcciones.Select(d => new DireccionFisica { Calle = d.Calle, Ciudad = d.Ciudad, CodigoPostal = d.CodigoPostal }).ToList()
            };

            return new RespuestaDto
            {
                Exito = true,
                Mensaje = "Registro exitoso",
                Detalle = "La persona fue registrada exitosamente.",
                Resultado = persona
            };
        }

        private bool SoloLetras(string texto)
        {
            return texto.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
        }

        private bool SoloNumeros(string texto)
        {
            return texto.All(c => char.IsDigit(c));
        }
    }
}

