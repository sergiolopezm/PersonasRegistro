namespace PersonasRegistro.Shared.DTOsGenerales
{
    public class ValidoDto
    {
        public bool EsValido { get; set; }
        public string? Mensaje { get; set; }

        public static ValidoDto Crear(bool valido, string? mensaje = null)
        {
            return new ValidoDto
            {
                EsValido = valido,
                Mensaje = mensaje
            };
        }
    }
}
