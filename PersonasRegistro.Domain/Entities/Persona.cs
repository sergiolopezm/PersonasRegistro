namespace PersonasRegistro.Domain.Entities
{
    public class Persona
    {
        public string DocumentoIdentidad { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }

        public List<Telefono> Telefonos { get; set; } = new();
        public List<CorreoElectronico> Correos { get; set; } = new();
        public List<DireccionFisica> Direcciones { get; set; } = new();
    }

}
