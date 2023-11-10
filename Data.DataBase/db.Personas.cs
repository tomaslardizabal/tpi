using Entidades;
using System;
using System.Data.SqlClient;
using System.Data;

namespace Data.DataBase
{
    public class PersonasDAO
    {
        #region ALTA PERSONA
        public int InsertarPersona(Persona persona)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "INSERT INTO Personas (nombre, apellido, direccion, email, telefono, fecha_nac, legajo, tipo_persona, id_plan) VALUES (@nombre, @apellido, @direccion, @email, @telefono, @fecha_nac, @legajo, @tipo_persona, @id_plan)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", persona.Nombre);
                        command.Parameters.AddWithValue("@apellido", persona.Apellido);
                        command.Parameters.AddWithValue("@direccion", persona.Direccion);
                        command.Parameters.AddWithValue("@email", persona.Email);
                        command.Parameters.AddWithValue("@telefono", persona.Telefono);
                        command.Parameters.AddWithValue("@fecha_nac", persona.FechaNac);
                        command.Parameters.AddWithValue("@legajo", persona.Legajo);
                        command.Parameters.AddWithValue("@tipo_persona", persona.TipoPersona);
                        command.Parameters.AddWithValue("@id_plan", persona.IdPlan);

                        int rowsAffected = command.ExecuteNonQuery();

                        query = "SELECT MAX(id_persona) FROM Personas";
                        using (SqlCommand idCommand = new SqlCommand(query, connection))
                        {
                            object result = idCommand.ExecuteScalar();
                            return Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region OBTENER TODAS LAS PERSONAS
        public DataTable ObtenerTodasLasPersonas(int tipo)
        {
            DataTable dtPersonas = new DataTable();

            using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
            {
                string query = "SELECT id_persona,nombre, apellido, direccion, email, telefono, fecha_nac, legajo, tipo_persona, id_plan FROM Personas WHERE tipo_persona = @tipo";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tipo", tipo); // Usamos un parámetro para el valor de tipo.

                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dtPersonas);
                }
            }
            return dtPersonas;
        }
        #endregion


        #region BUSCAR PERSONA POR APELLIDO
        public DataTable BusquedaFiltrada(int tipo, string apellido)
        {
            DataTable dtPersonasFiltradas = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "SELECT id_persona,nombre, apellido, direccion, email, telefono, fecha_nac, legajo, tipo_persona, id_plan FROM Personas WHERE tipo_persona = @tipo AND apellido = @apellido";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tipo", tipo);
                        command.Parameters.AddWithValue("@apellido", apellido);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dtPersonasFiltradas);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la búsqueda: " + ex.Message);
            }
            return dtPersonasFiltradas;
        }
        #endregion

        #region MODIFICAR PERSONA
        public int ModificarPersona(Persona persona)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();

                    string query = "UPDATE Personas SET nombre = @NuevoNombre, apellido = @NuevoApellido, direccion = @NuevaDireccion, email = @NuevoEmail, telefono = @NuevoTelefono, fecha_nac = @NuevaFechaNac, legajo = @NuevoLegajo, tipo_persona = @NuevoTipoPersona, id_plan = @NuevoIdPlan WHERE id_persona = @IDPersona";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IDPersona", persona.IdPersona);
                        command.Parameters.AddWithValue("@NuevoNombre", persona.Nombre);
                        command.Parameters.AddWithValue("@NuevoApellido", persona.Apellido);
                        command.Parameters.AddWithValue("@NuevaDireccion", persona.Direccion);
                        command.Parameters.AddWithValue("@NuevoEmail", persona.Email);
                        command.Parameters.AddWithValue("@NuevoTelefono", persona.Telefono);
                        command.Parameters.AddWithValue("@NuevaFechaNac", persona.FechaNac);
                        command.Parameters.AddWithValue("@NuevoLegajo", persona.Legajo);
                        command.Parameters.AddWithValue("@NuevoTipoPersona", persona.TipoPersona);
                        command.Parameters.AddWithValue("@NuevoIdPlan", persona.IdPlan);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected;
                    }
                }
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region ELIMINAR PERSONA
        public bool EliminarPersona(Persona persona)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = @"
                                    DELETE FROM Usuarios WHERE id_persona = @id_persona;
                            
                                    DELETE FROM Personas WHERE id_persona = @id_persona;
                                   
                                    ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id_persona", persona.IdPersona);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}