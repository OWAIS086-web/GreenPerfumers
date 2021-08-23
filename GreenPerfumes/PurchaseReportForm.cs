using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenPerfumes
{
    public partial class PurchaseReportForm : Form
    {
        ReportDocument rd;
        public PurchaseReportForm()
        {
            InitializeComponent();
        }

        private void PurchaseReportForm_Load(object sender, EventArgs e)
        {
            rd = new ReportDocument();
            if (PurchaseInvoice.INVOICENO == "")
            {
                MainClass.ShowReportsp(rd, crystalReportViewer1, "GetPurchaseRecieptWRTSuipplierInvoiceID", "@SupplierInvoice_ID",AllReports.Invoice_ID);
            }
            else
            {
                MainClass.ShowReportsp(rd, crystalReportViewer1, "GetPurchaseReciept", "@PurchaseInvoiceNo", PurchaseInvoice.INVOICENO);
            }
        }

        private void PurchaseReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (rd != null)
            {
                rd.Close();
            }
        }
    }
}
