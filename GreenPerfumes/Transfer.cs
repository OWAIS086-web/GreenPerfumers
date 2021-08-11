using System;
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
    public partial class Transfer : Form
    {
        bool productcheck = false;
        int rowindex = 0;
        public Transfer()
        {
            InitializeComponent();
        }
        private void Transfer_Load(object sender, EventArgs e)
        {
            MainClass.FillWarehouses(cboFromWarehouse);
            MainClass.FillWarehouses(cbotoWarehouse);
            dateTimePicker1.Value = DateTime.Now;
        }

        private void ShowStocks(DataGridView dgv, DataGridViewColumn pname, DataGridViewColumn unit, DataGridViewColumn quantity, string data = null)
        {
            SqlCommand cmd;
            try
            {
                MainClass.con.Open();
                if (data == "" || data == null)
                {
                    cmd = new SqlCommand("select p.ProductName,u.UnitName as 'Unit',s.st_Qty as 'Quantity'  from Stocks s inner join Units u on u.UnitID = s.st_Unit inner join Products p on p.Pcode = s.st_Pcode where s.st_Qty > 0 group by p.ProductName,u.UnitName,s.st_Qty ", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("select p.ProductName,u.UnitName as 'Unit',s.st_Qty as 'Quantity' from Stocks s inner join Units u on u.UnitID = s.st_Unit inner join Products p on p.Pcode = s.st_Pcode where p.ProductName  like '%" + data + "%' and s.st_Qty > 0 group by p.ProductName,u.UnitName,s.st_Qty  ", MainClass.con);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                pname.DataPropertyName = dt.Columns["ProductName"].ToString();
                quantity.DataPropertyName = dt.Columns["Quantity"].ToString();
                unit.DataPropertyName = dt.Columns["Unit"].ToString();
                dgv.DataSource = dt;


                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MessageBox.Show(ex.Message);
            }

        }

        private void ShowStocks2(DataGridView dgv, DataGridViewColumn pname, DataGridViewColumn unit, DataGridViewColumn quantity,  string data = null)
        {
            SqlCommand cmd;
            try
            {
                MainClass.con.Open();
                if (data == "" || data == null)
                {
                    cmd = new SqlCommand("select p.ProductName,u.UnitName as 'Unit',s.sh_Qty as 'Quantity' from ShopStocks s inner join Units u on u.UnitID = s.sh_Unit inner join Products p on p.Pcode = s.sh_Pcode where s.sh_Qty > 0 ", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("select p.ProductName,u.UnitName as 'Unit',s.sh_Qty as 'Quantity' from ShopStocks s inner join Units u on u.UnitID = s.sh_Unit inner join Products p on p.Pcode = s.sh_Pcode where p.ProductName  like '%" + data + "%' and s.sh_Qty > 0   ", MainClass.con);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                pname.DataPropertyName = dt.Columns["ProductName"].ToString();
                quantity.DataPropertyName = dt.Columns["Quantity"].ToString();
                unit.DataPropertyName = dt.Columns["Unit"].ToString();
                dgv.DataSource = dt;

                MainClass.con.Close();

            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MessageBox.Show(ex.Message);
            }

        }

        private void cboFromWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                if (cboFromWarehouse.SelectedValue.ToString() == "2")
                {
                    ShowStocks(dgvWarehouse, PoductNameGVC, UnitGVC, QtyGVC, txtSearch.Text);
                }

                if (cboFromWarehouse.SelectedValue.ToString() == "3")
                {
                    ShowStocks2(dgvWarehouse, PoductNameGVC, UnitGVC, QtyGVC, txtSearch.Text);
                }
           

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dashboard ds = new Dashboard();
            MainClass.showWindow(ds, this, MDI.ActiveForm);
        }



       

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (cboFromWarehouse.SelectedValue.ToString() == "2")
            {
                ShowStocks(dgvWarehouse, PoductNameGVC, UnitGVC, QtyGVC,  txtSearch.Text);
            }

            if (cboFromWarehouse.SelectedValue.ToString() == "3")
            {
                ShowStocks2(dgvWarehouse, PoductNameGVC, UnitGVC, QtyGVC,  txtSearch.Text);
            }


        }

        

        private void dgvWarehouse_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                txtProductName.Text = dgvWarehouse.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtUnit.Text = dgvWarehouse.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtQuantity.Text = dgvWarehouse.Rows[e.RowIndex].Cells[2].Value.ToString();

            }

        }

        private void ClearF()
        {
            txtProductName.Text = "";
            txtQuantity.Text = "";
            txtUnit.Text = "";
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            if (txtUnit.Text == "" || txtUnit.Text == "0")
            {
                MessageBox.Show("Please Enter Quantity");
                return;
            }
            else
            {


                float quantity;
                float.TryParse(txtQuantity.Text.Trim(), out quantity);
                if (DgvToTransfer.Rows.Count == 0)
                {
                    DataGridViewRow createrow = new DataGridViewRow();
                    createrow.CreateCells(DgvToTransfer);
                    createrow.Cells[0].Value = txtProductName.Text;
                    createrow.Cells[1].Value = txtUnit.Text;
                    createrow.Cells[2].Value = txtQuantity.Text;
                    DgvToTransfer.Rows.Add(createrow);
                }
                else
                {
                    foreach (DataGridViewRow check in DgvToTransfer.Rows)
                    {
                        if (Convert.ToString(check.Cells[0].Value) == Convert.ToString(txtProductName.Text) &&
                            Convert.ToString(check.Cells[2].Value) == Convert.ToString(txtUnit.Text))
                        {
                            productcheck = true;
                        }

                    }
                    if (productcheck == true)
                    {
                        foreach (DataGridViewRow row in DgvToTransfer.Rows)
                        {
                            if (Convert.ToString(row.Cells[0].Value) == Convert.ToString(txtProductName.Text))
                            {
                                row.Cells["QtyGVC"].Value = float.Parse(row.Cells["QtyGVC"].Value.ToString()) + quantity;
                                ClearF();
                            }
                        }
                    }
                    else
                    {
                        DataGridViewRow createrow = new DataGridViewRow();
                        createrow.CreateCells(DgvToTransfer);
                        createrow.Cells[0].Value = txtProductName.Text;
                        createrow.Cells[1].Value = txtUnit.Text;
                        createrow.Cells[2].Value = txtQuantity.Text;
                        DgvToTransfer.Rows.Add(createrow);
                    }
                }
                MainClass.con.Open();
                SqlCommand cmd = new SqlCommand("select pr.PurchaseRate from Prices pr inner join Products p on p.Pcode = pr.Pcode inner join Stocks s on s.st_Pcode = pr.Pcode where p.ProductName = '" + txtProductName.Text + "' ", MainClass.con);
                float rate = float.Parse(cmd.ExecuteScalar().ToString());
                MainClass.con.Close();
                if (txtQuantity.Text != "0" || txtQuantity.Text != "")
                {
                    if (txtUnit.Text == "Grams")
                    {
                        txtTransferTotal.Text = Convert.ToString(rate * float.Parse(txtQuantity.Text) / 1000);
                    }
                    else
                    {
                        txtTransferTotal.Text = Convert.ToString(rate * float.Parse(txtQuantity.Text));
                    }
                }

            }
        }

        private void DgvToTransfer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (e.ColumnIndex == 3)
                {
                    if (!this.DgvToTransfer.Rows[this.rowindex].IsNewRow)
                    {
                        this.DgvToTransfer.Rows.RemoveAt(this.rowindex);
                    }
                }
            }
           // ClearComplete();
        }

        private void cbKg_CheckedChanged(object sender, EventArgs e)
        {
            if (cbKg.Checked == true)
            {
                foreach (DataGridViewRow item in dgvWarehouse.Rows)
                {
                    if (item.Cells[1].Value.ToString() == "Grams")
                    {
                        item.Cells[1].Value = "Kg";
                        item.Cells[2].Value = (float.Parse(item.Cells[2].Value.ToString())*1.0 / 1000).ToString();
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow item in dgvWarehouse.Rows)
                {
                    if (item.Cells[1].Value.ToString() == "Kg")
                    {
                        item.Cells[1].Value = "Grams";
                        item.Cells[2].Value = (float.Parse(item.Cells[2].Value.ToString()) * 1000).ToString();
                    }
                }

            }
        }

        private void cbKg2_CheckedChanged(object sender, EventArgs e)
        {

            if (cbKg2.Checked == true)
            {
                if (txtUnit.Text == "Grams")
                {
                    txtUnit.Text = "Kg";
                    txtQuantity.Text =  (float.Parse(txtQuantity.Text) / 1000).ToString();
                }
                
            }
            else
            {
                if (txtUnit.Text == "Kg")
                {
                    txtUnit.Text = "Grams";
                    if (txtQuantity.Text != "")
                    {
                        txtQuantity.Text = (float.Parse(txtQuantity.Text) * 1000).ToString();
                    }
                    else
                    {
                        txtQuantity.Text = "0";
                    }
                }


            }
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
               SqlCommand cmd;
               SqlCommand cmd1;

               MainClass.con.Open();
            try
            {

                //Updating Stocks
                if (cboFromWarehouse.SelectedValue.ToString() == "2" && cbotoWarehouse.SelectedValue.ToString() == "3")
                {

                    if (txtUnit.Text == "Grams" || txtUnit.Text == "Kg")
                    {
                        foreach (DataGridViewRow dataGrid in DgvToTransfer.Rows)
                        {


                            //getting Quantity
                            object pcode;
                            cmd = new SqlCommand("select Pcode from Products  where ProductName like  '" + txtProductName.Text + "'",MainClass.con);
                            pcode = cmd.ExecuteScalar();

                            float q;
                           cmd = new SqlCommand("select s.st_Qty from Stocks s where s.st_Pcode = '"+pcode+"'  and s.st_Unit = '3' ", MainClass.con);
                            object ob = cmd.ExecuteScalar();
                            q = float.Parse(ob.ToString());



                            //gettin pcode
                            cmd = new SqlCommand("select p.Pcode from Products p where p.ProductName = '" + txtProductName.Text + "'", MainClass.con);
                            Int32 code = Convert.ToInt32(cmd.ExecuteScalar());


                            //getting unit for shop
                            cmd = new SqlCommand("select s.st_Unit from Stocks s where s.st_Unit = (select u.UnitID from Units u where u.UnitName = 'Grams' )", MainClass.con);
                            Int32 unit1 = Convert.ToInt32(cmd.ExecuteScalar());

                            cmd = new SqlCommand("update Stocks  set st_Qty = @st_Qty where st_Pcode = @st_Pcode and st_Unit = @st_Unit", MainClass.con);
                            if (Convert.ToString(dataGrid.Cells[1].Value) == "Kg")
                            {
                                dataGrid.Cells[2].Value = float.Parse(dataGrid.Cells[2].Value.ToString()) * 1000;
                                q -= float.Parse(dataGrid.Cells[2].Value.ToString());
                            }
                            else
                            {
                                q -= float.Parse(dataGrid.Cells[2].Value.ToString());
                            }
                            cmd.Parameters.AddWithValue("@st_Qty", q);
                            cmd.Parameters.AddWithValue("@st_Pcode", code);
                            cmd.Parameters.AddWithValue("@st_Unit", unit1);
                            cmd.ExecuteNonQuery();





                            //Checking Product Exists
                            foreach (DataGridViewRow item in DgvToTransfer.Rows)
                            {
                                //    float q1;
                                string CheckStock = "select s.sh_Qty as 'Quantity' from ShopStocks s where s.sh_Pcode = '" + code + "'and s.sh_Unit =  (select u.UnitID from Units u where u.UnitName = 'Grams') ";
                                cmd = new SqlCommand(CheckStock, MainClass.con);
                                object ob1 = cmd.ExecuteScalar();


                                //Updating Products
                                if (ob1 != null)
                                {
                                    cmd1 = new SqlCommand("update ShopStocks  set sh_Qty = @sh_Qty where sh_Pcode = @sh_Pcode and sh_Unit = @sh_Unit", MainClass.con);
                                    q += float.Parse(dataGrid.Cells[2].Value.ToString());
                                    cmd1.Parameters.AddWithValue("@sh_Qty", q);
                                    cmd1.Parameters.AddWithValue("@sh_Pcode", code);
                                    cmd1.Parameters.AddWithValue("@sh_Unit", unit1);
                                    cmd1.ExecuteNonQuery();

                                    cmd = new SqlCommand("insert into Transfers (Pcode,TransferQty,FromWarehouseID,ToWarehouseID,TransferUnit,TransferTotal,Date) values (@Pcode,@TransferQty,@FromWarehouseID,@ToWarehouseID,@TransferUnit,@TransferTotal,@Date)", MainClass.con);
                                    cmd.Parameters.AddWithValue("@Pcode", code);
                                    cmd.Parameters.AddWithValue("@TransferQty", dataGrid.Cells[2].Value);
                                    cmd.Parameters.AddWithValue("@FromWarehouseID", cboFromWarehouse.SelectedValue);
                                    cmd.Parameters.AddWithValue("@ToWarehouseID", cbotoWarehouse.SelectedValue);
                                    cmd.Parameters.AddWithValue("@TransferUnit", unit1);
                                    cmd.Parameters.AddWithValue("@TransferTotal", txtTransferTotal.Text);
                                    cmd.Parameters.AddWithValue("@Date", dateTimePicker1.Value.ToShortDateString());
                                    cmd.ExecuteNonQuery();
                                }

                                //Inserting Products
                                else
                                {
                                    foreach (DataGridViewRow products1 in DgvToTransfer.Rows)
                                    {

                                        //gettin pcode
                                        cmd = new SqlCommand("select p.Pcode from Products p where p.ProductName = '" + txtProductName.Text + "'", MainClass.con);
                                        Int32 shopcode = Convert.ToInt32(cmd.ExecuteScalar());

                                        //getting unitcode
                                        cmd = new SqlCommand("select u.UnitID from Units u where u.UnitName = 'Grams'", MainClass.con);
                                        Int32 shopunit = Convert.ToInt32(cmd.ExecuteScalar());




                                        // bool okay = false;
                                        string InsertStocks = "insert into ShopStocks (sh_Pcode,sh_Qty,sh_Unit) values (@sh_Pcode,@sh_Qty,@sh_Unit)";
                                        SqlCommand cmd4 = new SqlCommand(InsertStocks, MainClass.con);

                                        cmd4.Parameters.AddWithValue("@sh_Pcode", shopcode);
                                        cmd4.Parameters.AddWithValue("@sh_Qty", dataGrid.Cells[2].Value);
                                        cmd4.Parameters.AddWithValue("@sh_Unit", shopunit);
                                        cmd4.ExecuteNonQuery();

                                        cmd = new SqlCommand("insert into Transfers (Pcode,TransferQty,FromWarehouseID,ToWarehouseID,TransferUnit,TransferTotal,Date) values (@Pcode,@TransferQty,@FromWarehouseID,@ToWarehouseID,@TransferUnit,@TransferTotal,@Date)", MainClass.con);
                                        cmd.Parameters.AddWithValue("@Pcode", shopcode);
                                        cmd.Parameters.AddWithValue("@TransferQty", dataGrid.Cells[2].Value);
                                        cmd.Parameters.AddWithValue("@FromWarehouseID", cboFromWarehouse.SelectedValue);
                                        cmd.Parameters.AddWithValue("@ToWarehouseID", cbotoWarehouse.SelectedValue);
                                        cmd.Parameters.AddWithValue("@TransferUnit", shopunit);
                                        cmd.Parameters.AddWithValue("@TransferTotal", txtTransferTotal.Text);
                                        cmd.Parameters.AddWithValue("@Date", dateTimePicker1.Value.ToShortDateString());
                                        cmd.ExecuteNonQuery();

                                    }

                                }
                            }
                        }
                    }


                    else
                    {

                        //Checking for Exisiting Products
                        foreach (DataGridViewRow item1 in DgvToTransfer.Rows)
                        {

                            //getting Quantity
                            float q;
                            cmd = new SqlCommand("select s.st_Qty from Stocks s where s.st_Pcode = (select p.Pcode from Products p where p.ProductName = '" + txtProductName.Text + "') and s.st_Unit =  (select u.UnitID from Units u where u.UnitName = '" + txtUnit.Text + "' )", MainClass.con);
                            object ob = cmd.ExecuteScalar();
                            q = float.Parse(ob.ToString());



                            //getting Pcode
                            cmd = new SqlCommand("select p.Pcode from Products p where p.ProductName = '" + txtProductName.Text + "'", MainClass.con);
                            Int32 pcode1 = Convert.ToInt32(cmd.ExecuteScalar());

                            //getting unit
                            cmd = new SqlCommand("select s.st_Unit from Stocks s where s.st_Unit = (select u.UnitID from Units u where u.UnitName = '" + txtUnit.Text + "' )", MainClass.con);
                            Int32 unitto = Convert.ToInt32(cmd.ExecuteScalar());

                            cmd = new SqlCommand("update Stocks  set st_Qty = @st_Qty where st_Pcode = @st_Pcode and st_Unit = @st_Unit", MainClass.con);
                            q -= float.Parse(item1.Cells[2].Value.ToString());
                            cmd.Parameters.AddWithValue("@st_Qty", q);
                            cmd.Parameters.AddWithValue("@st_Pcode", pcode1);
                            cmd.Parameters.AddWithValue("@st_Unit", unitto);
                            cmd.ExecuteNonQuery();


                            //     float q1;
                            string CheckStock = "select s.sh_Qty as 'Quantity' from ShopStocks s where s.sh_Pcode = '" + pcode1 + "'and s.sh_Unit = '" + unitto + "' ";
                            cmd = new SqlCommand(CheckStock, MainClass.con);
                            object ob2 = cmd.ExecuteScalar();


                            if (ob2 != null)
                            {
                               

                                //getting unit
                                cmd = new SqlCommand("select s.sh_Unit from ShopStocks s where s.sh_Unit = (select u.UnitID from Units u where u.UnitName = '" + txtUnit.Text + "' )", MainClass.con);
                                Int32 unitt1o = Convert.ToInt32(cmd.ExecuteScalar());


                                cmd1 = new SqlCommand("update ShopStocks  set sh_Qty = @sh_Qty where sh_Pcode = @sh_Pcode and sh_Unit = @sh_Unit", MainClass.con);
                                q += float.Parse(txtQuantity.Text);
                                cmd1.Parameters.AddWithValue("@sh_Qty", q);
                                cmd1.Parameters.AddWithValue("@sh_Pcode", pcode1);
                                cmd1.Parameters.AddWithValue("@sh_Unit", unitt1o);
                                cmd1.ExecuteNonQuery();


                                cmd = new SqlCommand("insert into Transfers (Pcode,TransferQty,FromWarehouseID,ToWarehouseID,TransferUnit,TransferTotal,Date) values (@Pcode,@TransferQty,@FromWarehouseID,@ToWarehouseID,@TransferUnit,@TransferTotal,@Date)", MainClass.con);
                                cmd.Parameters.AddWithValue("@Pcode", pcode1);
                                cmd.Parameters.AddWithValue("@TransferQty", q);
                                cmd.Parameters.AddWithValue("@FromWarehouseID", cboFromWarehouse.SelectedValue);
                                cmd.Parameters.AddWithValue("@ToWarehouseID", cbotoWarehouse.SelectedValue);
                                cmd.Parameters.AddWithValue("@TransferUnit", unitt1o);
                                cmd.Parameters.AddWithValue("@TransferTotal", txtTransferTotal.Text);
                                cmd.Parameters.AddWithValue("@Date", dateTimePicker1.Value.ToShortDateString());
                                cmd.ExecuteNonQuery();

                            }
                            else
                            {
                                //gettin pcode
                                cmd = new SqlCommand("select p.Pcode from Products p where p.ProductName = '" + txtProductName.Text + "'", MainClass.con);
                                Int32 shopcode = Convert.ToInt32(cmd.ExecuteScalar());

                                //getting unitcode
                                cmd = new SqlCommand("select u.UnitID from Units u where u.UnitName = '" + item1.Cells[1].Value.ToString() + "'", MainClass.con);
                                Int32 shopunit = Convert.ToInt32(cmd.ExecuteScalar());



                                string InsertStocks = "insert into ShopStocks (sh_Pcode,sh_Qty,sh_Unit) values (@sh_Pcode,@sh_Qty,@sh_Unit)";
                                SqlCommand cmd4 = new SqlCommand(InsertStocks, MainClass.con);
                                cmd4.Parameters.AddWithValue("@sh_Pcode", shopcode);
                                cmd4.Parameters.AddWithValue("@sh_Qty", item1.Cells[2].Value);
                                cmd4.Parameters.AddWithValue("@sh_Unit", shopunit);
                                //   cmd4.Parameters.AddWithValue("@sh_PurchaseRate", item1.Cells[3].Value);
                                cmd4.ExecuteNonQuery();


                                cmd = new SqlCommand("insert into Transfers (Pcode,TransferQty,FromWarehouseID,ToWarehouseID,TransferUnit,TransferTotal,Date) values (@Pcode,@TransferQty,@FromWarehouseID,@ToWarehouseID,@TransferUnit,@TransferTotal,@Date)", MainClass.con);
                                cmd.Parameters.AddWithValue("@Pcode", shopcode);
                                cmd.Parameters.AddWithValue("@TransferQty", item1.Cells[2].Value);
                                cmd.Parameters.AddWithValue("@FromWarehouseID", cboFromWarehouse.SelectedValue);
                                cmd.Parameters.AddWithValue("@ToWarehouseID", cbotoWarehouse.SelectedValue);
                                cmd.Parameters.AddWithValue("@TransferUnit", shopunit);
                                cmd.Parameters.AddWithValue("@TransferTotal", txtTransferTotal.Text);
                                cmd.Parameters.AddWithValue("@Date", dateTimePicker1.Value.ToShortDateString());
                                cmd.ExecuteNonQuery();

                            }
                        }
                    }
                }

                MessageBox.Show("Transfer Completed");
                ClearComplete();
                addbtn.Enabled = true;



            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MessageBox.Show(ex.Message);
            }

         MainClass.con.Close();

        }

        private void ClearComplete()
        {
            cboFromWarehouse.SelectedIndex = 0;
            cbotoWarehouse.SelectedIndex = 0;
            txtProductName.Text = "";
            txtQuantity.Text = "";
            txtSearch.Text = "";
            txtTransferTotal.Text = "";
            txtUnit.Text = "";
            DgvToTransfer.Rows.Clear();
        }

        private void dgvWarehouse_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            decimal decValue;

            if (e.Value == null)

                return;

            if (decimal.TryParse(e.Value.ToString(), out decValue) == false)

                return;



            e.Value = Math.Round(decValue, 2);
        }

        private void DgvToTransfer_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if(DgvToTransfer.Rows.Count > 0)
            {
                DgvToTransfer.Rows.Clear();
            }
            txtProductName.Text = "";
            txtQuantity.Text = "";
            cbKg.Checked = false;
            txtUnit.Text = "";
            txtQuantity.Text = "";
            cbKg2.Checked = false;
            txtTransferTotal.Text = "";
            txtSearch.Text = "";
        }

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}