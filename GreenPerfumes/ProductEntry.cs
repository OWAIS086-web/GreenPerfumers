using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenPerfumes
{
    public partial class ProductEntry : Form
    {
        int edit = 0;
        SqlCommand cmd;
        SqlDataReader dr;
        public ProductEntry()
        {
            InitializeComponent();
        }

        private void ShowProducts(DataGridView dgv, DataGridViewColumn PCODEGV, DataGridViewColumn ProductNameGV, DataGridViewColumn CategoryGV,DataGridViewColumn UrduName, string data = null)
        {
            SqlCommand cmd = null;
            try
            {
                MainClass.con.Open();
                if (data != null)
                {
                    cmd = new SqlCommand("select p.Pcode,p.ProductName,c.Category,p.UrduName from  Products as p inner join Categories c on c.CategoryID = p.CatID where p.ProductName like '%" + data + "%' and Extra = 0 order by p.ProductName asc	", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("select p.Pcode,p.ProductName,c.Category,p.UrduName from  Products as p inner join Categories c on c.CategoryID = p.CatID  where  Extra = 0 order by p.ProductName asc	", MainClass.con);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                PCODEGV.DataPropertyName = dt.Columns["Pcode"].ToString();
                ProductNameGV.DataPropertyName = dt.Columns["ProductName"].ToString();
                CategoryGV.DataPropertyName = dt.Columns["Category"].ToString();
                UrduName.DataPropertyName = dt.Columns["UrduName"].ToString();
                dgv.DataSource = dt;
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MessageBox.Show(ex.Message);
            }
        }


        private void ProductEntry_Load(object sender, EventArgs e)
        {
            ShowProducts(dataGridView1, PcodeGV, ProductNameGV, CategoryGV,ProductNameUrduGV);
            MainClass.FillCategories(cboCategory);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dashboard ds = new Dashboard();
            MainClass.showWindow(ds, this, MDI.ActiveForm);
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            MainClass.FillCategories(cboCategory);
        }

        private void btnaddcat_Click(object sender, EventArgs e)
        {
            Categories cs = new Categories();
            cs.ShowDialog();
        }


        private void Clear()
        {
            txtProductName.Text = "";
            cboCategory.SelectedIndex = 0;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (edit == 0)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (txtProductName.Text == row.Cells[1].Value.ToString() )
                    {
                        MessageBox.Show("Current Product Already Exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (txtProductName.Text == "" || cboCategory.SelectedIndex == 0)
                {
                    MessageBox.Show("Please Input Details");
                }

                else
                {
                    string catId = "";
                    MainClass.con.Open();
                    cmd = new SqlCommand("select CategoryID from Categories where Category like '" + cboCategory.Text + "'", MainClass.con);
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            catId = dr[0].ToString();
                        }
                    }
                    dr.Close();
                    MainClass.con.Close();

                 
                    MainClass.con.Open();
                    cmd = new SqlCommand("Insert into Products (ProductName,CatID,Extra,UrduName) values(@ProductName,@CatID,@Extra,@UrduName)", MainClass.con);
                    cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@CatID", catId);
                    cmd.Parameters.AddWithValue("@Extra", 0);
                    cmd.Parameters.AddWithValue("@UrduName", txtUrduName.Text);
                    cmd.ExecuteNonQuery();
                    MainClass.con.Close();
                    MessageBox.Show("Product Added Successfully");
                    Clear();
            ShowProducts(dataGridView1, PcodeGV, ProductNameGV, CategoryGV,ProductNameUrduGV);
                }
            }
            else
            {
                if(edit == 1)
                {
                    if (txtProductName.Text == "" || cboCategory.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please Input Details");
                    }
                    else
                    {
                        try
                        {
                            string  catId = "";
                            MainClass.con.Open();
                            cmd = new SqlCommand("select CategoryID from Categories where Category like '" + cboCategory.Text + "'", MainClass.con);
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    catId = dr[0].ToString();
                                }
                            }
                            dr.Close();
                            MainClass.con.Close();

                            

                            MainClass.con.Open();
                            cmd = new SqlCommand("update  Products set ProductName = @ProductName,CatID = @CatID, UrduName=@UrduName where Pcode = @Pcode ", MainClass.con);
                            cmd.Parameters.AddWithValue("@Pcode", lblcode.Text);
                            cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text);
                            cmd.Parameters.AddWithValue("@CatID", catId);
                            cmd.Parameters.AddWithValue("@UrduName", txtUrduName.Text);

                            cmd.ExecuteNonQuery();
                            MainClass.con.Close();
                            MessageBox.Show("Product Updated Successfully.");
                            Clear();
                    ShowProducts(dataGridView1, PcodeGV, ProductNameGV, CategoryGV,ProductNameUrduGV);
                            edit = 0;
                        }
                        catch (Exception ex)
                        {
                            MainClass.con.Close();
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }


        }





        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1 != null)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    if (dataGridView1.SelectedRows.Count == 1)
                    {
                        try
                        {
                            MainClass.con.Open();
                            SqlCommand cmd = new SqlCommand("delete from Products where Pcode = @Pcode", MainClass.con);
                            cmd.Parameters.AddWithValue("@Pcode", dataGridView1.CurrentRow.Cells[0].Value.ToString());
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Products Deleted Successfully");
                            MainClass.con.Close();
                    ShowProducts(dataGridView1, PcodeGV, ProductNameGV, CategoryGV,ProductNameUrduGV);
                        }
                        catch (Exception ex)
                        {
                            MainClass.con.Close();
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit = 1;
            lblcode.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtProductName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            cboCategory.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtUrduName.Text = dataGridView1.CurrentRow.Cells["ProductNameUrduGV"].Value.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (txtProductName.Text != "" || cboCategory.SelectedIndex != 0  )
            {
                Clear();
            }
            else
            {
                Dashboard ds = new Dashboard();
                MainClass.showWindow(ds, this, MDI.ActiveForm);
            }
        }


        private void dataGridView1_DataError_2(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
            {
                object value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (!((DataGridViewComboBoxColumn)dataGridView1.Columns[e.ColumnIndex]).Items.Contains(value))
                {
                    ((DataGridViewComboBoxColumn)dataGridView1.Columns[e.ColumnIndex]).Items.Add(value);
                    e.ThrowException = false;
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ShowProducts(dataGridView1, PcodeGV, ProductNameGV, CategoryGV,ProductNameUrduGV,txtSearch.Text);

        }
    }
}
