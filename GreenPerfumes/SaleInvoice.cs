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
using System.Drawing.Printing;
using EasyTabs;

namespace GreenPerfumes
{
    public partial class SaleInvoice : Form
    {
        SqlDataReader dr;
        bool productcheck = false;


        protected TitleBarTabs ParentTabs
        {
            get
            {
                return (ParentForm as TitleBarTabs);
            }
        }


      
        public SaleInvoice()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void SaleInvoice_Load(object sender, EventArgs e)
        {
            MainClass.FillCustomer(cboCustomer);
            MainClass.FillProducts2(cboProductName);
            MainClass.FillUnits(cboUnit);
            MainClass.FillWarehouses(cboWarehouse);
            cboInvoiceType.SelectedIndex = 0;
       //    txtDated.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dashboard ds = new Dashboard();
            MainClass.showWindow(ds, this, MDI.ActiveForm);
        }


        private void cboCustomer_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboCustomer.SelectedIndex != 0)
            {
                float inopening = 0;
                float insales = 0;
                try
                {
                    MainClass.con.Open();
                    SqlCommand cmd = new SqlCommand("select PersonPhone from Persons where PersonID = '" + cboCustomer.SelectedValue + "' and PersonType = '2'", MainClass.con);
                    txtContactNo.Text = cmd.ExecuteScalar().ToString();
                    MainClass.con.Close();
                    txtCustomerName.Text = cboCustomer.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MainClass.con.Close();
                } //Person Contact
                try
                {
                    MainClass.con.Open();
                    SqlCommand cmd = new SqlCommand("select sum(DueAmount) from OpeningAccounts where PesonType_ID = '2' and  PersonName_ID = '" + cboCustomer.SelectedValue.ToString() + "'", MainClass.con);
                    object ob = cmd.ExecuteScalar();
                    if (ob.ToString() != "")
                    {
                        inopening = float.Parse(ob.ToString());
                        txtInOpenbalances.Text = inopening.ToString();
                    }
                    else
                    {
                        txtInOpenbalances.Text = "0";
                    }
                    MainClass.con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MainClass.con.Close();
                } // In opening
                try
                {
                    MainClass.con.Open();
                    SqlCommand cmd = new SqlCommand("select sum(RemainingBalance)from CustomerLedgers where Customer_ID = '"+ cboCustomer.SelectedValue.ToString() + "'", MainClass.con);
                    object ob = cmd.ExecuteScalar();
                    if (ob != null && ob.ToString() != "")
                    {
                        insales = float.Parse(ob.ToString());
                        txtInSalesLedgers.Text = insales.ToString();
                    }
                    else
                    {
                        txtInSalesLedgers.Text = "0";
                    }
                    MainClass.con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MainClass.con.Close();
                } // In Sales
                txtTotalBalance.Text = (inopening + insales).ToString();

            }
            else
            {
                txtContactNo.Text = "";
                txtInOpenbalances.Text = "";
                txtInSalesLedgers.Text = "";
                txtTotalBalance.Text = "";
            }
        }
        float txtstk = 0;
        private void cboProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProductName.SelectedIndex != 0)
            {
                SqlCommand cmd = null;
                try
                {
                    object stockqty = null;
                    object unitcheck = null;
                    MainClass.con.Open();
                    cmd = new SqlCommand("select sh_Unit from ShopStocks where sh_Pcode = '" + cboProductName.SelectedValue + "'    ", MainClass.con);
                    unitcheck = cmd.ExecuteScalar();


                    if(guna2CheckBox1.Checked )
                    {
                        cmd = new SqlCommand("select InHandQty from ExtraProducts where ProductName = '" + cboProductName.Text + "'    ", MainClass.con);
                        stockqty = cmd.ExecuteScalar();
                    }
                    else
                    {
                        cmd = new SqlCommand("select sh_Qty  from ShopStocks where sh_Pcode = '" + cboProductName.SelectedValue + "'    ", MainClass.con);
                        stockqty = cmd.ExecuteScalar();
                    }
                    if (stockqty == null )
                    {
                        lblInStock.Text = "No";
                    }
                    else
                    {
                        lblInStock.Text = "Yes";
                    }
                    MainClass.con.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                    MainClass.con.Close();
                }    
                    
                
            }
        }

    

        private string[] ProductsData = new string[3];
        public static  string SALEINVOICENO = "";
        private void cboUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtStockInfo.Text != "")
            {
                SqlCommand cmd = null;

                MainClass.con.Open();
                cmd = new SqlCommand("select sh_Unit from ShopStocks where sh_Pcode = '" + cboProductName.SelectedValue + "'    ", MainClass.con);
                object unitcheck = cmd.ExecuteScalar();
                object ob = null;
                if (guna2CheckBox1.Checked)
                {
                    cmd = new SqlCommand("select InHandQty from ExtraProducts where ProductName = '" + cboProductName.Text + "'    ", MainClass.con);
                    ob = cmd.ExecuteScalar();
                }
                else
                {
                    cmd = new SqlCommand("select sh_Qty  from ShopStocks where sh_Unit = '" + unitcheck + "' and sh_Pcode = '" + cboProductName.SelectedValue + "'    ", MainClass.con);
                    ob = cmd.ExecuteScalar();
                }

                if (ob != null)
                {
                    txtstk = float.Parse(ob.ToString());
                }
                else
                {
                    txtstk = 0;
                }

                MainClass.con.Close();


                if (guna2CheckBox1.Checked)
                {
                    txtStockInfo.Text = txtstk.ToString();
                }
                else
                {


                    if (cboUnit.SelectedValue.ToString() == "1")
                    {
                        txtstk = txtstk / 1000;
                        txtStockInfo.Text = txtstk.ToString();
                    }
                    else if (cboUnit.SelectedValue.ToString() == "2")
                    {
                        txtstk = txtstk / 25;
                        txtStockInfo.Text = txtstk.ToString();
                    }
                    else if (cboUnit.SelectedValue.ToString() == "6")
                    {
                        txtstk = txtstk / 12;
                        txtStockInfo.Text = txtstk.ToString();
                    }
                    else
                    {
                        txtStockInfo.Text = txtstk.ToString();
                    }

                }
            }
            else
            {
                txtStockInfo.Text = "0";
            }

            if (cboUnit.SelectedIndex != 0)
            {
                try
                {
                    bool found = false;
                    object extraunit = null;
                    MainClass.con.Open();
                    if (guna2CheckBox1.Checked)
                    {
                        SqlCommand cmd = new SqlCommand("select pr.SaleRate,pr.ProductName,u.UnitName from ExtraProducts pr inner join Units u on u.UnitID = pr.UnitID where pr.ProductName = '" + cboProductName.Text + "' ", MainClass.con);
                        dr = cmd.ExecuteReader();

                        SqlCommand cmd1 = new SqlCommand("select UnitID from ExtraProducts where ProductName = '" + cboProductName.Text + "'",MainClass.con);
                        extraunit = cmd1.ExecuteScalar().ToString();
                    }
                    else
                    {
                        if (cboUnit.Text != "Kg" && cboUnit.Text != "Grams")
                        {
                            SqlCommand cmd = new SqlCommand("select pr.SaleRate,p.ProductName,u.UnitName from Prices pr inner join Products p on p.Pcode = pr.pcode inner join Units u on u.UnitID = pr.Pro_Unit where pr.pcode = '" + cboProductName.SelectedValue + "' and pr.Pro_Unit = '" + cboUnit.SelectedValue + "'", MainClass.con);
                            dr = cmd.ExecuteReader();

                        }
                        else
                        {
                            SqlCommand cmd = new SqlCommand("select pr.SaleRate,p.ProductName,u.UnitName from Prices pr inner join Products p on p.Pcode = pr.pcode inner join Units u on u.UnitID = pr.Pro_Unit where pr.pcode = '" + cboProductName.SelectedValue + "' and pr.Pro_Unit = '1'", MainClass.con);
                            dr = cmd.ExecuteReader();
                        }
                    }
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ProductsData[0] = dr[0].ToString();  //Sale Rate
                            ProductsData[1] = dr[1].ToString(); // ProductName
                            ProductsData[2] = dr[2].ToString(); // Unit Name
                            found = true;
                            if (guna2CheckBox1.Checked)
                            {
                                if (ProductsData[2] == cboUnit.Text)
                                {
                                    txtQuantity.Enabled = true;
                                }
                                else
                                {
                                    txtQuantity.Enabled = false;
                                }
                            }
                            else
                            {
                                if (cboUnit.Text == "Grams")
                                {
                                    float rate;
                                    rate = float.Parse(ProductsData[0].ToString());
                                    rate = rate / 1000;
                                    ProductsData[0] = rate.ToString();
                                }
                                else
                                {
                                    txtSaleRate.Text = ProductsData[0].ToString();
                                }
                            }
                        }
                    }
                    if (found == false)
                    {
                        txtSaleRate.Text = "";
                        txtQuantity.Enabled = false;
                    }
                    else
                    {
                        if (found == true)
                        {
                            if(guna2CheckBox1.Checked == true)
                            {
                                if (ProductsData[2] == cboUnit.Text)
                                {

                                    txtSaleRate.Text = ProductsData[0].ToString();
                                    txtQuantity.Enabled = true;
                                }
                                else
                                {
                                    txtQuantity.Enabled = false;
                                }
                            }
                            else
                            {
                                txtSaleRate.Text = ProductsData[0].ToString();
                                txtQuantity.Enabled = true;
                            }
                     
                        }
                      
                        else
                        {
                            txtQuantity.Enabled = false;

                        }
                    }
                    MainClass.con.Close();



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MainClass.con.Close();
                }
            }
            else
            {
                if (cboUnit.SelectedIndex == 0)
                {
                    txtSaleRate.Text = "";
                    txtQuantity.Enabled = false;
                }
            }

            if (guna2CheckBox1.Checked != true)
            {
                if (txtStockInfo.Text == "0" || txtStockInfo.Text == "")
                {
                    txtQuantity.Enabled = false;
                }
            }
        }

        private void CompleteClear()
        {
            ClearForm();
            txtCustomerName.Text = "";
            txtInvoiceType.Text = "";
            txtTotalAmount.Text = "";
            SALEINVOICENO = "";
            txtPayingAmount.Text = "";
            txtTotalBalance.Text = "";
            txtRemainingAmount.Text = "";
            txtGrandTotal.Text = "";
            dtInvoice.Value = DateTime.Now;
            txtInOpenbalances.Text = "";
            txtInSalesLedgers.Text = "";
            lblInStock.Text = "";
            guna2CheckBox1.Checked = false;
            cboCustomer.SelectedIndex = 0;
            dgvSaleItems.Rows.Clear();
            cboCustomer.Focus();
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            int dx;
            if(cboUnit.SelectedIndex.ToString()== "4" && txtQuantity.Text.ToString() == "12" || cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "24" || cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "36" || 
                cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "48" ||
               cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "60" || cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "72" || cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "84" ||
              cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "96" || cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "108" || cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "120")
            {
                cboUnit.SelectedIndex = 5;
                dx = int.Parse(txtQuantity.Text);
                txtQuantity.Text = Convert.ToString(dx / 12);

            }
            if (txtQuantity.Text != "" || txtQuantity.Text != "0" || txtSaleRate.Text != "")
            {
                float rate = float.Parse(txtSaleRate.Text);
                float qty;
                float.TryParse(txtQuantity.Text.Trim(), out qty);
                txtProductTot.Text = Convert.ToString(qty * rate);
            }
            else
            {
                txtProductTot.Text = "0";
            }
        }

        private void del()
        {

          

        }

      

        private void ClearForm()
        {
            cboProductName.SelectedIndex = 0;
            cboWarehouse.SelectedIndex = 0;
            cboUnit.SelectedIndex = 0;
           
            if (txtQuantity.Text != "")
            {
                txtQuantity.TextChanged -= txtQuantity_TextChanged;
                txtQuantity.Clear();
                txtQuantity.TextChanged += txtQuantity_TextChanged;

            }

            if (txtSaleRate.Text != "")
            {
                txtSaleRate.TextChanged -= txtSaleRate_TextChanged;
                txtSaleRate.Clear();
                txtSaleRate.TextChanged += txtSaleRate_TextChanged;

            }

            if (txtProductTot.Text != "")
            {
                txtProductTot.TextChanged -= txtProductTot_TextChanged;
                txtProductTot.Clear();
                txtProductTot.TextChanged += txtProductTot_TextChanged;

            }
        }
        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if(txtProductTot.Text == "" || txtProductTot.Text == "0")
            {
                MessageBox.Show("Check Details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(float.Parse(txtQuantity.Text) > float.Parse(txtStockInfo.Text))
            {
                MessageBox.Show("Not Enough Stock", "Low Stock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtProductTot.Text == "0")
            {
                if (txtQuantity.Text == "")
                {
                    MessageBox.Show("Please Enter Quantity", "Input Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (txtSaleRate.Text == "")
                {
                    MessageBox.Show("Please Get Rate", "Input Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                float quantity;
                float totalofproduct, grandtotal, puchaserate;

                float.TryParse(txtQuantity.Text.Trim(), out quantity);
                float.TryParse(txtSaleRate.Text.Trim(), out puchaserate);
                float.TryParse(txtProductTot.Text.Trim(), out totalofproduct);
                float.TryParse(txtGrandTotal.Text.Trim(), out grandtotal);



                if (dgvSaleItems.Rows.Count == 0)
                {
                    DataGridViewRow createrow = new DataGridViewRow();
                    createrow.CreateCells(dgvSaleItems);
                    createrow.Cells[0].Value = cboProductName.SelectedValue;
                    createrow.Cells[1].Value = cboProductName.Text;
                    createrow.Cells[2].Value = cboWarehouse.SelectedValue;
                    createrow.Cells[3].Value = cboWarehouse.Text;
                    createrow.Cells[4].Value = quantity;
                    createrow.Cells[5].Value = cboUnit.SelectedValue;
                    createrow.Cells[6].Value = cboUnit.Text;
                    createrow.Cells[7].Value = puchaserate;
                    createrow.Cells[8].Value = totalofproduct;
                
                    dgvSaleItems.Rows.Add(createrow);
                    ClearForm();
                    cboProductName.Focus();
                }
                else
                {
                    foreach (DataGridViewRow check in dgvSaleItems.Rows)
                    {
                        if (Convert.ToString(check.Cells[0].Value) == Convert.ToString(cboProductName.SelectedValue) &&
                            Convert.ToString(check.Cells[5].Value) == Convert.ToString(cboUnit.SelectedValue))
                        {
                            productcheck = true;
                            break;
                        }
                        else
                        {
                            productcheck = false;
                        }
                    }
                    if (productcheck == true)
                    {
                        foreach (DataGridViewRow row in dgvSaleItems.Rows)
                        {
                            if (Convert.ToString(row.Cells[0].Value) == Convert.ToString(cboProductName.SelectedValue))
                            {
                                row.Cells["QtyGVC"].Value = float.Parse(row.Cells["QtyGVC"].Value.ToString()) + quantity;
                                row.Cells["TotalGV"].Value = float.Parse(row.Cells["QtyGVC"].Value.ToString()) * float.Parse(row.Cells["SaleRateGVC"].Value.ToString());
                                ClearForm();
                            }
                        }
                    }
                    else
                    {
                        if (productcheck == false)
                        {
                            DataGridViewRow createrow = new DataGridViewRow();
                            createrow.CreateCells(dgvSaleItems);
                            createrow.Cells[0].Value = cboProductName.SelectedValue;
                            createrow.Cells[1].Value = cboProductName.Text;
                            createrow.Cells[2].Value = cboWarehouse.SelectedValue;
                            createrow.Cells[3].Value = cboWarehouse.Text;
                            createrow.Cells[4].Value = quantity;
                            createrow.Cells[5].Value = cboUnit.SelectedValue;
                            createrow.Cells[6].Value = cboUnit.Text;
                            createrow.Cells[7].Value = puchaserate;
                            createrow.Cells[8].Value = totalofproduct;
                     

                            dgvSaleItems.Rows.Add(createrow);
                            ClearForm();
                            cboProductName.Focus();
                        }
                    }
                }
            }



            button1.PerformClick();
        }

        private void txtProductTot_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSaleRate_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            float gross = 0;
            if (dgvSaleItems.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvSaleItems.Rows)
                {
                    gross += float.Parse(row.Cells[8].Value.ToString());
                }
                txtGrandTotal.Text = Convert.ToString(gross);
                txtTotalAmount.Text = Convert.ToString(gross);
                cboCustomer.Text = txtCustomerName.Text;
                if (cboInvoiceType.Text == "Cash")
                {
                    txtPayingAmount.Text = Convert.ToString(gross);
                }
                
            }
            if (txtPayingAmount.Text == "0" || txtPayingAmount.Text == "")
            {
                txtRemainingAmount.Text = gross.ToString();
            }
            del();


            int productcount = 0;
            foreach (DataGridViewRow row in dgvSaleItems.Rows)
            {
                productcount++;
            }
            label25.Text = int.Parse(productcount.ToString()).ToString();


        }

        private void txtPayingAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtPayingAmount.Text != "" || txtPayingAmount.Text == "0")
            {
                float tot = float.Parse(txtTotalAmount.Text);
                float paying = float.Parse(txtPayingAmount.Text);
                txtRemainingAmount.Text = Convert.ToString(tot - paying);
            }
            else
            {
                txtRemainingAmount.Text = txtTotalAmount.Text;
            }
        }

        private void dtInvoice_ValueChanged(object sender, EventArgs e)
        {
       //     txtDated.Text = dtInvoice.Text.ToString();
        }

        private void cboInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtInvoiceType.Text = cboInvoiceType.Text;
            if (cboInvoiceType.Text == "Cash")
            {
                guna2GroupBox2.Visible = false;
            }
            else
            {
                guna2GroupBox2.Visible = true;
            }


        }

        private void dgvSaleItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if(e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if(e.ColumnIndex == 9)
                {
                    dgvSaleItems.Rows.RemoveAt(dgvSaleItems.CurrentRow.Index);
                }
            }
        }

        private string[] Checking = new string[14];
        public static int SaleID = 0;
        private void btnFinalize_Click(object sender, EventArgs e)
        {

            if (btnFinalize.Text == "&FINALIZE")
            {
                if (cboInvoiceType.SelectedIndex == 0)
                {
                    MessageBox.Show("Please Select Invoice Type");
                    return;
                }
                if (cboCustomer.SelectedIndex == 0)
                {
                    MessageBox.Show("Please Select Customer");
                    return;
                }

                Random generator = new Random();
                string invoiceno = "SAL" + generator.Next(0, 1000000).ToString("D6");
                float grandtotal = float.Parse(txtGrandTotal.Text.ToString());
                MainClass.con.Open();
                try
                {
                    string InsertCustomerInvoice = "insert into CustomerInvoices (Customer_ID,PaymentType,InvoiceDate,Warehouse_ID) values (@Customer_ID,@PaymentType,@InvoiceDate,@Warehouse_ID)";
                    SqlCommand cmd = new SqlCommand(InsertCustomerInvoice, MainClass.con);
                    cmd.Parameters.AddWithValue("@Customer_ID", cboCustomer.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@PaymentType", cboInvoiceType.Text);
                    cmd.Parameters.AddWithValue("@InvoiceDate", dtInvoice.Value.ToShortDateString());
                    cmd.Parameters.AddWithValue("@Warehouse_ID", dgvSaleItems.CurrentRow.Cells[2].Value);
                    cmd.ExecuteNonQuery();


                    ////Get Customer InvoiceID
                    string CustomerInvoiceID = Convert.ToString(MainClass.Retrieve("select MAX(CustomerInvoiceID) from CustomerInvoices").Rows[0][0]);
                    if (string.IsNullOrEmpty(CustomerInvoiceID))
                    {
                        MessageBox.Show("Please Check The Error or Try Again");
                        return;
                    }
                    float discount = 0;
                    if (txtDiscountAmount.Text != "")
                    {
                        discount = float.Parse(txtDiscountAmount.Text);
                    }

                    if (dgvSaleItems.Rows.Count > 0)
                    {


                        //Inserting Sales
                        string InsertSales = "insert into Sales (InvoiceNo,CustomerInvoice_ID,Discount,GrandTotal) values (@InvoiceNo,@CustomerInvoice_ID,@Discount,@GrandTotal)";
                        SqlCommand cmd1 = new SqlCommand(InsertSales, MainClass.con);
                        cmd1.Parameters.AddWithValue("@InvoiceNo", invoiceno);
                        cmd1.Parameters.AddWithValue("@Discount", discount);
                        cmd1.Parameters.AddWithValue("@CustomerInvoice_ID", CustomerInvoiceID);
                        cmd1.Parameters.AddWithValue("@GrandTotal", grandtotal);
                        cmd1.ExecuteNonQuery();


                        ////Get SalesID
                        string SalesID = Convert.ToString(MainClass.Retrieve("select MAX(SalesID) from Sales").Rows[0][0]);
                        if (string.IsNullOrEmpty(SalesID))
                        {
                            MessageBox.Show("Please Check The Error or Try Again");
                            return;
                        }




                        object exid = null;
                        //Inserting Products
                        foreach (DataGridViewRow products in dgvSaleItems.Rows)
                        {
                            string InsertSaleDetails = "insert into SalesDetails (Sales_ID,CustomerInvoice_ID,Product_ID,SalesQty,SalesUnit_ID,SalesRate,TotalofProduct) values (@Sales_ID,@CustomerInvoice_ID,@Product_ID,@SalesQty,@SalesUnit_ID,@SalesRate,@TotalofProduct)";
                            SqlCommand cmd2 = new SqlCommand(InsertSaleDetails, MainClass.con);
                            cmd2.Parameters.AddWithValue("@Sales_ID", SalesID);
                            cmd2.Parameters.AddWithValue("@CustomerInvoice_ID", CustomerInvoiceID);
                            cmd2.Parameters.AddWithValue("@Product_ID", products.Cells[0].Value);
                            cmd2.Parameters.AddWithValue("@SalesQty", products.Cells[4].Value);
                            cmd2.Parameters.AddWithValue("@SalesUnit_ID", products.Cells[5].Value);
                            cmd2.Parameters.AddWithValue("@SalesRate", products.Cells[7].Value);
                            cmd2.Parameters.AddWithValue("@TotalofProduct", products.Cells[8].Value);
                            cmd2.ExecuteNonQuery();

                           
                        }


                        foreach (DataGridViewRow sold in dgvSaleItems.Rows)
                        {
                            cmd = new SqlCommand("select Extra from Products where Pcode = '" + sold.Cells[0].Value + "'", MainClass.con);
                            exid = cmd.ExecuteScalar().ToString();
                            if (exid.ToString() == "False")
                            {

                                float q;
                                int unitchanged = 0;

                                if (sold.Cells[6].Value.ToString() == "Kg")
                                {
                                    q = float.Parse(sold.Cells[4].Value.ToString()) * 1000;
                                    unitchanged = 1;

                                }
                                else if (sold.Cells[6].Value.ToString() == "Ounce")
                                {
                                    q = float.Parse(sold.Cells[4].Value.ToString()) * 25;
                                    unitchanged = 2;
                                }
                                else if (sold.Cells[6].Value.ToString() == "Dozen")
                                {
                                    q = float.Parse(sold.Cells[4].Value.ToString()) * 12;
                                    unitchanged = 3;
                                }
                                else
                                {
                                    q = float.Parse(sold.Cells[4].Value.ToString());
                                    unitchanged = 0;
                                }
                                string updateStocks = "";
                                if (unitchanged == 1)
                                {
                                    updateStocks = string.Format("update ShopStocks set sh_Qty = sh_Qty - '{0}' where sh_Pcode = '{1}' and sh_Unit = '{2}'", q, sold.Cells[0].Value, '3');
                                }
                                else if (unitchanged == 2)
                                {
                                    updateStocks = string.Format("update ShopStocks set sh_Qty = sh_Qty - '{0}' where sh_Pcode = '{1}' and sh_Unit = '{2}'", q, sold.Cells[0].Value, '3');
                                }
                                else if (unitchanged == 3)
                                {
                                    updateStocks = string.Format("update ShopStocks set sh_Qty = sh_Qty - '{0}' where sh_Pcode = '{1}' and sh_Unit = '{2}'", q, sold.Cells[0].Value, '5');
                                }
                                else
                                {
                                    updateStocks = string.Format("update ShopStocks set sh_Qty = sh_Qty - '{0}' where sh_Pcode = '{1}' and sh_Unit = '{2}'", q, sold.Cells[0].Value, sold.Cells[5].Value);
                                }
                                cmd = new SqlCommand(updateStocks, MainClass.con);

                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                float qty = 0;
                                object stockqty = null;
                                float stock = 0;
                                cmd = new SqlCommand("select InHandQty from ExtraProducts where ProductName = '" + sold.Cells[1].Value.ToString()+ "'    ", MainClass.con);
                                stockqty = cmd.ExecuteScalar();
                                if (stockqty != null)
                                {
                                   stock = float.Parse(stockqty.ToString());
                                    qty = float.Parse(sold.Cells[4].Value.ToString());

                                    stock -= qty;
                                    cmd = new SqlCommand("update ExtraProducts set InHandQty = '" + stock + "' where ProductName = '" + sold.Cells[1].Value.ToString() + "' ", MainClass.con);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                        }
                        
                        //Inserting Customer Payments
                        string InsertPayment = "insert into CustomerLedgers (CustomerInvoice_ID,Customer_ID,InvoiceType,InvoiceDate,TotalAmount,PaidAmount,RemainingBalance) values(@CustomerInvoice_ID,@Customer_ID,@InvoiceType,@InvoiceDate,@TotalAmount,@PaidAmount,@RemainingBalance)";
                        SqlCommand cmd3 = new SqlCommand(InsertPayment, MainClass.con);
                        cmd3.Parameters.AddWithValue("@CustomerInvoice_ID", CustomerInvoiceID);
                        cmd3.Parameters.AddWithValue("@Customer_ID", cboCustomer.SelectedValue.ToString());
                        cmd3.Parameters.AddWithValue("@InvoiceType", cboInvoiceType.Text);
                        cmd3.Parameters.AddWithValue("@InvoiceDate", dtInvoice.Value.ToShortDateString());
                        cmd3.Parameters.AddWithValue("@TotalAmount", float.Parse(txtTotalAmount.Text));
                        cmd3.Parameters.AddWithValue("@PaidAmount", float.Parse(txtPayingAmount.Text));
                        cmd3.Parameters.AddWithValue("@RemainingBalance", float.Parse(txtRemainingAmount.Text));
                        cmd3.ExecuteNonQuery();

                        ////Get SalesID
                        string CustomerLedgeRID = Convert.ToString(MainClass.Retrieve("select MAX(CustomerLedgerID) from CustomerLedgers").Rows[0][0]);
                        if (string.IsNullOrEmpty(CustomerLedgeRID))
                        {
                            MessageBox.Show("Please Check The Error or Try Again");
                            return;
                        }

                        cmd = new SqlCommand("insert into CustomerReport (Customer_ID,SaleInvoiceNo,SaleInvoiceDate,SaleTotalAmount,SaleLastPaid,SaleLastPayingDate,SaleBalance) values (@Customer_ID,@SaleInvoiceNo,@SaleInvoiceDate,@SaleTotalAmount,@SaleLastPaid,@SaleLastPayingDate,@SaleBalance)", MainClass.con);
                        cmd.Parameters.AddWithValue("@Customer_ID", cboCustomer.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@SaleInvoiceDate", dtInvoice.Value.ToShortDateString());
                        cmd.Parameters.AddWithValue("@SaleInvoiceNo", invoiceno);
                        cmd.Parameters.AddWithValue("@SaleTotalAmount", float.Parse(txtTotalAmount.Text));
                        cmd.Parameters.AddWithValue("@SaleLastPaid", float.Parse(txtPayingAmount.Text));
                        cmd.Parameters.AddWithValue("@SaleLastPayingDate", dtInvoice.Value.ToShortDateString());
                        cmd.Parameters.AddWithValue("@SaleBalance", float.Parse(txtRemainingAmount.Text));
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Sale Successful");
                        SALEINVOICENO = invoiceno;
                        SaleReportForm srf = new SaleReportForm();
                        srf.Show();
                        CompleteClear();

                    }


                }
                catch (Exception ex)
                {
                    MainClass.con.Close();
                    MessageBox.Show(ex.Message);
                }
                MainClass.con.Close();
            }
            else 
            {
                SqlCommand cmd = null;
                float discount = 0;
                if (txtDiscountAmount.Text != "")
                {
                    discount = float.Parse(txtDiscountAmount.Text);
                }
                float grandtotal = float.Parse(txtGrandTotal.Text.ToString());
                int customerinvoiceid = int.Parse(lblCustomerInvoiceID.Text);
                SaleID = int.Parse(lblSalesID.Text);
                string invoiceno = lblInvoice.Text.ToString();
                MainClass.con.Open();

                try
                {
                    cmd = new SqlCommand("update CustomerInvoices set Customer_ID = @Customer_ID, PaymentType = @PaymentType, InvoiceDate = @InvoiceDate,Warehouse_ID = @Warehouse_ID where CustomerInvoiceID = '" + customerinvoiceid + "'", MainClass.con);
                    cmd.Parameters.AddWithValue("@CustomerInvoiceID", customerinvoiceid);
                    cmd.Parameters.AddWithValue("@Customer_ID", cboCustomer.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@PaymentType", cboInvoiceType.Text);
                    cmd.Parameters.AddWithValue("@InvoiceDate", dtInvoice.Value.ToShortDateString());
                    cmd.Parameters.AddWithValue("@Warehouse_ID", dgvSaleItems.CurrentRow.Cells[2].Value);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MainClass.con.Close();
                    MessageBox.Show(ex.Message);
                }//Update CustomerInvoice
                try
                {
                    cmd = new SqlCommand("update Sales set Discount = @Discount, GrandTotal = @GrandTotal where CustomerInvoice_ID = @CustomerInvoice_ID and InvoiceNo = @InvoiceNo and SalesID = @SalesID", MainClass.con);
                    cmd.Parameters.AddWithValue("@SalesID", int.Parse(lblSalesID.Text));
                    cmd.Parameters.AddWithValue("@Discount", discount);
                    cmd.Parameters.AddWithValue("@InvoiceNo", invoiceno);
                    cmd.Parameters.AddWithValue("@CustomerInvoice_ID", customerinvoiceid);
                    cmd.Parameters.AddWithValue("@GrandTotal", grandtotal);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MainClass.con.Close();
                    MessageBox.Show(ex.Message);
                }//Update Sales
                try
                {

                    SqlDataReader dr;
                    cmd = new SqlCommand("select s.SalesID,p.Pcode,u.UnitName,sd.SalesQty,u.UnitID,s.InvoiceNo,sd.SalesRate from Sales s inner join SalesDetails sd on sd.Sales_ID = s.SalesID inner join Products p on p.Pcode = sd.Product_ID inner join Units u on u.UnitID = sd.SalesUnit_ID where s.SalesID = '" + int.Parse(lblSalesID.Text) + "'", MainClass.con);
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SalesQTY[0] = dr[0].ToString();  //Sale ID
                            SalesQTY[1] = dr[1].ToString(); // Pcode
                            SalesQTY[2] = dr[2].ToString(); // UnitName
                            SalesQTY[3] = dr[3].ToString(); // Qty
                            SalesQTY[4] = dr[4].ToString(); // UntID
                            SalesQTY[5] = dr[5].ToString(); // Invoice
                            SalesQTY[6] = dr[6].ToString(); // Invoice
                            cmd = new SqlCommand("select Extra from Products where Pcode = '" + SalesQTY[1].ToString() + "'", MainClass.con);
                            object extra2 = cmd.ExecuteScalar();
                            if (bool.Parse(extra2.ToString()) == true)
                            {
                                SalesQTY[3] = dr[3].ToString(); // SaleQty
                            }
                            else
                            {

                                if (SalesQTY[2] == "Kg")
                                {
                                    SalesQTY[3] = Convert.ToString(float.Parse(dr[3].ToString()) * 1000);
                                }
                                else if (SalesQTY[2] == "Ounce")
                                {
                                    SalesQTY[3] = Convert.ToString(float.Parse(dr[3].ToString()) * 25);
                                }
                                else if (SalesQTY[2] == "Dozen")
                                {
                                    SalesQTY[3] = Convert.ToString(float.Parse(dr[3].ToString()) * 12);
                                }
                                else
                                {
                                    SalesQTY[3] = dr[3].ToString(); // SaleQty
                                }
                            }

                            try
                            {
                                cmd = new SqlCommand("select Extra from Products where Pcode = '" + SalesQTY[1].ToString() + "'", MainClass.con);
                                object extra = cmd.ExecuteScalar();

                                if (bool.Parse(extra.ToString()) == true)
                                {
                                    cmd = new SqlCommand("select ProductName from Products where Pcode = '" + SalesQTY[1].ToString() + "'    ", MainClass.con);
                                    object productname = cmd.ExecuteScalar();

                                    cmd = new SqlCommand("select InHandQty from ExtraProducts where ProductName = '" + productname + "'    ", MainClass.con);
                                    object stockqty = cmd.ExecuteScalar();



                                    if (stockqty == null)
                                    {
                                        cmd = new SqlCommand("Insert into ExtraProducts (ProductName,UnitID,PurchaseRate,SaleRate,InHandQty) values(@ProductName,@UnitID,@PurchaseRate,@SaleRate,@InHandQty)", MainClass.con);
                                        cmd.Parameters.AddWithValue("@ProductName", productname);
                                        cmd.Parameters.AddWithValue("@UnitID", SalesQTY[4]);
                                        cmd.Parameters.AddWithValue("@PurchaseRate", float.Parse(SalesQTY[6]));
                                        cmd.Parameters.AddWithValue("@SaleRate", float.Parse(SalesQTY[6]));
                                        cmd.Parameters.AddWithValue("@InHandQty", float.Parse(SalesQTY[3]));
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        cmd = new SqlCommand("update ExtraProducts set InHandQty = @InHandQty where ProductName = @ProductName", MainClass.con);
                                        cmd.Parameters.AddWithValue("@ProductName", productname);
                                        cmd.Parameters.AddWithValue("@InHandQty", float.Parse(SalesQTY[3]));
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {

                                    string CheckStock = "select s.sh_Qty as 'Quantity' from ShopStocks s where s.sh_Pcode = '" + SalesQTY[1].ToString() + "' ";
                                    cmd = new SqlCommand(CheckStock, MainClass.con);
                                    object ob = cmd.ExecuteScalar();



                                    if (ob == null)
                                    {
                                        cmd = new SqlCommand("insert into ShopStocks (sh_Pcode,sh_Qty,sh_Unit) values(@sh_Pcode,@sh_Qty,@sh_Unit)", MainClass.con);
                                        cmd.Parameters.AddWithValue("@sh_Pcode", SalesQTY[1].ToString());
                                        cmd.Parameters.AddWithValue("@sh_Qty", SalesQTY[3].ToString());
                                        cmd.Parameters.AddWithValue("@sh_Unit", SalesQTY[4].ToString());
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        float total = float.Parse(SalesQTY[3].ToString());
                                        total += float.Parse(ob.ToString());
                                        MainClass.UpdateShopStock(int.Parse(SalesQTY[1]), total);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MainClass.con.Close();
                                MessageBox.Show(ex.Message);
                            }


                        }



                    }
                }
                catch (Exception ex)
                {
                    MainClass.con.Close();
                    MessageBox.Show(ex.Message);
                } //Bring Back Inventory
                try
                {
                    cmd = new SqlCommand("delete from SalesDetails where Sales_ID = '" + int.Parse(lblSalesID.Text) + "'", MainClass.con);
                    cmd.ExecuteNonQuery();

                    foreach (DataGridViewRow products in dgvSaleItems.Rows)
                    {

                        string InsertSaleDetails = "insert into SalesDetails (Sales_ID,CustomerInvoice_ID,Product_ID,SalesQty,SalesUnit_ID,SalesRate,TotalofProduct) values (@Sales_ID,@CustomerInvoice_ID,@Product_ID,@SalesQty,@SalesUnit_ID,@SalesRate,@TotalofProduct)";
                        SqlCommand cmd2 = new SqlCommand(InsertSaleDetails, MainClass.con);
                        cmd2.Parameters.AddWithValue("@Sales_ID", SaleID);
                        cmd2.Parameters.AddWithValue("@CustomerInvoice_ID", customerinvoiceid);
                        cmd2.Parameters.AddWithValue("@Product_ID", products.Cells[0].Value);
                        cmd2.Parameters.AddWithValue("@SalesQty", products.Cells[4].Value);
                        cmd2.Parameters.AddWithValue("@SalesUnit_ID", products.Cells[5].Value);
                        cmd2.Parameters.AddWithValue("@SalesRate", products.Cells[7].Value);
                        cmd2.Parameters.AddWithValue("@TotalofProduct", products.Cells[8].Value);
                        cmd2.ExecuteNonQuery();
                        
                    }


                }
                catch (Exception ex)
                {
                    MainClass.con.Close();
                    MessageBox.Show(ex.Message);
                } //Update SalesDetials
                try
                {
                    foreach (DataGridViewRow sold in dgvSaleItems.Rows)
                    {

                        float q;
                        int unitchanged = 0;

                        if (sold.Cells[6].Value.ToString() == "Kg")
                        {
                            q = float.Parse(sold.Cells[4].Value.ToString()) * 1000;
                            unitchanged = 1;

                        }
                        else if (sold.Cells[6].Value.ToString() == "Ounce")
                        {
                            q = float.Parse(sold.Cells[4].Value.ToString()) * 25;
                            unitchanged = 2;
                        }
                        else if (sold.Cells[6].Value.ToString() == "Dozen")
                        {
                            q = float.Parse(sold.Cells[4].Value.ToString()) * 12;
                            unitchanged = 3;
                        }
                        else
                        {
                            q = float.Parse(sold.Cells[4].Value.ToString());
                            unitchanged = 0;
                        }
                        string updateStocks = "";
                        if (unitchanged == 1)
                        {
                            updateStocks = string.Format("update ShopStocks set sh_Qty = sh_Qty - '{0}' where sh_Pcode = '{1}' and sh_Unit = '{2}'", q, sold.Cells[0].Value, '3');
                        }
                        else if (unitchanged == 2)
                        {
                            updateStocks = string.Format("update ShopStocks set sh_Qty = sh_Qty - '{0}' where sh_Pcode = '{1}' and sh_Unit = '{2}'", q, sold.Cells[0].Value, '3');
                        }
                        else if (unitchanged == 3)
                        {
                            updateStocks = string.Format("update ShopStocks set sh_Qty = sh_Qty - '{0}' where sh_Pcode = '{1}' and sh_Unit = '{2}'", q, sold.Cells[0].Value, '5');
                        }
                        else
                        {
                            updateStocks = string.Format("update ShopStocks set sh_Qty = sh_Qty - '{0}' where sh_Pcode = '{1}' and sh_Unit = '{2}'", q, sold.Cells[0].Value, sold.Cells[5].Value);
                        }
                        cmd = new SqlCommand(updateStocks, MainClass.con);

                        cmd.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    MainClass.con.Close();
                    MessageBox.Show(ex.Message);
                } //ReUpdate Inventory
                try
                {
                    cmd = new SqlCommand("update CustomerLedgers set InvoiceType = @InvoiceType, InvoiceDate = @InvoiceDate, TotalAmount = @TotalAmount,PaidAmount = @PaidAmount, RemainingBalance = @RemainingBalance where CustomerLedgerID = @CustomerLedgerID ", MainClass.con);
                    cmd.Parameters.AddWithValue("@CustomerLedgerID", int.Parse(lblCustomerLedgerID.Text));
                    cmd.Parameters.AddWithValue("@InvoiceType", txtInvoiceType.Text);
                    cmd.Parameters.AddWithValue("@InvoiceDate", dtInvoice.Value.ToShortDateString());
                    cmd.Parameters.AddWithValue("@TotalAmount", float.Parse(txtTotalAmount.Text));
                    cmd.Parameters.AddWithValue("@PaidAmount", float.Parse(txtPayingAmount.Text));
                    cmd.Parameters.AddWithValue("@RemainingBalance", float.Parse(txtRemainingAmount.Text));
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MainClass.con.Close();
                    MessageBox.Show(ex.Message);
                } //Update CustomerLedgers
               

                MainClass.con.Close();
                SaleReportForm srf = new SaleReportForm();
                srf.Show();
                CompleteClear();
            }
        }

        private string[] SalesQTY = new string[7];


        private void btnReset_Click(object sender, EventArgs e)
        {
            CompleteClear();
        }

       
     

        private void btnPreview_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printDocument1.DefaultPageSettings.PaperSize = new PaperSize("Custom", 520, 820);
            printPreviewDialog1.ShowDialog();

          
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Sale Invoice", new Font("Arial", 25, FontStyle.Regular), Brushes.Black, new Point(100, 10));
            e.Graphics.DrawString("Date: " + dtInvoice.Value.ToShortDateString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(20, 70));
            e.Graphics.DrawString("Customer: " + cboCustomer.Text.ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(20, 90));
            e.Graphics.DrawString("___________________________________________________________ ", new Font("Helvetica", 12, FontStyle.Regular), Brushes.Black, new Point(0, 115));
            e.Graphics.DrawString("Product ", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(25, 135));
            e.Graphics.DrawString("QTY ", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(190, 135));
            e.Graphics.DrawString("Unit ", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(245, 135));
            e.Graphics.DrawString("Rate ", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(300, 135));
            e.Graphics.DrawString("Amount ", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(400, 135));
            e.Graphics.DrawString("___________________________________________________________ ", new Font("Helvetica", 12, FontStyle.Regular), Brushes.Black, new Point(0, 135));
            int pos = 160;
            foreach (DataGridViewRow item in dgvSaleItems.Rows)
            {
                e.Graphics.DrawString(item.Cells[1].Value.ToString(), new Font("Arial", 9, FontStyle.Regular), Brushes.Black, new Point(25, pos));    //Product
                e.Graphics.DrawString(item.Cells[4].Value.ToString(), new Font("Arial", 9, FontStyle.Regular), Brushes.Black, new Point(190, pos));    // Qty
                e.Graphics.DrawString(item.Cells[6].Value.ToString(), new Font("Arial", 9, FontStyle.Regular), Brushes.Black, new Point(245, pos));    //Unit
                e.Graphics.DrawString(item.Cells[7].Value.ToString(), new Font("Arial", 9, FontStyle.Regular), Brushes.Black, new Point(300, pos));    //Rate
                e.Graphics.DrawString(item.Cells[8].Value.ToString(), new Font("Arial", 9, FontStyle.Regular), Brushes.Black, new Point(400, pos));    //Total
                pos += 20;
            }
            e.Graphics.DrawString("Total Amount: " + txtGrandTotal.Text.ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(280, 65));
            e.Graphics.DrawString("Discount: " + txtDiscountAmount.Text.ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(280, 80));
            if (cboInvoiceType.Text != "Cash")
            {
                e.Graphics.DrawString("Paying Amount: " + txtPayingAmount.Text.ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(280, 93));
                e.Graphics.DrawString("Remaining Balance: " + txtRemainingAmount.Text.ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(280, 113));
            }

        }

        private void cboInvoiceType_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Tab)
            {
                cboProductName.Focus();
            }
        }

        private void txtGrandTotal_TextChanged(object sender, EventArgs e)
        {
            
            txtTotalAmount.Text = txtGrandTotal.Text;
            txtRemainingAmount.Text = txtGrandTotal.Text;
        }

        private void txtDiscountAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtDiscountAmount.Text != "" && txtDiscountAmount.Text != "0")
            {
                float gd = 0;
                foreach (DataGridViewRow item in dgvSaleItems.Rows)
                {
                    gd += float.Parse(item.Cells[8].Value.ToString());
                }
                float disc;
                float tot;
                disc = float.Parse(txtDiscountAmount.Text.ToString());
                tot = gd - disc;
                txtGrandTotal.Text = tot.ToString();
            }
            else
            {
                float gd = 0;
                foreach (DataGridViewRow item in dgvSaleItems.Rows)
                {
                    gd += float.Parse(item.Cells[8].Value.ToString());
                }
                txtGrandTotal.Text = Convert.ToString(gd);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ViewSaleInvoices vs = new ViewSaleInvoices(this);
            vs.Show();
        }

        private void lblInvoice_TextChanged(object sender, EventArgs e)
        {
            if(lblInvoice.Text != "lblInvocien")
            {
                btnFinalize.Text = "&UPDATE";
                btnFinalize.BackColor = Color.DarkRed;
            }
        }

        private void dgvSaleItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            button1.PerformClick();
        }

        private void dgvSaleItems_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            button1.PerformClick();

        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(guna2CheckBox1.Checked)
            {
                MainClass.FillExtra(cboProductName);   
            }
            else
            {
                MainClass.FillProducts2(cboProductName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ExtraProduct ep = new ExtraProduct();
            ep.Show();
        }

        private void guna2CheckBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Tab)
            {
                cboProductName.Focus();
            }
        }

        private void dgvSaleItems_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
           
                if (e.ColumnIndex == 4 || e.ColumnIndex == 8 || e.ColumnIndex == 7)
                {
                    e.CellStyle.Format = "N2";
                }
            
        }

      
    }
}
