namespace PersonasRegistro.Shared.DTOsGenerales
{
    public class RespuestaDto
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public object? Resultado { get; set; }

        public static RespuestaDto ParametrosIncorrectos(string mensaje, string detalle)
        {
            return new RespuestaDto
            {
                Exito = false,
                Mensaje = mensaje,
                Detalle = detalle
            };
        }
    }
}
