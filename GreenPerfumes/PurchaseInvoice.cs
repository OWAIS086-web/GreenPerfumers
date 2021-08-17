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
    public partial class PurchaseInvoice : Form
    {
        bool productcheck = false;
        int rowindex = 0;
        int ad = 0;
        int quan = 0;
        object ob1 = null;
        SqlDataReader dr;
        MainClass ms = new MainClass();
        

        protected TitleBarTabs ParentTabs
        {
            get
            {
                return (ParentForm as TitleBarTabs);
            }
        }
        
        public PurchaseInvoice()
        {
            InitializeComponent();
        }

        private void PurchaseInvoice_Load(object sender, EventArgs e)
        {
            cboInvoiceType.SelectedIndex = 0;
            MainClass.FillProducts2(cboProductName);
            MainClass.FillWarehouses(cboWarehouse);
            MainClass.FillUnits(cboUnit);
            MainClass.FillSupplier(cboSupplier);
            txtDated.Text = DateTime.Now.ToString("dd/MM/yyyy");

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dashboard ds = new Dashboard();
            ds.Show();
            //MainClass.showWindow(ds, this, MDI.ActiveForm);
        }

        private string[] ProductsData = new string[3];
        private void cboUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (cboUnit.SelectedIndex != 0)
            {
                try
                {
                    bool found = false;
                    MainClass.con.Open();
                        if(cboUnit.Text != "Kg" && cboUnit.Text != "Grams")
                    {
                        //       SqlCommand cmd = new SqlCommand("select pr.PurchaseRate,p.ProductName,u.UnitName from PricesOther pr inner join Products p on p.Pcode = pr.pcode inner join Units u on u.UnitID = pr.Pro_UnitO where pr.pcode = '" + cboProductName.SelectedValue + "' and pr.Pro_UnitO = '" + cboUnit.SelectedValue + "'", MainClass.con);
                        SqlCommand cmd = new SqlCommand("select pr.PurchaseRate,p.ProductName,u.UnitName from Prices pr inner join Products p on p.Pcode = pr.pcode inner join Units u on u.UnitID = pr.Pro_Unit where pr.pcode = '" + cboProductName.SelectedValue + "' and pr.Pro_Unit = '" + cboUnit.SelectedValue + "'", MainClass.con);
                        dr = cmd.ExecuteReader();

                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("select pr.PurchaseRate,p.ProductName,u.UnitName from Prices pr inner join Products p on p.Pcode = pr.pcode inner join Units u on u.UnitID = pr.Pro_Unit where pr.pcode = '" + cboProductName.SelectedValue + "' and pr.Pro_Unit = '" + cboUnit.SelectedValue + "'", MainClass.con);
                         dr = cmd.ExecuteReader();
                    }
                    if (dr.HasRows)
                    {
                        while(dr.Read())
                        {
                            ProductsData[0] = dr[0].ToString();  //Purchase Rate
                            ProductsData[1] = dr[1].ToString(); // ProductName
                            ProductsData[2] = dr[2].ToString(); // Unit Name
                            found = true;
                            txtPurchaseRate.Text = ProductsData[0].ToString();                    
                        }
                    }
                    if (found == false)
                    {
                        txtPurchaseRate.Text = "";
                        txtQuantity.Enabled = false;
                        txtDiscountAmount.Enabled = false;
                    }
                    else
                    {
                        if(found == true)
                        {
                            txtPurchaseRate.Text = ProductsData[0].ToString();
                            txtQuantity.Enabled = true;
                            txtDiscountAmount.Enabled = true;
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
                    txtPurchaseRate.Text = "";
                    txtQuantity.Enabled = false;
                    txtDiscountAmount.Enabled = false;
                }
            }
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            int dx;
            object ob = null;
            try
            {
                SqlCommand cmd;
                MainClass.con.Open();
                string CheckPrice = "select PurchaseRate from Prices where Pcode = '"+cboProductName.SelectedValue+"' and Pro_Unit = '6'";
                cmd = new SqlCommand(CheckPrice, MainClass.con);
                ob= cmd.ExecuteScalar();
                MainClass.con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                MainClass.con.Close();
            }
            if (ob != null)
            {
                if (cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "12" || cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "24" || cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "36" ||
                    cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "48" ||
                   cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "60" || cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "72" || cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "84" ||
                  cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "96" || cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "108" || cboUnit.SelectedIndex.ToString() == "4" && txtQuantity.Text.ToString() == "120")
                {
                    cboUnit.SelectedIndex = 5;
                    dx = int.Parse(txtQuantity.Text);
                    txtQuantity.Text = Convert.ToString(dx / 12);

                }
            }
            if (txtQuantity.Text != "" || txtQuantity.Text != "0" || txtPurchaseRate.Text != "")
            {
                float rate = float.Parse(txtPurchaseRate.Text);
                float qty;
                float.TryParse(txtQuantity.Text.Trim(), out qty);
                txtProductTot.Text = Convert.ToString(qty * rate);
            }
            else
            {
                txtProductTot.Text = "0";
            }
        
            
        }

        private void txtDiscountAmount_TextChanged(object sender, EventArgs e)
        {

            if (txtDiscountAmount.Text == "" || txtPurchaseRate.Text == "" || txtQuantity.Text == "")
            {
                txtDiscountAmount.Text = "0";
            }
            else
            {
                if (txtDiscountAmount.Text != "")
                {
                    float qty = float.Parse(txtQuantity.Text);
                    float rate = float.Parse(txtPurchaseRate.Text);
                    float dis = float.Parse(txtDiscountAmount.Text);
                    txtProductTot.Text = Convert.ToString((qty * rate) - dis);
                }
                else
                {
                    float rate = float.Parse(txtPurchaseRate.Text);
                    float qty;
                    float.TryParse(txtQuantity.Text.Trim(), out qty);
                    txtProductTot.Text = Convert.ToString(qty * rate);
                }
            }

        }

        private void CompleteClear()
        {
            ClearForm();
            txtSupplierName.Text = "";
            txtInvoiceType.Text = "";
            txtTotalAmount.Text = "";
            txtPayingAmount.Text = "";
            txtRemainingAmount.Text = "";
            dtInvoice.Value = DateTime.Now;
            dgvPurchaseItems.Rows.Clear();
            cboSupplier.Focus();
        }

        private void ClearForm()
        {
            cboProductName.SelectedIndex = 0;
            cboWarehouse.SelectedIndex = 0;
            cboUnit.SelectedIndex = 0;
            if(txtDiscountAmount.Text != "")
            {
                txtDiscountAmount.TextChanged -= txtDiscountAmount_TextChanged;
                txtDiscountAmount.Clear();
                txtDiscountAmount.TextChanged += txtDiscountAmount_TextChanged;

            }
            if (txtQuantity.Text != "")
            {
                txtQuantity.TextChanged -= txtQuantity_TextChanged;
                txtQuantity.Clear();
                txtQuantity.TextChanged += txtQuantity_TextChanged;

            }

            if (txtPurchaseRate.Text != "")
            {
                txtPurchaseRate.TextChanged -= txtPurchaseRate_TextChanged;
                txtPurchaseRate.Clear();
                txtPurchaseRate.TextChanged += txtPurchaseRate_TextChanged;

            }

            if (txtProductTot.Text != "")
            {
                txtProductTot.TextChanged -= txtProductTot_TextChanged;
                txtProductTot.Clear();
                txtProductTot.TextChanged += txtProductTot_TextChanged;

            }
        }

        private void btnAddCart_Click(object sender, EventArgs e)
        {
            if(txtProductTot.Text == "0" || txtProductTot.Text == "")
            {
                if(txtQuantity.Text == "" )
                {
                    MessageBox.Show("Please Enter Quantity", "Input Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (txtPurchaseRate.Text == "")
                {
                    MessageBox.Show("Please Get Rate", "Input Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                float quantity;
                float discount, totalofproduct, grandtotal, puchaserate;

                float.TryParse(txtQuantity.Text.Trim(), out quantity);
                float.TryParse(txtDiscountAmount.Text.Trim(), out discount);
                float.TryParse(txtPurchaseRate.Text.Trim(), out puchaserate);
                float.TryParse(txtProductTot.Text.Trim(), out totalofproduct);
                float.TryParse(txtGrandTotal.Text.Trim(), out grandtotal);

              

                if (dgvPurchaseItems.Rows.Count == 0)
                {
                    DataGridViewRow createrow = new DataGridViewRow();
                    createrow.CreateCells(dgvPurchaseItems);
                    createrow.Cells[0].Value = cboProductName.SelectedValue;
                    createrow.Cells[1].Value = cboProductName.Text;
                    createrow.Cells[2].Value = cboWarehouse.SelectedValue;
                    createrow.Cells[3].Value = cboWarehouse.Text;
                    createrow.Cells[4].Value = quantity;
                    createrow.Cells[5].Value = cboUnit.SelectedValue;
                    createrow.Cells[6].Value = cboUnit.Text;
                    createrow.Cells[7].Value = puchaserate;
                    createrow.Cells[8].Value = discount;
                    createrow.Cells[9].Value = totalofproduct;
                    createrow.Cells[10].Value = cboSupplier.SelectedValue;
                    createrow.Cells[11].Value = cboSupplier.Text;
                    dgvPurchaseItems.Rows.Add(createrow);
                    ClearForm();
                    cboProductName.Focus();
                }
                else
                {
                    foreach (DataGridViewRow check in dgvPurchaseItems.Rows)
                    {
                        if (Convert.ToString(check.Cells[0].Value) == Convert.ToString(cboProductName.SelectedValue) &&
                            Convert.ToString(check.Cells[5].Value) == Convert.ToString(cboUnit.SelectedValue))
                        {
                            productcheck = true;
                        }
                    }
                    if(productcheck == true)
                    {
                        foreach (DataGridViewRow row in dgvPurchaseItems.Rows)
                        {
                            if(Convert.ToString(row.Cells[0].Value) == Convert.ToString(cboProductName.SelectedValue))
                            {
                                row.Cells["QtyGVC"].Value = float.Parse(row.Cells["QtyGVC"].Value.ToString()) + quantity;
                                row.Cells["TotalGV"].Value = float.Parse(row.Cells["QtyGVC"].Value.ToString()) * float.Parse(row.Cells["PurchaseRateGVC"].Value.ToString());
                                ClearForm();
                            }
                        }
                    }
                    else
                    {
                        if (productcheck == false)
                        {
                            DataGridViewRow createrow = new DataGridViewRow();
                            createrow.CreateCells(dgvPurchaseItems);
                            createrow.Cells[0].Value = cboProductName.SelectedValue;
                            createrow.Cells[1].Value = cboProductName.Text;
                            createrow.Cells[2].Value = cboWarehouse.SelectedValue;
                            createrow.Cells[3].Value = cboWarehouse.Text;
                            createrow.Cells[4].Value = quantity;
                            createrow.Cells[5].Value = cboUnit.SelectedValue;
                            createrow.Cells[6].Value = cboUnit.Text;
                            createrow.Cells[7].Value = puchaserate;
                            createrow.Cells[8].Value = discount;
                            createrow.Cells[9].Value = totalofproduct;
                            createrow.Cells[10].Value = cboSupplier.SelectedValue;
                            createrow.Cells[11].Value = cboSupplier.Text;

                            dgvPurchaseItems.Rows.Add(createrow);
                            ClearForm();
                            cboProductName.Focus();
                        }
                    }
                }
            }
            
        }



        private void txtPurchaseRate_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtProductTot_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvPurchaseItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if(e.ColumnIndex == 12)
                {
                    dgvPurchaseItems.Rows.RemoveAt(dgvPurchaseItems.CurrentRow.Index);
                }
            }
        }

        private void cboSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSupplierName.Text = cboSupplier.Text;
        }

        private void cboInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtInvoiceType.Text = cboInvoiceType.Text;
            if(cboInvoiceType.Text == "Cash" )
            {
                guna2GroupBox2.Visible = false;
            }
            else
            {
                guna2GroupBox2.Visible = true;
            }

        }

        private void dtInvoice_ValueChanged(object sender, EventArgs e)
        {
            txtDated.Text = dtInvoice.Text.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float gross = 0;
            if (dgvPurchaseItems.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvPurchaseItems.Rows)
                {
                    gross += float.Parse(row.Cells["TotalGV"].Value.ToString());
                }
                txtGrandTotal.Text = Convert.ToString(gross);
                txtTotalAmount.Text = Convert.ToString(gross);
                cboSupplier.Text = txtSupplierName.Text;
                if(cboInvoiceType.Text == "Cash")
                {
                    txtPayingAmount.Text = Convert.ToString(gross);
                }
            }
            if(txtPayingAmount.Text == "0" || txtPayingAmount.Text == "")
            {
                txtRemainingAmount.Text = gross.ToString();
            }
           
        }

        public static string INVOICENO = "";
        private void btnFinalize_Click(object sender, EventArgs e)
        {
            if(cboInvoiceType.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Invoice Type");
                return;
            }
            if (cboSupplier.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Supplier");
                return;
            }
            
            string invoiceno = "PUR" + DateTime.Now.ToString("yyddff");
            button1.PerformClick();
            float grandtotal =float.Parse(txtGrandTotal.Text.ToString());
            MainClass.con.Open();
            try
            {
                //Insert Invoice
                string InsertInvoice = "insert into SupplierInvoices (Supplier_ID,PaymentType,InvoiceDate,Warehouse_ID) values (@Supplier_ID,@PaymentType,@InvoiceDate,@Warehouse_ID)";
                SqlCommand cmd = new SqlCommand(InsertInvoice, MainClass.con);
                cmd.Parameters.AddWithValue("@Supplier_ID", dgvPurchaseItems.CurrentRow.Cells[10].Value);
                cmd.Parameters.AddWithValue("@PaymentType", cboInvoiceType.Text);
                cmd.Parameters.AddWithValue("@InvoiceDate", dtInvoice.Value.ToShortDateString());
                cmd.Parameters.AddWithValue("@Warehouse_ID", dgvPurchaseItems.CurrentRow.Cells[2].Value);
                cmd.ExecuteNonQuery();


                ////Get Supplier InvoiceID
                string SupplierInvoiceID = Convert.ToString(MainClass.Retrieve("select MAX(SupplierInvoiceID) from SupplierInvoices").Rows[0][0]);
                if (string.IsNullOrEmpty(SupplierInvoiceID))
                {
                    MessageBox.Show("Please Check The Error or Try Again");
                    return;
                }

                //Inserting Purchase
                string InserPurchase = "insert into Purchases (InvoiceNo,SupplierInvoice_ID,GrandTotal) values (@InvoiceNo,@SupplierInvoice_ID,@GrandTotal)";
                SqlCommand cmd1 = new SqlCommand(InserPurchase, MainClass.con);
                cmd1.Parameters.AddWithValue("@InvoiceNo", invoiceno);
                cmd1.Parameters.AddWithValue("@SupplierInvoice_ID", SupplierInvoiceID);
                cmd1.Parameters.AddWithValue("@GrandTotal", grandtotal);
                cmd1.ExecuteNonQuery();



                ////Get PurchaseID
                string PurchaseID = Convert.ToString(MainClass.Retrieve("select MAX(PurchaseID) from Purchases").Rows[0][0]);
                if (string.IsNullOrEmpty(PurchaseID))
                {
                    MessageBox.Show("Please Check The Error or Try Again");
                    return;
                }

                //Inserting Products
                foreach (DataGridViewRow products in dgvPurchaseItems.Rows)
                {
                    string InsertPurchaseDetails = "insert into PurchaseDetails (Purchase_ID,SupplerInvoice_ID,Product_ID,PurchaseQty,PurchaseUnit_ID,PurchaseRate,Discount,TotalofProduct) values (@Purchase_ID,@SupplerInvoice_ID,@Product_ID,@PurchaseQty,@PurchaseUnit_ID,@PurchaseRate,@Discount,@TotalofProduct)";
                    SqlCommand cmd2 = new SqlCommand(InsertPurchaseDetails, MainClass.con);
                    cmd2.Parameters.AddWithValue("@Purchase_ID", PurchaseID);
                    cmd2.Parameters.AddWithValue("@SupplerInvoice_ID", SupplierInvoiceID);
                    cmd2.Parameters.AddWithValue("@Product_ID", products.Cells[0].Value);
                    cmd2.Parameters.AddWithValue("@PurchaseQty", products.Cells[4].Value);
                    cmd2.Parameters.AddWithValue("@PurchaseUnit_ID", products.Cells[5].Value);
                    cmd2.Parameters.AddWithValue("@PurchaseRate", products.Cells[7].Value);
                    cmd2.Parameters.AddWithValue("@Discount", products.Cells[8].Value);
                    cmd2.Parameters.AddWithValue("@TotalofProduct", products.Cells[9].Value);
                    cmd2.ExecuteNonQuery();
                }


                foreach (DataGridViewRow item in dgvPurchaseItems.Rows)
                {
                    if (item.Cells[2].Value.ToString() == "3")
                    {
                        float q;
                        string CheckStock = "select s.sh_Qty as 'Quantity' from ShopStocks s where s.sh_Pcode = '" + Convert.ToInt32(item.Cells["PcodeGVC"].Value.ToString()) + "'and s.sh_Unit = '" + Convert.ToInt32(item.Cells["UnitID"].Value.ToString()) + "'";
                        cmd = new SqlCommand(CheckStock, MainClass.con);
                        object ob = cmd.ExecuteScalar();

                        if (ob != null)
                        {
                            //Update Quantity into Stocks
                            if (item.Cells[5].Value.ToString() == "1")
                            {
                                q = Convert.ToInt32(ob);
                                q += float.Parse(item.Cells["QtyGVC"].Value.ToString()) * 1000;
                                MainClass.UpdateShopStock(Convert.ToInt32(item.Cells["PcodeGVC"].Value.ToString()), q);
                            }
                            else if (item.Cells[5].Value.ToString() == "2")
                            {
                                q = Convert.ToInt32(ob);
                                q += float.Parse(item.Cells["QtyGVC"].Value.ToString()) * 25;
                                MainClass.UpdateShopStock(Convert.ToInt32(item.Cells["PcodeGVC"].Value.ToString()), q);
                            }
                            else if (item.Cells[5].Value.ToString() == "6")
                            {
                                q = Convert.ToInt32(ob);
                                q += float.Parse(item.Cells["QtyGVC"].Value.ToString()) * 12;
                                MainClass.UpdateShopStock(Convert.ToInt32(item.Cells["PcodeGVC"].Value.ToString()), q);
                            }
                            else
                            {
                                q = Convert.ToInt32(ob);
                                q += float.Parse(item.Cells["QtyGVC"].Value.ToString());
                                MainClass.UpdateShopStock(Convert.ToInt32(item.Cells["PcodeGVC"].Value.ToString()), q);
                            }
                        }
                        else
                        {
                            //Insert Stocks into Stocks

                            if (item.Cells[2].Value.ToString() == "3")
                            {
                                bool okay = false;
                                string InsertStocks = "insert into ShopStocks (sh_Pcode,sh_Qty,sh_Unit) values (@sh_Pcode,@sh_Qty,@sh_Unit)";
                                SqlCommand cmd4 = new SqlCommand(InsertStocks, MainClass.con);
                                void check()
                                {
                                    if (item.Cells[5].Value.ToString() == "1")
                                    {
                                        Int32 quantity = Convert.ToInt32(item.Cells[4].Value) * 1000;
                                        cmd4.Parameters.AddWithValue("@sh_Qty", quantity);
                                        item.Cells[5].Value = "3";
                                        cmd4.Parameters.AddWithValue("@sh_Unit", item.Cells[5].Value);
                                        okay = true;
                                    }

                                    else if (item.Cells[5].Value.ToString() == "2")
                                    {
                                        Int32 quantity = Convert.ToInt32(item.Cells[4].Value) * 25;
                                        cmd4.Parameters.AddWithValue("@sh_Qty", quantity);
                                        item.Cells[5].Value = "3";
                                        cmd4.Parameters.AddWithValue("@sh_Unit", item.Cells[5].Value);
                                        okay = true;
                                    }

                                    else if (item.Cells[5].Value.ToString() == "6")
                                    {
                                        Int32 quantity = Convert.ToInt32(item.Cells[4].Value) * 12;
                                        cmd4.Parameters.AddWithValue("@sh_Qty", quantity);
                                        item.Cells[5].Value = "5";
                                        cmd4.Parameters.AddWithValue("@sh_Unit", item.Cells[5].Value);
                                        okay = true;
                                    }
                                    else
                                    {
                                        okay = false;
                                    }
                                }
                                cmd4.Parameters.AddWithValue("@sh_Pcode", item.Cells[0].Value);
                                check();
                                if (okay == false)
                                {
                                    cmd4.Parameters.AddWithValue("@sh_Qty", item.Cells[4].Value);
                                    cmd4.Parameters.AddWithValue("@sh_Unit", item.Cells[5].Value);
                                }
                                cmd4.ExecuteNonQuery();
                            }
                        }
                    }

                    else
                    {
                        float q;
                        int unitchanged = 0;
                        //Exisiting Stock Check

                        if (item.Cells[5].Value.ToString() == "1")
                        {
                            item.Cells[5].Value = "3";
                            unitchanged = 1;
                        }
                        else if (item.Cells[5].Value.ToString() == "2")
                        {
                            item.Cells[5].Value = "3";
                            unitchanged = 2;
                        }

                        else if (item.Cells[5].Value.ToString() == "6")
                        {
                            item.Cells[5].Value = "5";
                            unitchanged = 3;
                        }
                        string CheckStock = "select s.st_Qty as 'Quantity' from Stocks s where s.st_Pcode = '" + Convert.ToInt32(item.Cells["PcodeGVC"].Value.ToString()) + "' ";
                        cmd = new SqlCommand(CheckStock, MainClass.con);
                        object ob = cmd.ExecuteScalar();

                        if (ob != null)
                        {

                            //Update Quantity into Stocks
                            if (unitchanged == 1)
                            {
                                q = Convert.ToInt32(ob);
                                q += float.Parse(item.Cells["QtyGVC"].Value.ToString()) * 1000;
                                MainClass.UpdateStock(Convert.ToInt32(item.Cells["PcodeGVC"].Value.ToString()), q);
                                ad = 2;
                            }
                            else if (unitchanged == 2)
                            {
                                q = Convert.ToInt32(ob);
                                q += float.Parse(item.Cells["QtyGVC"].Value.ToString()) * 25;
                                MainClass.UpdateStock(Convert.ToInt32(item.Cells["PcodeGVC"].Value.ToString()), q);
                                ad = 1;
                            }

                            else if (unitchanged == 2)
                            {
                                q = Convert.ToInt32(ob);
                                q += float.Parse(item.Cells["QtyGVC"].Value.ToString()) * 12;
                                MainClass.UpdateStock(Convert.ToInt32(item.Cells["PcodeGVC"].Value.ToString()), q);
                                ad = 4;
                            }
                            else
                            {
                                q = Convert.ToInt32(ob);
                                q += float.Parse(item.Cells["QtyGVC"].Value.ToString());
                                MainClass.UpdateStock(Convert.ToInt32(item.Cells["PcodeGVC"].Value.ToString()), q);
                                ad = 3;
                            }
                        }

                        else
                        {

                            bool okay = false;
                            string InsertStocks = "insert into Stocks (st_Pcode,st_Qty,st_Unit) values (@st_Pcode,@st_Qty,@st_Unit)";
                            SqlCommand cmd4 = new SqlCommand(InsertStocks, MainClass.con);
                            void check()
                            {
                                if (item.Cells[5].Value.ToString() == "1" || unitchanged == 1)
                                {
                                    float quantity = float.Parse(item.Cells[4].Value.ToString()) * 1000;
                                    cmd4.Parameters.AddWithValue("@st_Qty", quantity);
                                    item.Cells[5].Value = "3";
                                    cmd4.Parameters.AddWithValue("@st_Unit", item.Cells[5].Value);
                                    okay = true;
                                    unitchanged = 0;
                                }
                                else if (item.Cells[5].Value.ToString() == "2" || unitchanged == 2)
                                {
                                    float quantity = float.Parse(item.Cells[4].Value.ToString()) * 25;
                                    cmd4.Parameters.AddWithValue("@st_Qty", quantity);
                                    item.Cells[5].Value = "3";
                                    cmd4.Parameters.AddWithValue("@st_Unit", item.Cells[5].Value);
                                    okay = true;
                                    unitchanged = 0;
                                }

                                else if (item.Cells[5].Value.ToString() == "6" || unitchanged == 3)
                                {
                                    float quantity = float.Parse(item.Cells[4].Value.ToString()) * 12;
                                    cmd4.Parameters.AddWithValue("@st_Qty", quantity);
                                    item.Cells[5].Value = "5";
                                    cmd4.Parameters.AddWithValue("@st_Unit", item.Cells[5].Value);
                                    okay = true;
                                    unitchanged = 0;
                                }
                                else
                                {
                                    okay = false;
                                }
                            }
                            cmd4.Parameters.AddWithValue("@st_Pcode", item.Cells[0].Value);
                            check();
                            if (okay == false)
                            {
                                cmd4.Parameters.AddWithValue("@st_Qty", item.Cells[4].Value);
                                cmd4.Parameters.AddWithValue("@st_Unit", item.Cells[5].Value);
                            }
                            cmd4.ExecuteNonQuery();


                        }
                    }

                }   
                




                

                //Inserting Supplier Payments
                string InsertPayment = "insert into SupplierLedgers (SupplierInvoice_ID,Supplier_ID,InvoiceType,InvoiceDate,TotalAmount,PaidAmount,RemaingBalance) values(@SupplierInvoice_ID,@Supplier_ID,@InvoiceType,@InvoiceDate,@TotalAmount,@PaidAmount,@RemaingBalance)";
                SqlCommand cmd3 = new SqlCommand(InsertPayment, MainClass.con);
                cmd3.Parameters.AddWithValue("@SupplierInvoice_ID", SupplierInvoiceID);
                cmd3.Parameters.AddWithValue("@Supplier_ID", dgvPurchaseItems.CurrentRow.Cells[10].Value);
                cmd3.Parameters.AddWithValue("@InvoiceType", txtInvoiceType.Text);
                cmd3.Parameters.AddWithValue("@InvoiceDate", dtInvoice.Value.ToShortDateString());
                cmd3.Parameters.AddWithValue("@TotalAmount", float.Parse(txtTotalAmount.Text));
                cmd3.Parameters.AddWithValue("@PaidAmount", float.Parse(txtPayingAmount.Text));
                cmd3.Parameters.AddWithValue("@RemaingBalance", float.Parse(txtRemainingAmount.Text));
                cmd3.ExecuteNonQuery();


                cmd = new SqlCommand("insert into SupplierReport (Supplierer_ID,SaleInvoiceNo,SaleInvoiceDate,SaleTotalAmount,SaleLastPaid,SaleLastPayingDate,SaleBalance) values (@Supplierer_ID,@SaleInvoiceNo,@SaleInvoiceDate,@SaleTotalAmount,@SaleLastPaid,@SaleLastPayingDate,@SaleBalance)", MainClass.con);
                cmd.Parameters.AddWithValue("@Supplierer_ID", cboSupplier.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@SaleInvoiceDate", dtInvoice.Value.ToShortDateString());
                cmd.Parameters.AddWithValue("@SaleInvoiceNo", invoiceno);
                cmd.Parameters.AddWithValue("@SaleTotalAmount", float.Parse(txtTotalAmount.Text));
                cmd.Parameters.AddWithValue("@SaleLastPaid", float.Parse(txtPayingAmount.Text));
                cmd.Parameters.AddWithValue("@SaleLastPayingDate", dtInvoice.Value.ToShortDateString());
                cmd.Parameters.AddWithValue("@SaleBalance", float.Parse(txtRemainingAmount.Text));
                cmd.ExecuteNonQuery();

                MainClass.con.Close();
                MessageBox.Show("Purchase Successful");
                INVOICENO = invoiceno;
                PurchaseReportForm prf = new PurchaseReportForm();
                prf.Show();
                CompleteClear();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MainClass.con.Close();
            }
        }

        private void txtPayingAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtPayingAmount.Text != "" || txtPayingAmount.Text == "0")
            {
                float tot = float.Parse(txtTotalAmount.Text);
                float paying = float.Parse(txtPayingAmount.Text);
                txtRemainingAmount.Text = Convert.ToString(tot - paying);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            CompleteClear();
        }

        private void cboProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProductName.SelectedIndex != 0)
            {
                try
                {
                    MainClass.con.Open();
                    SqlCommand cmd1;
                    string check2 = "select count(*)  from Stocks where st_Unit = '3' and st_Pcode = '" + cboProductName.SelectedValue + "'";
                    cmd1 = new SqlCommand(check2, MainClass.con);
                    ob1 = cmd1.ExecuteScalar();
                    MainClass.con.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                    MainClass.con.Close();
                }


                if (ob1.ToString() != "0")
                {
                    quan = 1;
                }
                else
                {
                    quan = 0;
                }
            }

            try
            {
                SqlCommand cmd = null;
                MainClass.con.Open();
                if (quan == 1)
                {
                    cmd = new SqlCommand("select sum(st_Qty*1.0/1000) from Stocks s inner join Products p on p.Pcode = s.st_Pcode where p.ProductName = '" + cboProductName.Text + "'", MainClass.con);

                }
                else
                {
                    cmd = new SqlCommand("select sum(st_Qty*1.0) from Stocks s inner join Products p on p.Pcode = s.st_Pcode where p.ProductName = '" + cboProductName.Text + "'", MainClass.con);

                }
                object ob2 = cmd.ExecuteScalar();
                if (ob2 != null)
                {
                    txtStockInfo.Text = ob2.ToString();
                }
                else
                {
                    txtStockInfo.Text = "0";
                }
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MainClass.con.Close();
            }


        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Purchase Invoice", new Font("Arial", 25, FontStyle.Regular), Brushes.Black, new Point(100, 10));
            e.Graphics.DrawString("Date: " + dtInvoice.Value.ToShortDateString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(20, 70));
            e.Graphics.DrawString("Supplier: " + cboSupplier.Text.ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(20, 90));
            e.Graphics.DrawString("___________________________________________________________ ", new Font("Helvetica", 12, FontStyle.Regular), Brushes.Black, new Point(0, 95));
            e.Graphics.DrawString("Product ", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(25, 115));
            e.Graphics.DrawString("QTY ", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(205, 115));
            e.Graphics.DrawString("Unit ", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(255, 115));
            e.Graphics.DrawString("Rate ", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(320, 115));
            e.Graphics.DrawString("Disc ", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(380, 115));
            e.Graphics.DrawString("Amount ", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(450, 115));
            e.Graphics.DrawString("___________________________________________________________ ", new Font("Helvetica", 12, FontStyle.Regular), Brushes.Black, new Point(0, 115));
            int pos = 140;
            foreach (DataGridViewRow item in dgvPurchaseItems.Rows)
            {
                e.Graphics.DrawString(item.Cells[1].Value.ToString(), new Font("Arial", 9, FontStyle.Regular), Brushes.Black, new Point(25, pos));    //Product
                e.Graphics.DrawString(item.Cells[4].Value.ToString(), new Font("Arial", 9, FontStyle.Regular), Brushes.Black, new Point(210, pos));    // Qty
                e.Graphics.DrawString(item.Cells[6].Value.ToString(), new Font("Arial", 9, FontStyle.Regular), Brushes.Black, new Point(255, pos));    //Unit
                e.Graphics.DrawString(item.Cells[7].Value.ToString(), new Font("Arial", 9, FontStyle.Regular), Brushes.Black, new Point(320, pos));    //Rate
                e.Graphics.DrawString(item.Cells[8].Value.ToString(), new Font("Arial", 9, FontStyle.Regular), Brushes.Black, new Point(390, pos));    //Discount
                e.Graphics.DrawString(item.Cells[9].Value.ToString(), new Font("Arial", 9, FontStyle.Regular), Brushes.Black, new Point(450, pos));    //Total
                pos += 20;
            }
            e.Graphics.DrawString("Total Amount: " + txtGrandTotal.Text.ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(250, 65));
            if (cboInvoiceType.Text != "Cash")
            {
                e.Graphics.DrawString("Paying Amount: " + txtPayingAmount.Text.ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(250, 80));
                e.Graphics.DrawString("Remaining Balance: " + txtRemainingAmount.Text.ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(250, 93));
            }



        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printDocument1.DefaultPageSettings.PaperSize = new PaperSize("Custom", 520, 820);
            printPreviewDialog1.ShowDialog();
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void cbPrint_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cboSupplier_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Tab)
            {
                cboProductName.Focus();
            }
        }
    }
}
