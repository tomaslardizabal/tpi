using Data.DataBase;
using Entidades;
using System;
using System.Data;


namespace Servicios
{
    public class S_Inscripcion
    {
        InscripcionesDAO InscripcionesData = new InscripcionesDAO();

        public DataTable ObtenerTodasLasInscripciones()
        {
            return InscripcionesData.ObtenerTodasLasInscripciones();
        }
        public bool ModificarInscripcion(Inscripcion inscripcion)
        {
            return InscripcionesData.ModificarInscripcion(inscripcion);
        }
        public bool InsertarInscripcion(Inscripcion inscripcion)
        {
            return InscripcionesData.InsertarInscripcion(inscripcion);
        }
        public bool EliminarInscripcion(Inscripcion inscripcion)
        {
            return InscripcionesData.EliminarInscripcion(inscripcion);
        }

        public DataTable BusquedaFiltrada(int curso)
        {
            return InscripcionesData.BusquedaFiltrada(curso);
        }

        public DataTable ObtenerInscripcionesAlumno(int id)
        {
            return InscripcionesData.ObtenerInscripcionesAlumno(id);
        }
        public int ValidarInscripcion(int id_usuario, int id_curso)
        {
            return InscripcionesData.ValidaInscripcion(id_usuario, id_curso);
        }
        public int ObtenerIDCursoPorMateriaYComision(int id_materia, int id_comision)
        {
            return CursosDAO.ObtenerIDCurso(id_materia, id_comision);
        }
    }
}