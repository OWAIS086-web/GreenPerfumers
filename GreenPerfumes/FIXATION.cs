using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace GreenPerfumes
{
    public partial class FIXATION : Form
    {
        public FIXATION()
        {
            InitializeComponent();
        }
        SqlCommand cmd = null;
        private void ShowSaleData(DataGridView dgv)
        {
             cmd = new SqlCommand("select * from SalesDetails where SalesRate = 0", MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dgv.DataSource = dt;

        }




        private void FIXATION_Load(object sender, EventArgs e)
        {
            ShowSaleData(dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainClass.con.Open();
            float salerate = 0;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                cmd = new SqlCommand("select SaleRate from Prices where Pcode = '" + item.Cells["Product_ID"].Value.ToString() + "' and Pro_Unit = 1 ", MainClass.con);
                salerate = float.Parse(cmd.ExecuteScalar().ToString());
                salerate = salerate / 1000;

                cmd = new SqlCommand("update SalesDetails set SalesRate = @SalesRate where CustomerInvoiceDetailsID = '" + item.Cells["CustomerInvoiceDetailsID"].Value.ToString() + "'", MainClass.con);
                cmd.Parameters.AddWithValue("@SalesRate", salerate);
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("DONE");
            MainClass.con.Close();
        }

    }
}
