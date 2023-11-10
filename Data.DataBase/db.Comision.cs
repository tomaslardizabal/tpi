using System;
using System.Data.SqlClient;
using System.Data;
using Entidades;

namespace Data.DataBase
{
    public class ComisionesDAO
    {
        #region ALTA COMISION
        public bool InsertarComision(Comision comision)     // Método para insertar una nueva comision en la base de datos
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "INSERT INTO Comisiones (desc_comision, anio_especialidad, id_plan) VALUES (@desc_comision, @anio_especialidad, @id_plan)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@desc_comision", comision.DescComision);
                        command.Parameters.AddWithValue("@anio_especialidad", comision.AnioEspecialidad);
                        command.Parameters.AddWithValue(@"id_plan", comision.IdPlan);
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


        #region MODIFICAR COMISION
        public bool ModificarComision(Comision comision)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();

                    string query = "UPDATE Comisiones SET desc_comision = @NuevaDescripcion, anio_especialidad = @NuevoAnioEspecialidad,  id_plan = @NuevoPlan WHERE id_comision = @IDComision";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IDComision", comision.IdComision);
                        command.Parameters.AddWithValue("@NuevaDescripcion", comision.DescComision);
                        command.Parameters.AddWithValue("@NuevoAnioEspecialidad", comision.AnioEspecialidad);
                        command.Parameters.AddWithValue("@NuevoPlan",comision.IdPlan);

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


        #region ELIMINAR COMISION
        public bool EliminarComision(Comision comision)
        {
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                    {
                        connection.Open();
                        string query = "DELETE FROM Comisiones WHERE id_comision = @id_comision";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id_comision", comision.IdComision);
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
        }
        #endregion


        #region OBTENER TODAS LAS COMISIONES
        public DataTable ObtenerTodasLasComisiones()
        {
            DataTable dtComisiones = new DataTable();

            using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
            {
                string query = "SELECT id_comision, desc_comision,anio_especialidad, id_plan FROM Comisiones";


                using (SqlCommand commnad = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(commnad);
                    adapter.Fill(dtComisiones);
                }
            }
            return dtComisiones;
        }
        #endregion


        #region OBTENER DESCRIPCION
        public string ObtenerDescripcionComision(int id_comision)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "SELECT desc_comision FROM Comisiones WHERE id_comision = @id_comision";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id_comision", id_comision);
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            return result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones (opcional)
                Console.WriteLine("Error al obtener la descripción de la comision: " + ex.Message);
            }

            return null; // Devolver null si no se encuentra la descripción
        }
        #endregion
    }
}