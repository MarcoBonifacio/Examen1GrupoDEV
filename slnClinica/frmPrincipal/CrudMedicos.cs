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
    public partial class CrudMedicos : Form
    {
        public CrudMedicos()
        {
            InitializeComponent();
        }

        // Cadena de conexion a BDClinica
        private static string cadena = "Server=localhost;Database=BDClinica;Integrated Security=true; TrustServerCertificate=true";

        private void CrudMedicos_Load(object sender, EventArgs e)
        {
            listar();
        }

        private void listar()
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM TMedico", conexion);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);
                    dgvMedico.DataSource = tabla;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar médicos: " + ex.Message);
            }
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string CodMedico = txtCodMedico.Text.Trim();
            string APaterno = txtAPaterno.Text.Trim();
            string AMaterno = txtAMaterno.Text.Trim();
            string Nombres = txtNombres.Text.Trim();
            string Especialidad = txtEspecialidad.Text.Trim();

            if (string.IsNullOrEmpty(CodMedico) || string.IsNullOrEmpty(Nombres))
            {
                MessageBox.Show("Complete los campos obligatorios: Código y Nombres.");
                return;
            }

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    // Verificar existencia
                    using (SqlCommand chk = new SqlCommand("SELECT COUNT(1) FROM TMedico WHERE CodMedico=@CodMedico", conexion))
                    {
                        chk.Parameters.AddWithValue("@CodMedico", CodMedico);
                        int existe = Convert.ToInt32(chk.ExecuteScalar());
                        if (existe > 0)
                        {
                            MessageBox.Show("Error: El CodMedico ya existe");
                            return;
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO TMedico (CodMedico, APaterno, AMaterno, Nombres, Especialidad) VALUES (@CodMedico,@APaterno,@AMaterno,@Nombres,@Especialidad)", conexion))
                    {
                        cmd.Parameters.AddWithValue("@CodMedico", CodMedico);
                        cmd.Parameters.AddWithValue("@APaterno", APaterno);
                        cmd.Parameters.AddWithValue("@AMaterno", AMaterno);
                        cmd.Parameters.AddWithValue("@Nombres", Nombres);
                        cmd.Parameters.AddWithValue("@Especialidad", Especialidad);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Médico agregado correctamente");
                            listar();
                        }
                        else MessageBox.Show("No se pudo agregar el médico.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar médico: " + ex.Message);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            string CodMedico = txtCodMedico.Text.Trim();
            string APaterno = txtAPaterno.Text.Trim();
            string AMaterno = txtAMaterno.Text.Trim();
            string Nombres = txtNombres.Text.Trim();
            string Especialidad = txtEspecialidad.Text.Trim();

            if (string.IsNullOrEmpty(CodMedico))
            {
                MessageBox.Show("Ingrese el código del médico a actualizar.");
                return;
            }

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    using (SqlCommand chk = new SqlCommand("SELECT COUNT(1) FROM TMedico WHERE CodMedico=@CodMedico", conexion))
                    {
                        chk.Parameters.AddWithValue("@CodMedico", CodMedico);
                        int existe = Convert.ToInt32(chk.ExecuteScalar());
                        if (existe == 0)
                        {
                            MessageBox.Show("Error: CodMedico no existe");
                            return;
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("UPDATE TMedico SET APaterno=@APaterno, AMaterno=@AMaterno, Nombres=@Nombres, Especialidad=@Especialidad WHERE CodMedico=@CodMedico", conexion))
                    {
                        cmd.Parameters.AddWithValue("@CodMedico", CodMedico);
                        cmd.Parameters.AddWithValue("@APaterno", APaterno);
                        cmd.Parameters.AddWithValue("@AMaterno", AMaterno);
                        cmd.Parameters.AddWithValue("@Nombres", Nombres);
                        cmd.Parameters.AddWithValue("@Especialidad", Especialidad);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Médico actualizado correctamente");
                            listar();
                        }
                        else MessageBox.Show("No se pudo actualizar el médico.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar médico: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string CodMedico = txtCodMedico.Text.Trim();
            if (string.IsNullOrEmpty(CodMedico))
            {
                MessageBox.Show("Ingrese el código del médico a eliminar.");
                return;
            }

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    using (SqlCommand chk = new SqlCommand("SELECT COUNT(1) FROM TCita WHERE CodMedico=@CodMedico", conexion))
                    {
                        chk.Parameters.AddWithValue("@CodMedico", CodMedico);
                        int citas = Convert.ToInt32(chk.ExecuteScalar());
                        if (citas > 0)
                        {
                            MessageBox.Show("Error: No se puede eliminar, el médico tiene citas pendientes");
                            return;
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("DELETE FROM TMedico WHERE CodMedico=@CodMedico", conexion))
                    {
                        cmd.Parameters.AddWithValue("@CodMedico", CodMedico);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Médico eliminado correctamente");
                            listar();
                        }
                        else MessageBox.Show("No se pudo eliminar el médico.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar médico: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = txtBuscar.Text.Trim();
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    SqlCommand cmd;
                    if (string.IsNullOrEmpty(criterio))
                    {
                        cmd = new SqlCommand("SELECT * FROM TMedico", conexion);
                    }
                    else
                    {
                        cmd = new SqlCommand("SELECT * FROM TMedico WHERE CodMedico = @c OR Nombres LIKE @like OR APaterno LIKE @like OR AMaterno LIKE @like OR Especialidad LIKE @like", conexion);
                        cmd.Parameters.AddWithValue("@c", criterio);
                        cmd.Parameters.AddWithValue("@like", "%" + criterio + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);
                    dgvMedico.DataSource = tabla;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar médicos: " + ex.Message);
            }
        }

        private void dgvMedico_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvMedico.Rows[e.RowIndex];
            txtCodMedico.Text = row.Cells["CodMedico"].Value?.ToString();
            txtAPaterno.Text = row.Cells["APaterno"].Value?.ToString();
            txtAMaterno.Text = row.Cells["AMaterno"].Value?.ToString();
            txtNombres.Text = row.Cells["Nombres"].Value?.ToString();
            txtEspecialidad.Text = row.Cells["Especialidad"].Value?.ToString();
        }
    }
}

