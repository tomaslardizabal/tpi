﻿using Servicios;
using Entidades;
using System;
using System.Data;
using System.Windows.Forms;

namespace UI.Escritorio
{
    public partial class formComision : Form
    {
        private Comision comisionSeleccionada;

        public formComision(int tipo)
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
            S_Comision comisionesDAO = new S_Comision();
            DataTable dtComisiones = comisionesDAO.ObtenerTodasLasComisiones();

            DataColumn descripcionPlanColumn = new DataColumn("Plan", typeof(string));
            dtComisiones.Columns.Add(descripcionPlanColumn);

            S_Plan planDAO = new S_Plan();
            foreach (DataRow row in dtComisiones.Rows)
            {
                int idPlan = Convert.ToInt32(row["id_plan"]);
                string descripcionPlan = planDAO.ObtenerDescripcionPlanes(idPlan);
                row["Plan"] = descripcionPlan;
            }
            dgvComisiones.AutoGenerateColumns = true;
            dgvComisiones.DataSource = dtComisiones;
            dgvComisiones.Columns["Plan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void btnAlta_Click(object sender, EventArgs e)
        {
            formComisionOpc frmComisionOp = new formComisionOpc();
            frmComisionOp.ShowDialog();
            ActualizarDataGridView();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            formComisionOpc frmComisionOp = new formComisionOpc(comisionSeleccionada);
            frmComisionOp.ShowDialog();
            ActualizarDataGridView();
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show($"Desea borrar {comisionSeleccionada.DescComision}", "Confirmar Baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                S_Comision comisionesDAO = new S_Comision();
                bool eliminado = comisionesDAO.EliminarComision(comisionSeleccionada);   //eliminado es mi variable bandera para saber si el metodo de eliminar funciono bien
                if (eliminado)                        
                {
                    MessageBox.Show("La comisión ha sido eliminada correctamente.", "Eliminación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error al eliminar la comisión.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else
            {
                MessageBox.Show("Operacion cancelada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            ActualizarDataGridView();
        }

        private void dgvComisiones_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            comisionSeleccionada = new Comision
            {
                IdComision = int.Parse(dgvComisiones.CurrentRow.Cells[0].Value.ToString()),
                DescComision = dgvComisiones.CurrentRow.Cells[1].Value.ToString(),
                AnioEspecialidad = int.Parse(dgvComisiones.CurrentRow.Cells[2].Value.ToString()),
                IdPlan = int.Parse(dgvComisiones.CurrentRow.Cells[3].Value.ToString())
            };
        }
    }                                   
}