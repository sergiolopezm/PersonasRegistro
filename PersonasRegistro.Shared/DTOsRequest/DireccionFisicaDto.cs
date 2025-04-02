namespace PersonasRegistro.Shared.DTOsRequest
{
    public class DireccionFisicaDto
    {
        public string Calle { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string? CodigoPostal { get; set; }
    }
}
