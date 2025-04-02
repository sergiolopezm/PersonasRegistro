namespace PersonasRegistro.Shared.DTOsRequest
{
    public class PersonaRequestDto
    {
        public string DocumentoIdentidad { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }

        public List<TelefonoDto> Telefonos { get; set; } = new();
        public List<CorreoElectronicoDto> Correos { get; set; } = new();
        public List<DireccionFisicaDto> Direcciones { get; set; } = new();
    }
}
