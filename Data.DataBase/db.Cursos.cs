using System;
using System.Data.SqlClient;
using System.Data;
using Entidades;

namespace Data.DataBase
{
    public class CursosDAO
    {
        #region ALTA CURSO
        public bool InsertarCursos(Curso curso)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "INSERT INTO Cursos (anio_calendario, cupo, id_comision, id_materia) VALUES (@anio_calendario, @cupo, @id_comision, @id_materia)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@anio_calendario", curso.AnioCalendario);
                        command.Parameters.AddWithValue("@cupo", curso.Cupo);
                        command.Parameters.AddWithValue("@id_comision", curso.IdComision);
                        command.Parameters.AddWithValue("@id_materia", curso.IdMateria);
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


        #region MODIFICAR CURSO
        public bool ModificarCurso(Curso curso)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();

                    string query = "UPDATE Cursos SET id_comision = @NuevaIdComision, id_materia = @NuevaIdMateria, anio_calendario = @NuevoAnioCalendario,  cupo = @NuevoCupo WHERE id_curso = @IdCurso";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdCurso", curso.IdCurso);
                        command.Parameters.AddWithValue("@NuevaIdComision", curso.IdComision);
                        command.Parameters.AddWithValue("@NuevaIdMateria", curso.IdMateria);
                        command.Parameters.AddWithValue("@NuevoAnioCalendario", curso.AnioCalendario);
                        command.Parameters.AddWithValue("@NuevoCupo", curso.Cupo);

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


        #region ELIMINAR CURSO
        public bool EliminarCurso(Curso curso)
        {
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                    {
                        connection.Open();
                        string query = "DELETE FROM Cursos WHERE  id_curso = @id_curso";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id_curso", curso.IdCurso);
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


        #region OBTENER TODOS LOS CURSOS
        public DataTable ObtenerTodasLosCursos()
        {
            DataTable dtCursos = new DataTable();

            using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
            {
                string query = "SELECT id_curso, anio_calendario, cupo, id_comision, id_materia FROM Cursos";


                using (SqlCommand commnad = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(commnad);
                    adapter.Fill(dtCursos);
                }
            }
            return dtCursos;
        }
        #endregion


        #region OBTENER COMISIONES POR MATERIA
        public DataTable ObtenerComisionesPorMateria(int id_materia)
        {
            DataTable dtComisiones = new DataTable();

            using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
            {
                string query = "SELECT CO.id_comision, CO.desc_comision FROM Cursos C INNER JOIN Comisiones CO ON CO.id_comision = C.id_comision  WHERE C.id_materia =  @id_materia";


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@id_materia", id_materia);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dtComisiones);
                }
            }
            return dtComisiones;
        }
        #endregion


        #region OBTENER ID CURSO
        public static int ObtenerIDCurso(int id_materia, int id_comision)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "SELECT id_curso FROM Cursos WHERE  id_materia = @id_materia AND id_comision = @id_comision";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id_materia", id_materia);
                        command.Parameters.AddWithValue("@id_comision", id_comision);

                        object result = command.ExecuteScalar();

                        // Si result es diferente de null, significa que se encontró una coincidencia, devuelvo el ID del curso como entero
                       // if (result != null)
                        //{
                            return Convert.ToInt32(result);
                     //   }
                    }
                }
            }
            catch 
            {
                return -1;
            }
        }
        #endregion


        #region OBTENER MATERIA Y COMISION
        public (int, int) ObtenerMateriaYComisionPorCurso(int idCurso)
        {
            using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
            {
                connection.Open();
                string query = "SELECT id_materia, id_comision FROM Cursos WHERE id_curso = @id_curso";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_curso", idCurso);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int idMateria = Convert.ToInt32(reader["id_materia"]);
                            int idComision = Convert.ToInt32(reader["id_comision"]);
                            return (idMateria, idComision);
                        }
                    }
                }
            }
            return (-1, -1);
        }
        #endregion
    }
}