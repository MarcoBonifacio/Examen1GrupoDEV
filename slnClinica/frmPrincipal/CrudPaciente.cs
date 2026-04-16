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
    public partial class CrudPaciente : Form
    {
        public CrudPaciente()
        {
            InitializeComponent();
        }

        // Cadena de conexion a BDClinica
        private static string cadena = "Server=localhost;Database=BDClinica;Integrated Security=true; TrustServerCertificate=true";

        private void CrudPaciente_Load(object sender, EventArgs e)
        {
            listar();
        }

        private void listar()
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    SqlCommand comando = new SqlCommand("spListarPaciente", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adapter = new SqlDataAdapter(comando);
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);
                    dgvPaciente.DataSource = tabla;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar pacientes: " + ex.Message);
            }
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string CodPaciente = txtCodPaciente.Text.Trim();
            string APaterno = txtAPaterno.Text.Trim();
            string AMaterno = txtAMaterno.Text.Trim();
            string Nombres = txtNombres.Text.Trim();
            string DNI = txtDNI.Text.Trim();

            if (string.IsNullOrEmpty(CodPaciente) || string.IsNullOrEmpty(Nombres) || string.IsNullOrEmpty(DNI))
            {
                MessageBox.Show("Complete los campos obligatorios: Código, Nombres y DNI.");
                return;
            }

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    SqlCommand comando = new SqlCommand("spAgregarPaciente", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@CodPaciente", CodPaciente);
                    comando.Parameters.AddWithValue("@APaterno", APaterno);
                    comando.Parameters.AddWithValue("@AMaterno", AMaterno);
                    comando.Parameters.AddWithValue("@Nombres", Nombres);
                    comando.Parameters.AddWithValue("@DNI", DNI);

                    SqlDataAdapter adapter = new SqlDataAdapter(comando);
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);

                    if (tabla.Rows.Count > 0 && tabla.Columns.Contains("CodError"))
                    {
                        DataRow fila = tabla.Rows[0];
                        int CodError = Convert.ToInt32(fila["CodError"]);
                        string Mensaje = fila["Mensaje"].ToString();
                        if (CodError == 0) listar();
                        MessageBox.Show(Mensaje);
                    }
                    else
                    {
                        MessageBox.Show("Operación completada.");
                        listar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar paciente: " + ex.Message);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            string CodPaciente = txtCodPaciente.Text.Trim();
            string APaterno = txtAPaterno.Text.Trim();
            string AMaterno = txtAMaterno.Text.Trim();
            string Nombres = txtNombres.Text.Trim();
            string DNI = txtDNI.Text.Trim();

            if (string.IsNullOrEmpty(CodPaciente))
            {
                MessageBox.Show("Ingrese el código del paciente a actualizar.");
                return;
            }

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    SqlCommand comando = new SqlCommand("spActualizarPaciente", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@CodPaciente", CodPaciente);
                    comando.Parameters.AddWithValue("@APaterno", APaterno);
                    comando.Parameters.AddWithValue("@AMaterno", AMaterno);
                    comando.Parameters.AddWithValue("@Nombres", Nombres);
                    comando.Parameters.AddWithValue("@DNI", DNI);

                    SqlDataAdapter adapter = new SqlDataAdapter(comando);
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);

                    if (tabla.Rows.Count > 0 && tabla.Columns.Contains("CodError"))
                    {
                        DataRow fila = tabla.Rows[0];
                        int CodError = Convert.ToInt32(fila["CodError"]);
                        string Mensaje = fila["Mensaje"].ToString();
                        if (CodError == 0) listar();
                        MessageBox.Show(Mensaje);
                    }
                    else
                    {
                        MessageBox.Show("Operación completada.");
                        listar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar paciente: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string CodPaciente = txtCodPaciente.Text.Trim();
            if (string.IsNullOrEmpty(CodPaciente))
            {
                MessageBox.Show("Ingrese el código del paciente a eliminar.");
                return;
            }

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    SqlCommand comando = new SqlCommand("spEliminarPaciente", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@CodPaciente", CodPaciente);

                    SqlDataAdapter adapter = new SqlDataAdapter(comando);
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);

                    if (tabla.Rows.Count > 0 && tabla.Columns.Contains("CodError"))
                    {
                        DataRow fila = tabla.Rows[0];
                        int CodError = Convert.ToInt32(fila["CodError"]);
                        string Mensaje = fila["Mensaje"].ToString();
                        if (CodError == 0) listar();
                        MessageBox.Show(Mensaje);
                    }
                    else
                    {
                        MessageBox.Show("Operación completada.");
                        listar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar paciente: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string CodPaciente = txtBuscarP.Text.Trim();

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    if (string.IsNullOrEmpty(CodPaciente))
                    {
                        listar();
                        return;
                    }

                    SqlCommand comando = new SqlCommand("spBuscarPaciente", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@CodPaciente", CodPaciente);

                    SqlDataAdapter adapter = new SqlDataAdapter(comando);
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);

                    if (tabla.Rows.Count > 0 && tabla.Columns.Contains("CodError"))
                    {
                        DataRow fila = tabla.Rows[0];
                        int CodError = Convert.ToInt32(fila["CodError"]);
                        string Mensaje = fila["Mensaje"].ToString();
                        if (CodError == 0) listar();
                        MessageBox.Show(Mensaje);
                    }
                    else
                    {
                        dgvPaciente.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar paciente: " + ex.Message);
            }
        }
    }
}




