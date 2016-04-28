using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DMS
{
    public partial class frmDrugView : Form
    {
        DateTimePicker dtp;
        BindingSource bSource = new BindingSource();
        DataTable table = new DataTable();
        MySqlDataAdapter MyDA = new MySqlDataAdapter();

        public frmDrugView()
        {
            InitializeComponent();
        }

        private void frmDrugView_Load(object sender, EventArgs e)
        {
            loadDataTable();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           // MessageBox.Show(e.);

           MessageBox.Show(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
           

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            String drugId = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            String cellValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            String columnName = dataGridView1.Columns[e.ColumnIndex].HeaderText;

            MySqlConnection conn = null;


            try
            {
                conn = new MySqlConnection(DbManager.connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                String updateQuery = "UPDATE drugs SET " + columnName + " = @" + columnName + " WHERE iddrugs = @" + drugId;

                cmd.Connection = conn;
                cmd.CommandText = updateQuery;
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@" + columnName, cellValue);
                cmd.Parameters.AddWithValue("@" + drugId, drugId);
                cmd.ExecuteNonQuery();
               

            }
            catch (MySqlException ex)
            {
                //logger.Fatal(ex.ToString());
                Console.WriteLine("Error: {0}", ex.ToString());
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                if (conn != null)
                {
                   conn.Close();
                }

            }



            MessageBox.Show(drugId+cellValue+columnName);
        }

    
        private void loadDataTable(){

            MySqlConnection mysqlCon = new MySqlConnection(DbManager.connectionString);
            mysqlCon.Open();

            
            string sqlSelectAll = "SELECT * from drugs";
            MyDA.SelectCommand = new MySqlCommand(sqlSelectAll, mysqlCon);

           

            MyDA.Fill(table);

           
            bSource.DataSource = table;

            
            dataGridView1.DataSource = bSource;
            dataGridView1.Columns[0].ReadOnly = true;
        }

        private void dtp_OnTextChange(object sender, EventArgs e)
        {

            dataGridView1.CurrentCell.Value = dtp.Text.ToString();
        }
        void dtp_CloseUp(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
        } 
    }
}
