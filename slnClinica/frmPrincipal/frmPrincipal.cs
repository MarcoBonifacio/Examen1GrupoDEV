
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace frmPrincipal
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }
        // Declarar la cadena de conexion
        private static string cadena = "Server=localhost; Database=BDClinica; Integrated Security=true; TrustServerCertificate=True";

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            // Inicializaciones al cargar el formulario (si se necesitan)
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void medicosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrudMedicos medico = new CrudMedicos();
            medico.ShowDialog();
        }

        private void pacienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrudPaciente paciente = new CrudPaciente();
            paciente.ShowDialog();
        }

        private void citasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Citas citas = new Citas();
            citas.ShowDialog();
        }
    }
}
