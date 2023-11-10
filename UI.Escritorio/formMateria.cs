﻿using Servicios;
using Entidades;
using System;
using System.Data;
using System.Windows.Forms;

namespace UI.Escritorio
{
    public partial class formMateria : Form
    {
        private Materia materiaSeleccionada;
        public formMateria(int tipo)
        {
            InitializeComponent();
            ActualizarDataGridView();
            if (tipo != 0)
            {
                btnAlta.Visible = false;
                btnBaja.Visible = false;
                btnModificar.Visible = false;
            }
        }

        private void ActualizarDataGridView()
        {
            S_Materia materiasDAO = new S_Materia();
            DataTable dtMaterias = materiasDAO.ObtenerTodasLasMaterias();

            DataColumn descripcionPlanColumn = new DataColumn("Plan", typeof(string));
            dtMaterias.Columns.Add(descripcionPlanColumn);

            S_Plan planDAO = new S_Plan();
            foreach (DataRow row in dtMaterias.Rows)
            {
                int idPlan = Convert.ToInt32(row["id_plan"]);
                string descripcionPlan = planDAO.ObtenerDescripcionPlanes(idPlan);
                row["Plan"] = descripcionPlan;
            }
            dgvMaterias.AutoGenerateColumns = true;
            dgvMaterias.DataSource = dtMaterias;
        }

        private void btnAlta_Click(object sender, EventArgs e)
        {
            formMateriaOpc frmMateriaOp = new formMateriaOpc();
            if (DialogResult.OK == frmMateriaOp.ShowDialog())
            ActualizarDataGridView();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            formMateriaOpc frmMateriaOp = new formMateriaOpc(materiaSeleccionada);
            if (DialogResult.OK == frmMateriaOp.ShowDialog())
            ActualizarDataGridView();
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show($"Desea borrar {materiaSeleccionada.DescMateria}", "Confirmar Baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);         

            if (res == DialogResult.Yes)
            {
                S_Materia materiasDAO = new S_Materia();
                bool eliminado = materiasDAO.EliminarMateria(materiaSeleccionada);   //eliminado es mi variable bandera para saber si el metodo de eliminar funciono bien
                if (eliminado)
                {
                    MessageBox.Show("La materia ha sido eliminada correctamente.", "Eliminación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error al eliminar la materia.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else
            {
                MessageBox.Show("Operacion cancelada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            ActualizarDataGridView();
        }

        private void dgvMaterias_CellMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            materiaSeleccionada = new Materia
            {
                IdMateria = int.Parse(dgvMaterias.CurrentRow.Cells[0].Value.ToString()),
                DescMateria = dgvMaterias.CurrentRow.Cells[1].Value.ToString(),
                HsSemanales = int.Parse(dgvMaterias.CurrentRow.Cells[2].Value.ToString()),
                HsTotales = int.Parse(dgvMaterias.CurrentRow.Cells[3].Value.ToString()),
                IdPlan = int.Parse(dgvMaterias.CurrentRow.Cells[4].Value.ToString()),
            };
        }
    }
}