using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace frmPrincipal
{
    public partial class Citas : Form
    {
        public Citas()
        {
            InitializeComponent();
        }

        // Cadena de conexion a BDClinica
        private static string cadena = "Server=localhost;Database=BDClinica;Integrated Security=true; TrustServerCertificate=true";

        private void Citas_Load(object sender, EventArgs e)
        {
            CargarPacientes();
            CargarMedicos();
            listar();
        }

        private void CargarPacientes()
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("SELECT CodPaciente, (APaterno + ' ' + AMaterno + ' ' + Nombres) AS Nombre FROM TPaciente", conexion);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);
                    cmbPaciente.DisplayMember = "Nombre";
                    cmbPaciente.ValueMember = "CodPaciente";
                    cmbPaciente.DataSource = tabla;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar pacientes: " + ex.Message);
            }
        }

        private void CargarMedicos()
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("SELECT CodMedico, (APaterno + ' ' + AMaterno + ' ' + Nombres + ' - ' + Especialidad) AS Nombre FROM TMedico", conexion);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);
                    cmbMedico.DisplayMember = "Nombre";
                    cmbMedico.ValueMember = "CodMedico";
                    cmbMedico.DataSource = tabla;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar medicos: " + ex.Message);
            }
        }

        private void listar()
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    string sql = @"SELECT c.IdCita, c.CodPaciente, p.Nombres AS Paciente, c.CodMedico, m.Nombres AS Medico, c.FechaCita, c.Motivo
                                    FROM TCita c
                                    INNER JOIN TPaciente p ON c.CodPaciente = p.CodPaciente
                                    INNER JOIN TMedico m ON c.CodMedico = m.CodMedico
                                    ORDER BY c.FechaCita";
                    SqlCommand cmd = new SqlCommand(sql, conexion);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);
                    dgvCitas.DataSource = tabla;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar citas: " + ex.Message);
            }
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cmbPaciente.SelectedValue == null || cmbMedico.SelectedValue == null)
            {
                MessageBox.Show("Seleccione paciente y médico.");
                return;
            }

            string codP = cmbPaciente.SelectedValue.ToString();
            string codM = cmbMedico.SelectedValue.ToString();
            DateTime fecha = dtpFecha.Value;
            string motivo = txtMotivo.Text.Trim();

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO TCita (CodPaciente, CodMedico, FechaCita, Motivo) VALUES (@CodPaciente,@CodMedico,@FechaCita,@Motivo)", conexion);
                    cmd.Parameters.AddWithValue("@CodPaciente", codP);
                    cmd.Parameters.AddWithValue("@CodMedico", codM);
                    cmd.Parameters.AddWithValue("@FechaCita", fecha);
                    cmd.Parameters.AddWithValue("@Motivo", string.IsNullOrEmpty(motivo) ? (object)DBNull.Value : motivo);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Cita agregada correctamente");
                        listar();
                    }
                    else MessageBox.Show("No se pudo agregar la cita.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar cita: " + ex.Message);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtIdCita.Text, out int id))
            {
                MessageBox.Show("Seleccione una cita para actualizar (doble clic en la lista).");
                return;
            }

            if (cmbPaciente.SelectedValue == null || cmbMedico.SelectedValue == null)
            {
                MessageBox.Show("Seleccione paciente y médico.");
                return;
            }

            string codP = cmbPaciente.SelectedValue.ToString();
            string codM = cmbMedico.SelectedValue.ToString();
            DateTime fecha = dtpFecha.Value;
            string motivo = txtMotivo.Text.Trim();

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE TCita SET CodPaciente=@CodPaciente, CodMedico=@CodMedico, FechaCita=@FechaCita, Motivo=@Motivo WHERE IdCita=@IdCita", conexion);
                    cmd.Parameters.AddWithValue("@CodPaciente", codP);
                    cmd.Parameters.AddWithValue("@CodMedico", codM);
                    cmd.Parameters.AddWithValue("@FechaCita", fecha);
                    cmd.Parameters.AddWithValue("@Motivo", string.IsNullOrEmpty(motivo) ? (object)DBNull.Value : motivo);
                    cmd.Parameters.AddWithValue("@IdCita", id);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Cita actualizada correctamente");
                        listar();
                    }
                    else MessageBox.Show("No se pudo actualizar la cita.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar cita: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtIdCita.Text, out int id))
            {
                MessageBox.Show("Seleccione una cita para eliminar (doble clic en la lista).");
                return;
            }

            var confirm = MessageBox.Show("¿Eliminar la cita seleccionada?", "Confirmar", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM TCita WHERE IdCita=@IdCita", conexion);
                    cmd.Parameters.AddWithValue("@IdCita", id);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Cita eliminada correctamente");
                        listar();
                    }
                    else MessageBox.Show("No se pudo eliminar la cita.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar cita: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = txtBuscarC.Text.Trim();
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    string sql = @"SELECT c.IdCita, c.CodPaciente, p.Nombres AS Paciente, c.CodMedico, m.Nombres AS Medico, c.FechaCita, c.Motivo
                                    FROM TCita c
                                    INNER JOIN TPaciente p ON c.CodPaciente = p.CodPaciente
                                    INNER JOIN TMedico m ON c.CodMedico = m.CodMedico";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conexion;
                    if (string.IsNullOrEmpty(criterio))
                    {
                        cmd.CommandText = sql + " ORDER BY c.FechaCita";
                    }
                    else
                    {
                        cmd.CommandText = sql + " WHERE c.IdCita = @c OR c.CodPaciente = @c OR c.CodMedico = @c OR p.Nombres LIKE @like OR m.Nombres LIKE @like OR c.Motivo LIKE @like ORDER BY c.FechaCita";
                        if (int.TryParse(criterio, out int id)) cmd.Parameters.AddWithValue("@c", id);
                        else cmd.Parameters.AddWithValue("@c", criterio);
                        cmd.Parameters.AddWithValue("@like", "%" + criterio + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);
                    dgvCitas.DataSource = tabla;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar citas: " + ex.Message);
            }
        }

        private void dgvCitas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvCitas.Rows[e.RowIndex];
            txtIdCita.Text = row.Cells["IdCita"].Value?.ToString();
            string codP = row.Cells["CodPaciente"].Value?.ToString();
            string codM = row.Cells["CodMedico"].Value?.ToString();
            if (!string.IsNullOrEmpty(codP)) cmbPaciente.SelectedValue = codP;
            if (!string.IsNullOrEmpty(codM)) cmbMedico.SelectedValue = codM;
            if (DateTime.TryParse(row.Cells["FechaCita"].Value?.ToString(), out DateTime fecha)) dtpFecha.Value = fecha;
            txtMotivo.Text = row.Cells["Motivo"].Value?.ToString();
        }
    }
}
