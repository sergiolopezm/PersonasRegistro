using PersonasRegistro.Domain.Entities;
using System.Data.SqlClient;

namespace PersonasRegistro.Infrastructure.Repositories
{
    public class PersonaRepository
    {
        private readonly string _connectionString;

        public PersonaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void GuardarPersona(Persona persona)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                var cmdPersona = new SqlCommand(@"
                    INSERT INTO Personas (DocumentoIdentidad, Nombres, Apellidos, FechaNacimiento)
                    VALUES (@doc, @nombres, @apellidos, @fecha)", connection, transaction);

                cmdPersona.Parameters.AddWithValue("@doc", persona.DocumentoIdentidad);
                cmdPersona.Parameters.AddWithValue("@nombres", persona.Nombres);
                cmdPersona.Parameters.AddWithValue("@apellidos", persona.Apellidos);
                cmdPersona.Parameters.AddWithValue("@fecha", persona.FechaNacimiento);

                cmdPersona.ExecuteNonQuery();

                foreach (var tel in persona.Telefonos)
                {
                    var cmdTel = new SqlCommand(@"
                        INSERT INTO Telefonos (Numero, PersonaId) VALUES (@num, @id)", connection, transaction);
                    cmdTel.Parameters.AddWithValue("@num", tel.Numero);
                    cmdTel.Parameters.AddWithValue("@id", persona.DocumentoIdentidad);
                    cmdTel.ExecuteNonQuery();
                }

                foreach (var correo in persona.Correos)
                {
                    var cmdCorreo = new SqlCommand(@"
                        INSERT INTO CorreosElectronicos (Direccion, PersonaId) VALUES (@correo, @id)", connection, transaction);
                    cmdCorreo.Parameters.AddWithValue("@correo", correo.Direccion);
                    cmdCorreo.Parameters.AddWithValue("@id", persona.DocumentoIdentidad);
                    cmdCorreo.ExecuteNonQuery();
                }

                foreach (var dir in persona.Direcciones)
                {
                    var cmdDir = new SqlCommand(@"
                        INSERT INTO DireccionesFisicas (Calle, Ciudad, CodigoPostal, PersonaId)
                        VALUES (@calle, @ciudad, @cp, @id)", connection, transaction);

                    cmdDir.Parameters.AddWithValue("@calle", dir.Calle);
                    cmdDir.Parameters.AddWithValue("@ciudad", dir.Ciudad);
                    cmdDir.Parameters.AddWithValue("@cp", (object?)dir.CodigoPostal ?? DBNull.Value);
                    cmdDir.Parameters.AddWithValue("@id", persona.DocumentoIdentidad);
                    cmdDir.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public bool ExistePersona(string documento)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand("SELECT COUNT(*) FROM Personas WHERE DocumentoIdentidad = @doc", connection);
            cmd.Parameters.AddWithValue("@doc", documento);

            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }
}
