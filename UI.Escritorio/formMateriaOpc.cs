﻿using Entidades;
using Servicios;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace UI.Escritorio
{
    public partial class formMateriaOpc : Form
    {
        Materia materiaN;

        public formMateriaOpc()
        {
            InitializeComponent();
            cargar_planes();
            this.Text = "Formulario ALTA materia";
            this.btnAceptar.Text = "AGREGAR";
        }
        public formMateriaOpc(Materia materia)
        {
            InitializeComponent();
            cargar_planes();

            this.Text = "Formulario MODIFICAR materia";
            this.textBoxDescripMateria.Text = materia.DescMateria.ToString();
            this.textBoxHsSem.Text = materia.HsSemanales.ToString();
            S_Plan planDAO = new S_Plan();
            this.comboBoxDescPlan.Text = planDAO.ObtenerDescripcionPlanes(materia.IdPlan);
            this.btnAceptar.Text = "MODIFICAR";
            materiaN = materia;
        }
            
        

        public void cargar_planes()
        {
            S_Plan planesDAO = new S_Plan();
            DataTable dtPlanes = planesDAO.ObtenerTodosLosPlanes();

            comboBoxDescPlan.ValueMember = "id_plan";
            comboBoxDescPlan.DisplayMember = "desc_plan";
            comboBoxDescPlan.DataSource = dtPlanes;
        }


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (comboBoxDescPlan.SelectedIndex < 0 || textBoxDescripMateria.Text.Length == 0 || textBoxHsSem.Text.Length == 0)
            {
                MessageBox.Show("Complete todos los campos antes de continuar.", "Campos Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!textBoxDescripMateria.Text.All(char.IsLetter) || !int.TryParse(textBoxHsSem.Text, out _))
            {
                MessageBox.Show("Formato incorrecto de campos", "Formato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                bool band;
                S_Materia materiasDAO = new S_Materia();
                if (this.btnAceptar.Text == "MODIFICAR")
                {
                    materiaN.DescMateria = textBoxDescripMateria.Text;
                    materiaN.HsSemanales = int.Parse(textBoxHsSem.Text);
                    materiaN.IdPlan = (int)comboBoxDescPlan.SelectedValue;
                    materiaN.HsTotales = (materiaN.HsSemanales * 40);           // 4 semanas en 10 meses son 40 semanas
                    band = materiasDAO.ModificarMateria(materiaN);
                }
                else
                {
                    Materia materia = new Materia
                    {
                        DescMateria = textBoxDescripMateria.Text,
                        HsSemanales = int.Parse(textBoxHsSem.Text),
                        IdPlan = (int)comboBoxDescPlan.SelectedValue,
                        HsTotales = (int.Parse(textBoxHsSem.Text) * 40),
                    };
                    band = materiasDAO.AgregarMateria(materia);
                }

                if (band)
                {
                    MessageBox.Show("La materia se agrego correctamente");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error al cargar materia");
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}