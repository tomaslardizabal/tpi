using Servicios;
using Entidades;
using System;
using System.Data;
using System.Windows.Forms;
using UI.Escritorio.Reportes;

namespace UI.Escritorio
{
    public partial class formInscripcion : Form
    {
        private Inscripcion inscripcionSeleccionada;
        int tipo_u;
        public formInscripcion(int tipo)
        {
            InitializeComponent();
            tipo_u = tipo;
            ActualizarDataGridView();
            if(tipo == 1)
            {
                btnModifica.Visible = false;
                lblBusqueda.Visible = false;
                txtCursoBusqueda.Visible = false;
                btnBuscar.Visible = false;
                btnReset.Visible = false;
                btnReporte.Visible = false;
            }
            else if (tipo == 2)
            {
                btnAlta.Visible = false;
                btnBaja.Visible = false;    
            }
        }

        private void ActualizarDataGridView()
        {
            S_Inscripcion inscripcionDAO = new S_Inscripcion();
            DataTable dtInscripciones;
            if (tipo_u == 1)
            {
                 dtInscripciones = inscripcionDAO.ObtenerInscripcionesAlumno(formLogin.id_usuario);
            }
            else
            {
                 dtInscripciones = inscripcionDAO.ObtenerTodasLasInscripciones();
            }

            dgvInscripciones.AutoGenerateColumns = true;
            dgvInscripciones.DataSource = dtInscripciones;      
        }


        private void btnAlta_Click(object sender, EventArgs e)
        {
            formInscripcionOpc frmInscripcionOp = new formInscripcionOpc();
            frmInscripcionOp.ShowDialog();
            ActualizarDataGridView();
        }

        private void btnModifica_Click(object sender, EventArgs e)
        {
            formInscripcionOpc frmInscripcionOp = new formInscripcionOpc(inscripcionSeleccionada);
            frmInscripcionOp.ShowDialog();
            ActualizarDataGridView();
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            DialogResult res = new DialogResult();
            res = MessageBox.Show($"Desea borrar {inscripcionSeleccionada.IdInscripcion}", "Confirmar Baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                S_Inscripcion inscripcionDAO = new S_Inscripcion();
                bool eliminado = inscripcionDAO.EliminarInscripcion(inscripcionSeleccionada);   //eliminado es mi variable bandera para saber si el metodo de eliminar funciono bien
                if (eliminado)
                {
                    MessageBox.Show("La inscripcion ha sido eliminada correctamente.", "Eliminación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error al eliminar la inscripcion.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else
            {
                MessageBox.Show("Operacion cancelada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            ActualizarDataGridView();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            dgvInscripciones.DataSource = null;
            ActualizarDataGridView();
            dgvInscripciones.Columns["id_inscripcion"].HeaderText = "ID Inscripcion";
            dgvInscripciones.Columns["id_alumno"].HeaderText = "ID Alumno";
            dgvInscripciones.Columns["id_curso"].HeaderText = "ID Curso";
            dgvInscripciones.Columns["condicion"].HeaderText = "Condicion";
            dgvInscripciones.Columns["nota"].HeaderText = "Nota";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if(txtCursoBusqueda.Text == "")
            {
                MessageBox.Show("Ingrese un curso");
            }
            else
            {
                int cursoBusqueda = int.Parse(txtCursoBusqueda.Text);
                S_Inscripcion insDAO = new S_Inscripcion();
                DataTable dtBusqueda = insDAO.BusquedaFiltrada(cursoBusqueda);

                dgvInscripciones.AutoGenerateColumns = true;
                dgvInscripciones.DataSource = dtBusqueda;
            }                    
        }

        private void dgvInscripciones_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            inscripcionSeleccionada = new Inscripcion
            {
                IdInscripcion = int.Parse(dgvInscripciones.CurrentRow.Cells[0].Value.ToString()),
                IdAlumno = int.Parse(dgvInscripciones.CurrentRow.Cells[1].Value.ToString()),
                IdCurso = int.Parse(dgvInscripciones.CurrentRow.Cells[2].Value.ToString()),
                Condicion = dgvInscripciones.CurrentRow.Cells[5].Value.ToString(), 
                Nota = int.Parse(dgvInscripciones.CurrentRow.Cells[6].Value.ToString()),
            };
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtCursoBusqueda.Text))
            {
                MessageBox.Show("Ingrese un id de curso.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Reportes.formReporteNotas frmReporteNotas = new Reportes.formReporteNotas();
                frmReporteNotas.txtIdCurso_Reporte.Text = txtCursoBusqueda.Text;
                frmReporteNotas.ShowDialog();
            }
            
        }
    }
}