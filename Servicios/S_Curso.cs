using Data.DataBase;
using Entidades;
using System;
using System.Data;


namespace Servicios
{
    public class S_Curso
    {
        CursosDAO CursosData = new CursosDAO();

        public DataTable ObtenerTodasLosCursos()
        {
            return CursosData.ObtenerTodasLosCursos();
        }
        public bool ModificarCurso(Curso curso)
        {
            return CursosData.ModificarCurso(curso);
        }
        public bool InsertarCursos(Curso curso)
        {
            return CursosData.InsertarCursos(curso);
        }
        public bool EliminarCurso(Curso curso)
        {
            return CursosData.EliminarCurso(curso);
        }

        public (int, int) ObtenerMateriayComision(int curso)
        {
            return CursosData.ObtenerMateriaYComisionPorCurso(curso);
        }

        public DataTable ObtenerComisionesPorMateria(int idMateria)
        {
            return CursosData.ObtenerComisionesPorMateria(idMateria);
        }
    }
}