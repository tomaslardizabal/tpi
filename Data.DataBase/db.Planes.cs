using System;
using System.Data;
using System.Data.SqlClient;
using Entidades;

namespace Data.DataBase
{
    public class PlanesDAO
    {
        #region ALTA PLAN
        public bool InsertarPlan(Plan plan)        // Método para insertar un nuevo plan en la base de datos
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "INSERT INTO Planes (desc_plan, id_especialidad) VALUES (@desc_plan, @id_especialidad)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@desc_plan", plan.DescPlan);
                        command.Parameters.AddWithValue("@id_especialidad", plan.IdEspecialidad);
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


        #region MODIFICAR PLAN
        public bool ModificarPlan(Plan plan)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();

                    string query = "UPDATE Planes SET desc_plan = @NuevaDescripcion, id_especialidad = @NuevoIdEspecialidad WHERE id_plan = @IDPlan";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IDPlan", plan.IdPlan);
                        command.Parameters.AddWithValue("@NuevaDescripcion", plan.DescPlan);
                        command.Parameters.AddWithValue("@NuevoIdEspecialidad", plan.IdEspecialidad);

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


        #region ELIMINAR PLAN
        public bool EliminarPlan(Plan plan)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "DELETE FROM Planes WHERE id_plan = @id_plan ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id_plan", plan.IdPlan);

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


        #region OBTENER TODOS LOS PLANES
        public DataTable ObtenerTodosLosPlanes()
        {
            DataTable dtPlanes = new DataTable();

            using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
            {
                string query = "SELECT id_plan, desc_plan, id_especialidad FROM Planes";

                using (SqlCommand commnad = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(commnad);
                    adapter.Fill(dtPlanes);
                }
            }
            return dtPlanes;
        }
        #endregion


        #region OBTENER DESCRIPCION DE LOS PLANES
        public string ObtenerDescripcionPlanes(int idPlan)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "SELECT desc_plan FROM Planes WHERE id_plan = @id_plan";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id_plan", idPlan);
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
                Console.WriteLine("Error al obtener la descripción de la especialidad: " + ex.Message);
            }

            return null; // Devolver null si no se encuentra la descripción
        }
        #endregion
    }
}