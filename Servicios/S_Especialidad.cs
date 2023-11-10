using Data.DataBase;
using Entidades;
using System;
using System.Data;


namespace Servicios
{
    public class S_Especialidad
    {
        EspecialidadesDAO EspecialidadData = new EspecialidadesDAO();

        public DataTable ObtenerTodasLasEspecialidades()
        {
            return EspecialidadData.ObtenerTodasLasEspecialidades();
        }
        public string ObtenerDescripcionEspecialidad(int idEspecialidad)
        {
            return EspecialidadData.ObtenerDescripcionEspecialidad(idEspecialidad);
        }
        public bool ModificarEspecialidad(Especialidad especialidad)
        {
            return EspecialidadData.ModificarEspecialidad(especialidad);
        }
        public bool InsertarEspecialidad(Especialidad especialidad)
        {
            return EspecialidadData.InsertarEspecialidad(especialidad);
        }
        public bool EliminarEspecialidad(Especialidad especialidad)
        {
            return EspecialidadData.EliminarEspecialidad(especialidad);
        }
    }
}