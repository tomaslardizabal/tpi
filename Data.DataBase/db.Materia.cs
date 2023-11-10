using System;
using System.Data;
using System.Data.SqlClient;
using Entidades;


namespace Data.DataBase
{
    public class MateriasDAO
    {
        #region ALTA MATERIA
        public bool AgregarMateria(Materia materia)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "INSERT INTO Materias (desc_materia, hs_semanales, hs_totales, id_plan) VALUES (@desc_materia, @hs_semanales, @hs_totales, @id_plan)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@desc_materia", materia.DescMateria);
                        command.Parameters.AddWithValue("@hs_semanales", materia.HsSemanales);
                        command.Parameters.AddWithValue("@hs_totales", materia.HsTotales);
                        command.Parameters.AddWithValue("@id_plan", materia.IdPlan);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar la materia: " + ex.Message);
                return false;
            }
        }
        #endregion


        #region MODIFICAR MATERIA
        public bool ModificarMateria(Materia materia)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();

                    string query = "UPDATE Materias SET desc_materia = @Nueva_desc_materia, hs_semanales = @Nuevo_hs_semanales, hs_totales = @Nuevo_hs_totales, id_plan = @Nuevo_id_plan WHERE id_materia = @IDMateria";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IDMateria", materia.IdMateria);
                        command.Parameters.AddWithValue("@Nueva_desc_materia", materia.DescMateria);
                        command.Parameters.AddWithValue("@Nuevo_hs_semanales", materia.HsSemanales);
                        command.Parameters.AddWithValue("@Nuevo_hs_totales", materia.HsTotales);
                        command.Parameters.AddWithValue("@Nuevo_id_plan", materia.IdPlan);
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al modificar la materia: " + ex.Message);
                return false;
            }
        }
        #endregion


        #region ELIMINAR MATERIA
        public bool EliminarMateria(Materia materia)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();

                    string query = "DELETE FROM Materias WHERE id_materia = @id_materia";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id_materia", materia.IdMateria);

                        int rowsAffected = command.ExecuteNonQuery();

                        // Si se eliminó al menos una fila, significa que el plan se eliminó correctamente
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


        #region OBTENER TODAS LAS MATERIAS
        public DataTable ObtenerTodasLasMaterias()
        {
            DataTable dtMaterias = new DataTable();
            using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
            {
                string query = "SELECT id_materia, desc_materia,hs_semanales, hs_totales, id_plan FROM Materias";
                using (SqlCommand commnad = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter(commnad);
                    da.Fill(dtMaterias);
                }
            }
            return dtMaterias;
        }
        #endregion


        #region OBTENER MATERIAS DE UN PLAN
        public DataTable ObtenerMateriasDePlan(int idPlan)
        {
            DataTable dtMaterias = new DataTable();
            using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
            {
                string query = "SELECT DISTINCT id_materia, desc_materia,hs_semanales, hs_totales FROM Materias INNER JOIN Planes ON Materias.id_plan = @idPlan";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idPlan", idPlan);
                    connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtMaterias);
                }
            }
            return dtMaterias;
        }
        #endregion


        #region OBTENER DESCRIPCION DE LA MATERIA
        public string ObtenerDescripcionMateria(int id_materia)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "SELECT desc_materia FROM Materias WHERE id_materia = @id_materia";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id_materia", id_materia);
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
                Console.WriteLine("Error al obtener la descripción de la materia: " + ex.Message);
            }

            return null; // Devolver null si no se encuentra la descripción
        }
        #endregion
    }
}