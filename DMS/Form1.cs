using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using log4net;
using log4net.Config;


namespace DMS
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static readonly ILog logger =
          LogManager.GetLogger(typeof(Form1));

        private void button1_Click(object sender, EventArgs e)
        {

            DOMConfigurator.Configure();

            logger.Debug("Here is a debug log.");
            logger.Info("... and an Info log.");
            logger.Warn("... and a warning.");
            logger.Error("... and an error.");
            logger.Fatal("... and a fatal error.");

          
            MySqlConnection conn = null;
          

            try
            {
                conn = new MySqlConnection(DbManager.connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO drugs(drug_name,amount) VALUES(@Name,@Amount)";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@Name", "Trygve Gulbranssen");
                cmd.Parameters.AddWithValue("@Amount", 2);
                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                logger.Fatal(ex.ToString());
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MySqlConnection mysqlCon = new

       MySqlConnection(DbManager.connectionString);
            mysqlCon.Open();

            MySqlDataAdapter MyDA = new MySqlDataAdapter();
            string sqlSelectAll = "SELECT * from drugs";
            MyDA.SelectCommand = new MySqlCommand(sqlSelectAll, mysqlCon);

            DataTable table = new DataTable();
            
            MyDA.Fill(table);

            BindingSource bSource = new BindingSource();
            bSource.DataSource = table;


            dataGridView1.DataSource = bSource;
        }

        private void button3_Click(object sender, EventArgs e)
        {
             new frmDrugView().Show();
        }
    }
}
