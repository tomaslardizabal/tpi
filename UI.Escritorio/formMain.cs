using Entidades;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.Escritorio
{
    public partial class formMain : Form
    {
        int tipo_user;
        public formMain()
        {
            InitializeComponent();
        }

        private void formMain_Shown(object sender, EventArgs e) 
        {
            formLogin appLogin = new formLogin();         //esto es para ejecutar el login 
            if (appLogin.ShowDialog() != DialogResult.OK)
            {
                this.Dispose();
            }
            tipo_user = appLogin.TipoUsuario;
            // Según el rol del usuario autenticado, habilitar o deshabilitar opciones de menú
            switch (appLogin.TipoUsuario)
            {
                case 1:
                    usuariosMnu.Visible = false;
                    alumnosMnu.Visible = false;
                    docentesMnu.Visible = false;
                    comisionesMnu.Visible = true;
                    cursosMnu.Visible = true;
                    materiasMnu.Visible = true;
                    especialidadesMnu.Visible = true;   
                    planesMnu.Visible = true;
                    inscripcionesMnu.Visible = true;
                    notasMnu.Visible = false;
                    break;
                case 2:
                    usuariosMnu.Visible = false;
                    alumnosMnu.Visible = true;
                    docentesMnu.Visible = false;
                    comisionesMnu.Visible = true;
                    cursosMnu.Visible = true;
                    materiasMnu.Visible = false;
                    especialidadesMnu.Visible = false;
                    planesMnu.Visible = true;
                    inscripcionesMnu.Visible = false;
                    notasMnu.Visible = true;
                    break;
            }
        }

        #region USUARIOS
        private void usuariosMnu_Click(object sender, EventArgs e)
        {
            formUsuario frmUsuario = new formUsuario();
            frmUsuario.ShowDialog();
        }
        #endregion

        #region ALUMNOS
        private void alumnosMnu_Click(object sender, EventArgs e)
        {
            formPersona frmAlumno = new formPersona(1,tipo_user);
            frmAlumno.ShowDialog();
        }
        #endregion

        #region DOCENTES
        private void docentesMnu_Click(object sender, EventArgs e)
        {
            formPersona frmDocente = new formPersona(2,tipo_user);
            frmDocente.ShowDialog();
        }
        #endregion

        #region COMISIONES
        private void comisionesMnu_Click(object sender, EventArgs e)
        {
            formComision frmComision = new formComision(tipo_user);
            frmComision.ShowDialog();
        }
        #endregion

        #region CURSOS
        private void cursosMnu_Click(object sender, EventArgs e)
        {
            formCurso frmCurso = new formCurso(tipo_user);
            frmCurso.ShowDialog();
        }
        #endregion

        #region MATERIAS
        private void materiasMnu_Click(object sender, EventArgs e)
        {
            formMateria frmMateria = new formMateria(tipo_user);
            frmMateria.ShowDialog();
        }
        #endregion 

        #region ESPECIALIDADES
        private void especialidadesMnu_Click(object sender, EventArgs e)
        {
            formEspecialidad frmEspecialidad = new formEspecialidad(tipo_user);
            frmEspecialidad.ShowDialog();
        }
        #endregion

        #region PLANES
        private void planesMnu_Click(object sender, EventArgs e)
        {
            formPlan frmPlan = new formPlan(tipo_user);
            frmPlan.ShowDialog();
        }
        #endregion

        #region INSCRIPCIONES
        private void inscripcionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formInscripcion frmInscripcion  = new formInscripcion(tipo_user);
            frmInscripcion.ShowDialog();
        }
        #endregion

        #region NOTAS
        private void notasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formInscripcion frmInscripcion = new formInscripcion(tipo_user);
            frmInscripcion.ShowDialog();
        }
        #endregion
    }
}