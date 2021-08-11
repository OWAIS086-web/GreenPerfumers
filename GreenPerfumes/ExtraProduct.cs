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
    public partial class ExtraProduct : Form
    {
        int edit = 0;
        public ExtraProduct()
        {
            InitializeComponent();
        }

        private void ShowExtraProducts(DataGridView dgv, DataGridViewColumn Pcode, DataGridViewColumn ProductName,
            DataGridViewColumn Unit, DataGridViewColumn PurchaseRate, DataGridViewColumn SaleRate, DataGridViewColumn InHandQTY)
        {
            SqlCommand cmd = null;
            MainClass.con.Open();
            cmd = new SqlCommand("select ep.Pcode,ep.ProductName,u.UnitName,ep.PurchaseRate,ep.SaleRate,ep.InHandQty from ExtraProducts ep inner join Units u on u.UnitID = ep.UnitID", MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Pcode.DataPropertyName = dt.Columns["Pcode"].ToString();
            ProductName.DataPropertyName = dt.Columns["ProductName"].ToString();
            Unit.DataPropertyName = dt.Columns["UnitName"].ToString();
            PurchaseRate.DataPropertyName = dt.Columns["PurchaseRate"].ToString();
            SaleRate.DataPropertyName = dt.Columns["SaleRate"].ToString();
            InHandQTY.DataPropertyName = dt.Columns["InHandQty"].ToString();
            dgv.DataSource = dt;
            MainClass.con.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Clear()
        {
            txtProductName.Text = "";
            txtPurchaseRAte.Text = "";
            txtSaleRate.Text = "";
            txtinHand.Text = "";
            cboCategory.SelectedIndex = 0;
            cboUnit.SelectedIndex = 0;
            lblID.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            SqlCommand cmd = null;
            SqlDataReader dr;
            if (edit == 0)
            {
                foreach (DataGridViewRow row in dgvExtra.Rows)
                {
                    if (txtProductName.Text == row.Cells[1].Value.ToString())
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
                    string unitID = "";
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
                    cmd = new SqlCommand("select UnitID from Units where UnitName like '" + cboUnit.Text + "'", MainClass.con);
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            unitID = dr[0].ToString();
                        }
                    }
                    dr.Close();
                    MainClass.con.Close();


                    MainClass.con.Open();
                    cmd = new SqlCommand("Insert into ExtraProducts (ProductName,UnitID,PurchaseRate,SaleRate,InHandQty) values(@ProductName,@UnitID,@PurchaseRate,@SaleRate,@InHandQty)", MainClass.con);
                    cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@UnitID", unitID);
                    cmd.Parameters.AddWithValue("@PurchaseRate", float.Parse(txtPurchaseRAte.Text));
                    cmd.Parameters.AddWithValue("@SaleRate", float.Parse(txtSaleRate.Text));
                    cmd.Parameters.AddWithValue("@InHandQty", float.Parse(txtinHand.Text));
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("Insert into Products (ProductName,CatID,Extra) values(@ProductName,@CatID,@Extra)", MainClass.con);
                    cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@CatID", catId);
                    cmd.Parameters.AddWithValue("@Extra", 1);
                    cmd.ExecuteNonQuery();
                    MainClass.con.Close();
                    MessageBox.Show("Product Added Successfully");
                    Clear();
                    ShowExtraProducts(dgvExtra, PcodeGV, ProductNameGV, UnitID, PurchaseRateGV, SaleRateGV, InHandQTyGV); 
                }
            }
            else
            {
                if (edit == 1)
                {
                    if (txtProductName.Text == "" || cboCategory.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please Input Details");
                    }
                    else
                    {
                        try
                        {
                            string catId = "";
                            string unitid = "";
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
                            cmd = new SqlCommand("select UnitID from Units where UnitName like '" + cboUnit.Text + "'", MainClass.con);
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    unitid = dr[0].ToString();
                                }
                            }
                            dr.Close();
                            MainClass.con.Close();



                            MainClass.con.Open();
                            cmd = new SqlCommand("update  ExtraProducts set ProductName = @ProductName, UnitID = @UnitID, PurchaseRate = @PurchaseRate, SaleRate = @SaleRate,InHandQty = @InHandQty where Pcode = @Pcode", MainClass.con);
                            cmd.Parameters.AddWithValue("@Pcode", lblID.Text);
                            cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text);
                            cmd.Parameters.AddWithValue("@UnitID", unitid);
                            cmd.Parameters.AddWithValue("@PurchaseRate", float.Parse(txtPurchaseRAte.Text));
                            cmd.Parameters.AddWithValue("@SaleRate", float.Parse(txtSaleRate.Text));
                            cmd.Parameters.AddWithValue("@InHandQty", float.Parse(txtinHand.Text));

                            cmd.ExecuteNonQuery();
                            MainClass.con.Close();
                            MessageBox.Show("Product Updated Successfully.");
                            Clear();
                            ShowExtraProducts(dgvExtra, PcodeGV, ProductNameGV, UnitID, PurchaseRateGV, SaleRateGV, InHandQTyGV);
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

        private void Delete0Ones()
        {
            MainClass.con.Open();
            if (dgvExtra.Rows.Count > 0)
            {
                foreach (DataGridViewRow item in dgvExtra.Rows)
                {
                    if (int.Parse(item.Cells[5].Value.ToString()) == 0)
                    {
                        SqlCommand cmd = new SqlCommand("delete from ExtraProducts where Pcode = '" + item.Cells[0].Value.ToString() + "'", MainClass.con);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            MainClass.con.Close();
            ShowExtraProducts(dgvExtra, PcodeGV, ProductNameGV, UnitID, PurchaseRateGV, SaleRateGV, InHandQTyGV);

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit = 1;
            lblID.Text = dgvExtra.CurrentRow.Cells[0].Value.ToString();
            txtProductName.Text = dgvExtra.CurrentRow.Cells[1].Value.ToString();
            cboCategory.Text = dgvExtra.CurrentRow.Cells[2].Value.ToString();
            cboUnit.Text = dgvExtra.CurrentRow.Cells[3].Value.ToString();
            txtPurchaseRAte.Text = dgvExtra.CurrentRow.Cells[4].Value.ToString();
            txtSaleRate.Text = dgvExtra.CurrentRow.Cells[5].Value.ToString();
            txtinHand.Text = dgvExtra.CurrentRow.Cells[6].Value.ToString();

        }

        private void ExtraProduct_Load(object sender, EventArgs e)
        {
            ShowExtraProducts(dgvExtra, PcodeGV, ProductNameGV, UnitID, PurchaseRateGV, SaleRateGV, InHandQTyGV);
            Delete0Ones();
            MainClass.FillUnits(cboUnit);MainClass.FillCategories(cboCategory);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvExtra != null)
            {
                if (dgvExtra.Rows.Count > 0)
                {
                    if (dgvExtra.SelectedRows.Count == 1)
                    {
                        try
                        {
                            MainClass.con.Open();
                            SqlCommand cmd = new SqlCommand("delete from ExtraProducts where Pcode = @Pcode", MainClass.con);
                            cmd.Parameters.AddWithValue("@Pcode", dgvExtra.CurrentRow.Cells[0].Value.ToString());
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Products Deleted Successfully");
                            MainClass.con.Close();
                            ShowExtraProducts(dgvExtra, PcodeGV, ProductNameGV, UnitID, PurchaseRateGV, SaleRateGV, InHandQTyGV);
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
    }
}
