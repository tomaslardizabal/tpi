using Servicios;
using Entidades;
using System;
using System.Data;
using System.Windows.Forms;

namespace UI.Escritorio
{
    public partial class formUsuario : Form
    {
        private Usuario usuarioSeleccionado;

        public formUsuario()
        {
            InitializeComponent();
            ActualizarDataGridView();
        }

        private void ActualizarDataGridView()
        {
            S_Usuario usuarioDAO = new S_Usuario();
            DataTable dtUsuarios = usuarioDAO.ObtenerTodosLosUsuarios();
            dgvUsuarios.AutoGenerateColumns = true;
            dgvUsuarios.DataSource = dtUsuarios;
        }


        private void btnAlta_Click(object sender, EventArgs e)
        {
            formPersonaOpc frmPersona = new formPersonaOpc();
            frmPersona.ShowDialog();
            ActualizarDataGridView();
        }

        private void btnModifica_Click(object sender, EventArgs e)
        {
            formUsuarioOpc frmUsuarioOp = new formUsuarioOpc(usuarioSeleccionado);
            if (DialogResult.OK == frmUsuarioOp.ShowDialog())
            ActualizarDataGridView();
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            {
                DialogResult res = MessageBox.Show($"Desea borrar {usuarioSeleccionado.NombreUsuario}", "Confirmar Baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res == DialogResult.Yes)
                {
                    S_Usuario usuarioDAO = new S_Usuario();
                    bool eliminado = usuarioDAO.EliminarUsuario(usuarioSeleccionado);   //eliminado es mi variable bandera para saber si el metodo de eliminar funciono bien

                    if (eliminado)
                    {
                        MessageBox.Show("Se elimino correctamente.", "Eliminación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar, hay una persona asociada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                else
                {
                    MessageBox.Show("Debe seleccionar un usuario antes de realizar la baja.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                ActualizarDataGridView();
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            usuarioSeleccionado = new Usuario
            {
                IdUsuario= int.Parse(dgvUsuarios.CurrentRow.Cells[0].Value.ToString()),
                NombreUsuario = dgvUsuarios.CurrentRow.Cells[1].Value.ToString(),
                Clave = dgvUsuarios.CurrentRow.Cells[2].Value.ToString(),
                IdPersona = int.Parse(dgvUsuarios.CurrentRow.Cells[3].Value.ToString()),          
            };
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            dgvUsuarios.DataSource = null;
            ActualizarDataGridView();
            dgvUsuarios.Columns["id_usuario"].HeaderText = "ID Usuario";
            dgvUsuarios.Columns["nombre_usuario"].HeaderText = "Usuario";
            dgvUsuarios.Columns["clave"].HeaderText = "Clave";
            dgvUsuarios.Columns["id_persona"].HeaderText = "ID Persona";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string apellidoBusqueda = txtApellidoBusca.Text;
            S_Usuario usuarioDAO = new S_Usuario();
            DataTable dtBusqueda = usuarioDAO.BusquedaFiltrada(apellidoBusqueda);          
            dgvUsuarios.DataSource = dtBusqueda;
        }
    }
}