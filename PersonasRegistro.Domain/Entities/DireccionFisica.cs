namespace PersonasRegistro.Domain.Entities
{
    public class DireccionFisica
    {
        public string Calle { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string? CodigoPostal { get; set; }
    }
}
