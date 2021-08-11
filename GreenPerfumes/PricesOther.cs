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
    public partial class PricesOther : Form
    {
        SqlDataReader dr;
        int edit = 0;
        public PricesOther()
        {
            InitializeComponent();
        }

        private void LoadProducts(DataGridView dgv, DataGridViewColumn NameGV, DataGridViewColumn Pcode, DataGridViewColumn UnitGVC, DataGridViewColumn PurRateGVC, DataGridViewColumn SaleRateGVC, DataGridViewColumn IDGVC)
        {

            try
            {
                MainClass.con.Open();
                SqlCommand cmd = new SqlCommand("select pr.ID ,p.ProductName, p.Pcode,u.UnitName, pr.PurchaseRate,pr.SaleRate from PricesOther pr inner join Products p on p.Pcode = pr.Pcode inner join Units u on u.UnitID = pr.Pro_UnitO  group by p.ProductName, p.Pcode,u.UnitName,pr.PurchaseRate,pr.SaleRate,pr.ID", MainClass.con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                IDGVC.DataPropertyName = dt.Columns["ID"].ToString();
                NameGV.DataPropertyName = dt.Columns["ProductName"].ToString();
                Pcode.DataPropertyName = dt.Columns["Pcode"].ToString();
                UnitGVC.DataPropertyName = dt.Columns["UnitName"].ToString();
                PurRateGVC.DataPropertyName = dt.Columns["PurchaseRate"].ToString();
                SaleRateGVC.DataPropertyName = dt.Columns["SaleRate"].ToString();
                dgv.DataSource = dt;
                MainClass.con.Close();


            }

            catch (Exception ex)
            {
                MainClass.con.Close();
                MessageBox.Show(ex.Message);
                LoadProducts(dgvpricing, ProNameGV, PcodeGVG, UnitGV, PurchaseRateGV, SaleRateGV, IDName);
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dashboard ds = new Dashboard();
            MainClass.showWindow(ds, this, MDI.ActiveForm);
        }

        private void PricesOther_Load(object sender, EventArgs e)
        {
            LoadProducts(dgvpricing, ProNameGV, PcodeGVG, UnitGV, PurchaseRateGV, SaleRateGV, IDName);
            MainClass.FillProducts2(cboProduct);
            MainClass.FillUnits(cboUnits);
        }

        private void Clear()
        {
            cboProduct.SelectedIndex = 0;
            cboUnits.SelectedIndex = 0;
            txtPurchaseRAte.Text = "";
            txtSaleRate.Text = "";
            cboProduct.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (edit == 0)
            {
                foreach (DataGridViewRow item in dgvpricing.Rows)
                {
                    if (cboProduct.Text == dgvpricing.CurrentRow.Cells[1].Value.ToString() && cboUnits.Text == dgvpricing.CurrentRow.Cells[2].Value.ToString())
                    {
                        MessageBox.Show("Current Product Already Exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (cboProduct.SelectedIndex == 0 || cboUnits.SelectedIndex == 0 || txtPurchaseRAte.Text == "" || txtSaleRate.Text == "")
                {
                    MessageBox.Show("Please Input Details");
                }


                try
                {
                    string unitid = "", proID = "";
                    MainClass.con.Open();
                    SqlCommand cmd = new SqlCommand("select UnitID from Units where UnitName like '" + cboUnits.Text + "'", MainClass.con);
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
                    cmd = new SqlCommand("select Pcode from Products where ProductName like '" + cboProduct.Text + "'", MainClass.con);
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            proID = dr[0].ToString();
                        }
                    }
                    dr.Close();
                    MainClass.con.Close();

                    MainClass.con.Open();
                    SqlCommand cmd1 = new SqlCommand("insert into PricesOther (Pcode,Pro_UnitO,PurchaseRate,SaleRate) values(@Pcode,@Pro_UnitO,@PurchaseRate,@SaleRate)", MainClass.con);
                    cmd1.Parameters.AddWithValue("@Pcode", proID);
                    cmd1.Parameters.AddWithValue("@Pro_UnitO", unitid);
                    cmd1.Parameters.AddWithValue("@PurchaseRate", txtPurchaseRAte.Text);
                    cmd1.Parameters.AddWithValue("@SaleRate", txtSaleRate.Text);
                    cmd1.ExecuteNonQuery();
                    MainClass.con.Close();

                    MessageBox.Show("Price Added Successfully");
                    Clear();
                    LoadProducts(dgvpricing, ProNameGV, PcodeGVG, UnitGV, PurchaseRateGV, SaleRateGV, IDName);



                }
                catch (Exception ex)
                {
                    MainClass.con.Close();
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                if (edit == 1)
                {
                    foreach (DataGridViewRow item in dgvpricing.Rows)
                    {
                        if (cboProduct.Text == dgvpricing.CurrentRow.Cells[1].Value.ToString() && cboUnits.Text == dgvpricing.CurrentRow.Cells[2].Value.ToString())
                        {
                            MessageBox.Show("Current Product Already Exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    if (cboProduct.SelectedIndex == 0 || cboUnits.SelectedIndex == 0 || txtPurchaseRAte.Text == "" || txtSaleRate.Text == "")
                    {
                        MessageBox.Show("Please Input Details");
                    }
                    else
                    {
                        try
                        {
                            string unitid = "", proID = "";
                            MainClass.con.Open();
                            SqlCommand cmd = new SqlCommand("select UnitID from Units where UnitName like '" + cboUnits.Text + "'", MainClass.con);
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
                            cmd = new SqlCommand("select Pcode from Products where ProductName like '" + cboProduct.Text + "'", MainClass.con);
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    proID = dr[0].ToString();
                                }
                            }
                            dr.Close();
                            MainClass.con.Close();



                            MainClass.con.Open();
                            cmd = new SqlCommand("update  PricesOther set Pcode = @Pcode, Pro_UnitO = @Pro_UnitO, PurchaseRate = @PurchaseRate, SaleRate = @SaleRate where ID = @ID", MainClass.con);
                            cmd.Parameters.AddWithValue("@ID", lblID2.Text);
                            cmd.Parameters.AddWithValue("@Pcode", proID);
                            cmd.Parameters.AddWithValue("@Pro_UnitO", unitid);
                            cmd.Parameters.AddWithValue("@PurchaseRate", txtPurchaseRAte.Text);
                            cmd.Parameters.AddWithValue("@SaleRate", txtSaleRate.Text);

                            cmd.ExecuteNonQuery();
                            MainClass.con.Close();
                            MessageBox.Show("Product Updated Successfully.");
                            Clear();
                            LoadProducts(dgvpricing, ProNameGV, PcodeGVG, UnitGV, PurchaseRateGV, SaleRateGV, IDName);
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

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit = 1;
            lblID2.Text = dgvpricing.CurrentRow.Cells[0].Value.ToString();
            cboProduct.Text = dgvpricing.CurrentRow.Cells[1].Value.ToString();
            lblID.Text = dgvpricing.CurrentRow.Cells[2].Value.ToString();
            cboUnits.Text = dgvpricing.CurrentRow.Cells[3].Value.ToString();
            txtPurchaseRAte.Text = dgvpricing.CurrentRow.Cells[4].Value.ToString();
            txtSaleRate.Text = dgvpricing.CurrentRow.Cells[5].Value.ToString();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvpricing != null)
            {
                if (dgvpricing.Rows.Count > 0)
                {
                    if (dgvpricing.SelectedRows.Count == 1)
                    {
                        try
                        {
                            MainClass.con.Open();
                            SqlCommand cmd = new SqlCommand("delete from PricesOther where ID = @ID", MainClass.con);
                            cmd.Parameters.AddWithValue("@ID", dgvpricing.CurrentRow.Cells[0].Value.ToString());
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Products Deleted Successfully");
                            MainClass.con.Close();
                            LoadProducts(dgvpricing, ProNameGV, PcodeGVG, UnitGV, PurchaseRateGV, SaleRateGV, IDName);
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (cboProduct.SelectedIndex != 0 || cboUnits.SelectedIndex != 0 || txtPurchaseRAte.Text == "" || txtSaleRate.Text == "")
            {
                cboProduct.SelectedIndex = 0;
                cboUnits.SelectedIndex = 0;
                txtPurchaseRAte.Text = "";
                txtSaleRate.Text = "";
            }
            else
            {
                Dashboard ds = new Dashboard();
                MainClass.showWindow(ds, this, MDI.ActiveForm);
            }
        }
    }
}
