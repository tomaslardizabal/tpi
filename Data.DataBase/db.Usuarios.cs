using Entidades;
using System;
using System.Data.SqlClient;
using System.Data;


namespace Data.DataBase
{
    public class UsuariosDAO
    {
        #region ALTA USUARIO
        public bool InsertarUsuario(Usuario usuario)        // Método para insertar un nuevo usuario en la base de datos
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "INSERT INTO Usuarios (nombre_usuario, clave, id_persona) VALUES (@nombre_usuario, @clave, @id_persona)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre_usuario", usuario.NombreUsuario);
                        command.Parameters.AddWithValue("@clave", usuario.Clave);
                        command.Parameters.AddWithValue("@id_persona", usuario.IdPersona);
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


        #region MODIFICAR USUARIO
        public bool ModificarUsuario(Usuario usuario)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();

                    string query = "UPDATE Usuarios SET nombre_usuario = @NuevoNombre, clave = @NuevaClave WHERE id_usuario = @IDUsuario";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IDUsuario", usuario.IdUsuario);
                        command.Parameters.AddWithValue("@NuevoNombre", usuario.NombreUsuario);
                        command.Parameters.AddWithValue("@NuevaClave", usuario.Clave);

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


        #region ELIMINAR USUARIO
        public bool EliminarUsuario(Usuario usuario)
        {
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                    {
                        connection.Open();
                        string query = "DELETE FROM Usuarios WHERE id_usuario = @id_usuario AND NOT EXIST (SELECT 1 FROM Personas WHERE id_usuario = @id_usuario)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id_usuario", usuario.IdUsuario);

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


        #region OBTENER PLAN DEL USUARIO
        public int ObtenerPlan(int id_persona)
        {
            using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
            {
                string query = "SELECT p.id_plan FROM Usuarios u INNER JOIN Personas p ON u.id_persona = p.id_persona WHERE p.id_persona=@idPersona";


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@idPersona", id_persona);
                    
                    int result = (int)command.ExecuteScalar();
                    return result;

                }
            }
        }
        #endregion


        #region OBTENER TODOS LOS USUARIOS
        public DataTable ObtenerTodosLosUsuarios()
        {
            DataTable dtUsuarios = new DataTable();

            using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
            {
                string query = "SELECT U.id_usuario,U.nombre_usuario, U.clave, U.id_persona, CASE WHEN P.tipo_persona = 0 THEN 'Administrador' WHEN P.tipo_persona = 1 THEN 'Alumno' WHEN P.tipo_persona = 2 THEN 'Docente' END AS Tipo, P.apellido AS Apellido,P.email AS Email FROM Usuarios U INNER JOIN Personas P ON P.id_persona = U.id_persona";


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dtUsuarios);
                }
            }
            return dtUsuarios;
        }
        #endregion


        #region BUSCAR USUARIOS POR APELLIDO
        public DataTable BusquedaFiltrada(string apellido)
        {
            DataTable dtUsuariosFiltrados = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "SELECT U.id_usuario,U.nombre_usuario, U.clave, U.id_persona, CASE WHEN P.tipo_persona = 0 THEN 'Administrador' WHEN P.tipo_persona = 1 THEN 'Alumno' WHEN P.tipo_persona = 2 THEN 'Docente' END AS Tipo, P.apellido AS Apellido,P.email AS Email FROM Usuarios U INNER JOIN Personas P ON P.id_persona = U.id_persona WHERE  P.apellido = @apellido";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@apellido", apellido);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dtUsuariosFiltrados);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la búsqueda: " + ex.Message);
            }
            return dtUsuariosFiltrados;
        }
        #endregion


        #region VALIDAR QUE EXISTE USUARIO PARA LOGIN
        public string existeUsuario(string nombreUsuario, string clave)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "SELECT P.tipo_persona FROM Usuarios U INNER JOIN Personas P ON U.id_persona = P.id_persona WHERE U.nombre_usuario = @nombreUsuario AND U.clave = @clave";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                        command.Parameters.AddWithValue("@clave", clave);

                        object result = command.ExecuteScalar();

                        // Si result es diferente de null, significa que se encontró una coincidencia, devuelvo el tipo de usuario como string
                        if (result != null)
                        {
                            return result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la búsqueda: " + ex.Message);
            }

            // Si no se encontró ninguna coincidencia o si ocurrió una excepción, retornamos null
            return null;
        }
        #endregion


        #region OBTENER IDPERSONA DEL USUARIO LOGEADO
        public string ObtenerId(string nombreUsuario)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Adaptador.GetConnection()))
                {
                    connection.Open();
                    string query = "SELECT id_persona FROM Usuarios WHERE nombre_usuario = @nombreUsuario";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);

                        object result = command.ExecuteScalar();
                        return result.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar la búsqueda: " + ex.Message);
            }
            return null;
        }
        #endregion

    }
}