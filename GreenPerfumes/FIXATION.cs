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
        private void ShowSaleData(DataGridView dgv , DataGridViewColumn ID, DataGridViewColumn InovoiceID, DataGridViewColumn Idate, DataGridViewColumn INo, DataGridViewColumn Grandtotal, DataGridViewColumn PaidAmount, DataGridViewColumn balannce)
        {
             cmd = new SqlCommand("select ci.Customer_ID,s.CustomerInvoice_ID,ci.InvoiceDate,s.InvoiceNo,s.GrandTotal,cl.PaidAmount,cl.RemainingBalance from Sales s inner join CustomerInvoices ci on ci.CustomerInvoiceID = s.CustomerInvoice_ID inner join CustomerLedgers cl on cl.CustomerInvoice_ID = ci.CustomerInvoiceID left join CustomerReport cr on cr.SaleInvoiceNo = s.InvoiceNo WHERE cr.SaleInvoiceNo Is Null order by ci.InvoiceDate", MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ID.DataPropertyName = dt.Columns["Customer_ID"].ToString();
            InovoiceID.DataPropertyName = dt.Columns["CustomerInvoice_ID"].ToString();
            Idate.DataPropertyName = dt.Columns["InvoiceDate"].ToString();
            INo.DataPropertyName = dt.Columns["InvoiceNo"].ToString();
            Grandtotal.DataPropertyName = dt.Columns["GrandTotal"].ToString();
            PaidAmount.DataPropertyName = dt.Columns["PaidAmount"].ToString();
            balannce.DataPropertyName = dt.Columns["RemainingBalance"].ToString();
            dgv.DataSource = dt;

        }



        private void ShowOpening(DataGridView dgv, DataGridViewColumn VID,  DataGridViewColumn Vdate, DataGridViewColumn BillBookNo, DataGridViewColumn BillNo, DataGridViewColumn Grandtotal, DataGridViewColumn PaidAmount, DataGridViewColumn balannce, DataGridViewColumn customID,DataGridViewColumn address)
        {
            cmd = new SqlCommand(" select o.VoucherID,o.VoucherDate,o.BillBookNo,o.BillNo,o.TotalAmount,o.PaidAmount,o.DueAmount,o.PersonName_ID,o.Address from OpeningAccounts o  left join CustomerReport cr on cr.OVoucherID = o.VoucherID where cr.OVoucherID is null", MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            VID.DataPropertyName = dt.Columns["VoucherID"].ToString();
            Vdate.DataPropertyName = dt.Columns["VoucherDate"].ToString();
            BillBookNo.DataPropertyName = dt.Columns["BillBookNo"].ToString();
            BillNo.DataPropertyName = dt.Columns["BillNo"].ToString();
            Grandtotal.DataPropertyName = dt.Columns["TotalAmount"].ToString();
            PaidAmount.DataPropertyName = dt.Columns["PaidAmount"].ToString();
            balannce.DataPropertyName = dt.Columns["DueAmount"].ToString();
            customID.DataPropertyName = dt.Columns["PersonName_ID"].ToString();
            address.DataPropertyName = dt.Columns["Address"].ToString();
            dgv.DataSource = dt;

        }

        private void FIXATION_Load(object sender, EventArgs e)
        {
            ShowSaleData(dataGridView1, CustomerID, CustomerInvoiceID, InvoiceDate, InvoiceNo, GrandTotal, PaidAmount, RemainingBalance);
            ShowOpening(dataGridView2, VoucherID, VoucherDate, BillBookNo, BillNo, totalAmount, PaidAmountG, DueAmount,CustomerIDd,Adress);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainClass.con.Open();
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                cmd = new SqlCommand("select cr.SaleInvoiceNo from CustomerReport cr where cr.SaleInvoiceNo = '" + item.Cells["InvoiceNo"].Value.ToString() + "' ", MainClass.con);
                object ob = cmd.ExecuteScalar();
                if(ob != null)
                { 
                    continue;
                }

                cmd = new SqlCommand("insert into CustomerReport (Customer_ID,SaleInvoiceNo,SaleInvoiceDate,SaleTotalAmount,SaleLastPaid,SaleLastPayingDate,SaleBalance) values (@Customer_ID,@SaleInvoiceNo,@SaleInvoiceDate,@SaleTotalAmount,@SaleLastPaid,@SaleLastPayingDate,@SaleBalance)", MainClass.con);
                cmd.Parameters.AddWithValue("@Customer_ID", item.Cells["CustomerID"].Value.ToString());
                cmd.Parameters.AddWithValue("@SaleInvoiceDate", item.Cells["InvoiceDate"].Value.ToString());
                cmd.Parameters.AddWithValue("@SaleInvoiceNo", item.Cells["InvoiceNo"].Value.ToString());
                cmd.Parameters.AddWithValue("@SaleTotalAmount", item.Cells["GrandTotal"].Value.ToString());
                cmd.Parameters.AddWithValue("@SaleLastPaid", item.Cells["PaidAmount"].Value.ToString());
                cmd.Parameters.AddWithValue("@SaleLastPayingDate", item.Cells["InvoiceDate"].Value.ToString());
                cmd.Parameters.AddWithValue("@SaleBalance", item.Cells["RemainingBalance"].Value.ToString());
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("DONE");
            MainClass.con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainClass.con.Open();
            foreach (DataGridViewRow item in dataGridView2.Rows)
            {
                cmd = new SqlCommand("select cr.OVoucherID from CustomerReport cr where cr.OVoucherID = '" + item.Cells["VoucherID"].Value.ToString() + "' ", MainClass.con);
                object ob = cmd.ExecuteScalar();
                if (ob != null)
                {
                    continue;
                }
                cmd = new SqlCommand("insert into CustomerReport (Customer_ID,OVoucherID,OVoucherDate,OBillBookNo,OBillNo,OTotalAmount,OLastPaid,OPayingDate,OBalance,Address)" +
                            "values (@Customer_ID,@OVoucherID,@OVoucherDate,@OBillBookNo,@OBillNo,@OTotalAmount,@OLastPaid,@OPayingDate,@OBalance,@Address)", MainClass.con);
                
                cmd.Parameters.AddWithValue("@OVoucherID", item.Cells["VoucherID"].Value.ToString());
                cmd.Parameters.AddWithValue("@OVoucherDate", item.Cells["VoucherDate"].Value.ToString());
                cmd.Parameters.AddWithValue("@OBillBookNo", item.Cells["BillBookNo"].Value.ToString());
                cmd.Parameters.AddWithValue("@OBillNo", item.Cells["BillNo"].Value.ToString());
                cmd.Parameters.AddWithValue("@OTotalAmount", item.Cells["totalAmount"].Value.ToString());
                cmd.Parameters.AddWithValue("@OLastPaid", item.Cells["PaidAmountG"].Value.ToString());
                cmd.Parameters.AddWithValue("@OPayingDate", item.Cells["VoucherDate"].Value.ToString());
                cmd.Parameters.AddWithValue("@OBalance", item.Cells["DueAmount"].Value.ToString());
                cmd.Parameters.AddWithValue("@Customer_ID", item.Cells["CustomerIDd"].Value.ToString());
                cmd.Parameters.AddWithValue("@Address", item.Cells["Adress"].Value.ToString());
                cmd.ExecuteNonQuery();

            }
            MessageBox.Show("DONE");
            MainClass.con.Close();
        }
    }
}
