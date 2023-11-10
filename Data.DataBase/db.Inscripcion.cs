using Entidades;
using System;
using System.Data.SqlClient;
using System.Data;

namespace Data.DataBase
{
    public class InscripcionesDAO
    {
        #region ALTA INSCRIPCION
        public bool InsertarInscripcion(Inscripcion inscripcion)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    // Verificar si el cupo del curso es mayor que 0
                    string cupoQuery = "SELECT cupo FROM Cursos WHERE id_curso = @id_curso";
                    using (SqlCommand cupoCommand = new SqlCommand(cupoQuery, connection))
                    {
                        cupoCommand.Parameters.AddWithValue("@id_curso", inscripcion.IdCurso);
                        int cupo = (int)cupoCommand.ExecuteScalar();

                        if (cupo > 0)
                        {
                            // Si el cupo es mayor que 0, realizar la inserción
                            string insertQuery = "INSERT INTO Alumnos_Inscripciones (id_alumno, id_curso, condicion, nota) VALUES (@id_alumno, @id_curso, @condicion, @nota)";
                            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@id_alumno", inscripcion.IdAlumno);
                                insertCommand.Parameters.AddWithValue("@id_curso", inscripcion.IdCurso);
                                insertCommand.Parameters.AddWithValue("@condicion", inscripcion.Condicion);
                                insertCommand.Parameters.AddWithValue("@nota", inscripcion.Nota);

                                int rowsAffected = insertCommand.ExecuteNonQuery();

                                // Actualizar el cupo
                                string updateCupoQuery = "UPDATE Cursos SET cupo = cupo - 1 WHERE id_curso = @id_curso";
                                using (SqlCommand updateCupoCommand = new SqlCommand(updateCupoQuery, connection))
                                {
                                    updateCupoCommand.Parameters.AddWithValue("@id_curso", inscripcion.IdCurso);
                                    updateCupoCommand.ExecuteNonQuery();
                                }

                                return rowsAffected > 0;
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }
        #endregion


        #region MODIFICAR INSCRIPCION
        public bool ModificarInscripcion(Inscripcion inscripcion)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();

                    string query = "UPDATE Alumnos_Inscripciones SET id_alumno = @NuevoIdAlumno, id_curso = @NuevoIdCurso, condicion = @NuevaCondicion,  nota = @NuevaNota WHERE id_inscripcion = @id_inscripcion";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id_inscripcion", inscripcion.IdInscripcion);
                        command.Parameters.AddWithValue("@NuevoIdAlumno", inscripcion.IdAlumno);
                        command.Parameters.AddWithValue("@NuevoIdCurso", inscripcion.IdCurso);
                        command.Parameters.AddWithValue("@NuevaCondicion", inscripcion.Condicion);
                        command.Parameters.AddWithValue("@NuevaNota", inscripcion.Nota);

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


        #region ELIMINAR INSCRIPCION
        public bool EliminarInscripcion(Inscripcion inscripcion)
        {
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                    {
                        connection.Open();
                        string query = @"DELETE FROM Alumnos_Inscripciones WHERE id_inscripcion = @id_inscripcion;
                                        UPDATE Cursos SET cupo = cupo + 1 WHERE id_curso = @id_curso;"; 
                                                        
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {   
                            command.Parameters.AddWithValue("@id_inscripcion", inscripcion.IdInscripcion);
                            command.Parameters.AddWithValue("@id_curso", inscripcion.IdCurso);

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


        #region OBTENER TODAS LAS INSCRIPCIONES
        public DataTable ObtenerTodasLasInscripciones()
        {
            DataTable dtInscripciones = new DataTable();

            using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
            {
                string query = "SELECT AI.id_inscripcion, AI.id_alumno, AI.id_curso, M.desc_materia AS Materia, CO.desc_comision AS Comision, AI.condicion,AI.nota, P.nombre AS Nombre, P.apellido AS Apellido FROM Alumnos_Inscripciones AI INNER JOIN Personas P ON P.id_persona = AI.id_alumno INNER JOIN Cursos C ON C.id_curso = AI.id_curso INNER JOIN Materias M ON M.id_materia = C.id_materia INNER JOIN Comisiones CO ON CO.id_comision=C.id_comision";


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dtInscripciones);
                }
            }
            return dtInscripciones;
        }
        #endregion


        #region OBTENER INSCRIPCIONES DE UN ALUMNO
        public DataTable ObtenerInscripcionesAlumno(int id)
        {
            DataTable dtInscripciones = new DataTable();

            using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
            {
                //string query = "SELECT id_inscripcion, id_alumno, id_curso, condicion,nota FROM Alumnos_Inscripciones WHERE id_alumno = @id";
                string query = "SELECT AI.id_inscripcion, AI.id_alumno, AI.id_curso, M.desc_materia AS Materia, CO.desc_comision AS Comision, AI.condicion,AI.nota, M.hs_semanales AS HorasSemanales, CO.anio_especialidad AS Año FROM Alumnos_Inscripciones AI INNER JOIN Cursos C ON C.id_curso = AI.id_curso INNER JOIN Materias M ON M.id_materia = C.id_materia INNER JOIN Comisiones CO ON CO.id_comision=C.id_comision WHERE AI.id_alumno = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@id", id);

                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dtInscripciones);
                }
            }
            return dtInscripciones;
        }
        #endregion


        #region BUSCAR INSCRIPCIONES
        public DataTable BusquedaFiltrada(int curso)
        {
            DataTable dtInscripcionesFiltradas = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "SELECT AI.id_inscripcion, AI.id_alumno, AI.id_curso, M.desc_materia AS Materia, CO.desc_comision AS Comision, AI.condicion,AI.nota, P.nombre AS Nombre, P.apellido AS Apellido FROM Alumnos_Inscripciones AI INNER JOIN Personas P ON P.id_persona = AI.id_alumno INNER JOIN Cursos C ON C.id_curso = AI.id_curso INNER JOIN Materias M ON M.id_materia = C.id_materia INNER JOIN Comisiones CO ON CO.id_comision=C.id_comision WHERE AI.id_curso = @curso";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@curso", curso);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dtInscripcionesFiltradas);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la búsqueda: " + ex.Message);
            }
            return dtInscripcionesFiltradas;
        }
        #endregion


        #region VALIDAR INSCRIPCIONES
        public int ValidaInscripcion(int id_usuario, int id_curso)
        {
            using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Alumnos_Inscripciones WHERE id_alumno = @id_usuario AND id_curso = @id_curso";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_usuario", id_usuario);
                    command.Parameters.AddWithValue("@id_curso", id_curso);

                    int count = (int)command.ExecuteScalar();

                    return count;
                }
            }
        }
        #endregion

    }
}