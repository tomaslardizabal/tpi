using System;
using System.Data.SqlClient;
using System.Data;
using Entidades;

namespace Data.DataBase
{
    public class EspecialidadesDAO
    {
        #region ALTA ESPECIALIDAD
        public bool InsertarEspecialidad(Especialidad especialidad)       // Método para insertar una nueva especialidad en la base de datos  
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "INSERT INTO Especialidades (desc_especialidad) VALUES (@desc_especialidad)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@desc_especialidad", especialidad.DescEspecialidad);
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


        #region MODIFICAR ESPECIALIDAD
        public bool ModificarEspecialidad(Especialidad especialidad)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "UPDATE Especialidades SET desc_especialidad = @NuevaDescripcion WHERE id_especialidad = @id_especialidad";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id_especialidad", especialidad.IdEspecialidad);
                        command.Parameters.AddWithValue("@NuevaDescripcion", especialidad.DescEspecialidad);

                        int rowsAffected = command.ExecuteNonQuery();

                         return rowsAffected > 0;
                    }
                }
            }
            catch               //si sale algo mal devuelve falso
            {
                return false;
            }
        }
        #endregion


        #region ELIMINAR ESPECIALIDAD
        public bool EliminarEspecialidad(Especialidad especialidad)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "DELETE FROM Especialidades WHERE id_especialidad = @id_especialidad";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id_especialidad", especialidad.IdEspecialidad);

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


        #region OBTENER TODAS LAS ESPECIALIDADES
        public DataTable ObtenerTodasLasEspecialidades()
        {
            DataTable dtEspecialidades = new DataTable();

            using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
            {
                string query = "SELECT  id_especialidad, desc_especialidad FROM Especialidades";

                using (SqlCommand commnad = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(commnad);
                    adapter.Fill(dtEspecialidades);
                }
            }
            return dtEspecialidades;
        }
        #endregion


        #region OBTENER DESCRIPCION ESPECIALIDAD
        public string ObtenerDescripcionEspecialidad(int idEspecialidad)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "SELECT desc_especialidad FROM Especialidades WHERE id_especialidad = @idEspecialidad";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idEspecialidad", idEspecialidad);
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            return result.ToString();
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error al cargar especialidades");
            }

            return null; // Devolver null si no se encuentra la descripción
        }
        #endregion
    }
}