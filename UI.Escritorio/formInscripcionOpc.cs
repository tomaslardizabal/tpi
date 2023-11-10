using Servicios;
using Entidades;
using System;
using System.Data;
using System.Windows.Forms;
using UI.Escritorio.TPI2023M07DataSet7TableAdapters;

namespace UI.Escritorio
{
    public partial class formInscripcionOpc : Form
    {
        Inscripcion inscripcionM;
        public formInscripcionOpc()     //constructor si la inscripcion es de alta
        {
            InitializeComponent();
         //   cargar_cursos();
            cargar_materias();
          //  cargar_comisiones();
            this.Text = "Formulario Alta";
            this.btnAceptar.Text = "AGREGAR";
            this.cmbCondicion.Text = "cursando";
            this.txtNota.Text = "0";
            cmbCondicion.Enabled = false;
            txtNota.Enabled = false;
        }
        public formInscripcionOpc(Inscripcion inscripcion)      //constructor si es de modificar
        {
            InitializeComponent();
            cargar_materias();
            S_Curso cursoDAO = new S_Curso();
            (int materia, int comision) = cursoDAO.ObtenerMateriayComision(inscripcion.IdCurso);

            S_Materia matDAO = new S_Materia();
            this.cmbMaterias.Text = matDAO.ObtenerDescripcionMateria(materia);
            S_Comision comDAO = new S_Comision();
            this.cmbComision.Text = comDAO.ObtenerDescripcionComision(comision);
            this.cmbCondicion.Text = inscripcion.Condicion;
            this.txtNota.Text = inscripcion.Nota.ToString();
            this.btnAceptar.Text = "MODIFICAR";
            this.Text = "Formulario Modificar";
            inscripcionM = inscripcion;
            this.cmbMaterias.Enabled = false;
            this.cmbComision.Enabled = false;
        }

        public void cargar_materias()
        {
            S_Materia materiasDAO = new S_Materia();
            S_Usuario usuarioDAO = new S_Usuario();
            DataTable dtMaterias = materiasDAO.ObtenerMateriasDePlan(usuarioDAO.ObtenerPlan(formLogin.id_usuario));
            cmbMaterias.ValueMember = "id_materia";
            cmbMaterias.DisplayMember = "desc_materia";
            cmbMaterias.DataSource = dtMaterias;
        }

        public void cargar_comisiones()
        {
            S_Comision comisionDAO = new S_Comision();
            DataTable dtComisiones = comisionDAO.ObtenerTodasLasComisiones();

            cmbComision.ValueMember = "id_comision";
            cmbComision.DisplayMember = "desc_comision";
            cmbComision.DataSource = dtComisiones;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (cmbMaterias.SelectedIndex < 0 || cmbComision.SelectedIndex < 0)
            {
                MessageBox.Show("Complete todos los campos antes de continuar.", "Campos Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                bool band;
                S_Inscripcion inscripcionDAO = new S_Inscripcion();

                if (btnAceptar.Text == "MODIFICAR")     
                {
                    inscripcionM.IdAlumno = inscripcionM.IdAlumno;
                    inscripcionM.IdCurso = inscripcionM.IdCurso;
                    inscripcionM.Condicion = cmbCondicion.SelectedItem as string;
                    inscripcionM.Nota = int.Parse(txtNota.Text);
                    band = inscripcionDAO.ModificarInscripcion(inscripcionM);
                }
                else
                {
                    int idCurso = inscripcionDAO.ObtenerIDCursoPorMateriaYComision((int)cmbMaterias.SelectedValue, (int)cmbComision.SelectedValue);
                    if (inscripcionDAO.ValidarInscripcion(formLogin.id_usuario, idCurso) == 0)
                    {
                        Inscripcion nuevaInscripcion = new Inscripcion
                        {
                            IdAlumno = formLogin.id_usuario,
                            IdCurso = idCurso,
                            Condicion = (string)cmbCondicion.Text,
                            Nota = int.Parse(txtNota.Text),
                        };
                        band = inscripcionDAO.InsertarInscripcion(nuevaInscripcion);           
                    }
                    else
                    {
                        band = false;
                    }
                }
                if (band)
                {
                    MessageBox.Show("La inscripcion se agregó correctamente");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error al cargar inscripcion, Inscripcion existente/Cupo lleno.");
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void cmbMaterias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMaterias.SelectedIndex >= 0)
            {
                // OBTENGO VALOR DEL ID MATERIA
                int idMateria = (int)cmbMaterias.SelectedValue;

                // LLAMO AL METODO PARA CARGAR COMISIONES DE ESA MATERIA
                CargarComisionesPorMateria(idMateria);
            }
        }

        private void CargarComisionesPorMateria(int idMateria)
        {
            S_Curso cursoDAO = new S_Curso();
            DataTable dtComisiones = cursoDAO.ObtenerComisionesPorMateria(idMateria);

            cmbComision.ValueMember = "id_comision";
            cmbComision.DisplayMember = "desc_comision";
            cmbComision.DataSource = dtComisiones;
        }
    }
}