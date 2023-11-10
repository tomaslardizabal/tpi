﻿using System;
using System.Data;
using System.Windows.Forms;
using Servicios;
using Entidades;

namespace UI.Escritorio
{
    public partial class formPlan : Form
    {
        private Plan planSeleccionado;

        public formPlan(int tipo)
        {
            InitializeComponent();
            ActualizarDataGridView();
            if (tipo != 0)
            {
                btnAlta.Visible = false;
                btnBaja.Visible = false;
                btnModifica.Visible = false;
            }
        }

        private void ActualizarDataGridView()
        {          
            S_Plan planesDao = new S_Plan();
            DataTable dtPlanes = planesDao.ObtenerTodosLosPlanes();

            DataColumn descripcionEspecialidadColumn = new DataColumn("Especialidad", typeof(string));
            dtPlanes.Columns.Add(descripcionEspecialidadColumn);

            S_Especialidad espDAO = new S_Especialidad();
            foreach (DataRow row in dtPlanes.Rows)
            {
                int idEspecialidad = Convert.ToInt32(row["id_Especialidad"]);
                string descripcionEspecialidad = espDAO.ObtenerDescripcionEspecialidad(idEspecialidad); 
                row["Especialidad"] = descripcionEspecialidad;
                
            }       
            dgvPlanes.AutoGenerateColumns = true;
            
            dgvPlanes.DataSource = dtPlanes;
             dgvPlanes.Columns["Especialidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void btnAlta_Click(object sender, EventArgs e)
        {                                         
            formPlanOpc frmPlanOp = new formPlanOpc();
            frmPlanOp.ShowDialog();
            ActualizarDataGridView();
        }
     
        private void btnModifica_Click(object sender, EventArgs e)
        {
            formPlanOpc frmPlanOp = new formPlanOpc(planSeleccionado);
            frmPlanOp.ShowDialog();
            ActualizarDataGridView();
        }


        private void btnBaja_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show($"Desea borrar {planSeleccionado.DescPlan}", "Confirmar Baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                S_Plan planesDAO = new S_Plan();
                bool eliminado = planesDAO.EliminarPlan(planSeleccionado);      //eliminado es mi variable bandera para saber si el metodo de eliminar funciono bien

                if (eliminado)
                {
                    MessageBox.Show("El plan ha sido eliminado correctamente.", "Eliminación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error al eliminar el plan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else
            {
                MessageBox.Show("Operación cancelada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            ActualizarDataGridView();
        }


        private void dgvPlanes_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            planSeleccionado = new Plan
            {
                IdPlan = int.Parse(dgvPlanes.CurrentRow.Cells[0].Value.ToString()),
                DescPlan = dgvPlanes.CurrentRow.Cells[1].Value.ToString(),
                IdEspecialidad = int.Parse(dgvPlanes.CurrentRow.Cells[2].Value.ToString())
            };
        }
    }
}