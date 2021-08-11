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
    public partial class ShopInventory : Form
    {
        public ShopInventory()
        {
            InitializeComponent();
        }

   

        private void LoadGodownInventory(DataGridView dgv, DataGridViewColumn pname, DataGridViewColumn quantity, DataGridViewColumn unit, string data = null)
        {
            try
            {
                SqlCommand cmd = null;
                MainClass.con.Open();
                if (data != null)
                {
                     cmd = new SqlCommand("select p.ProductName,s.st_Qty,u.UnitName from Stocks s inner join Units u on u.UnitID = s.st_Unit inner join Products p on p.Pcode = s.st_Pcode where p.ProductName like '%" + data + "%' and s.st_Qty > 0 group by p.ProductName,s.st_Qty,u.UnitName ", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("select p.ProductName,s.st_Qty,u.UnitName from Stocks s inner join Units u on u.UnitID = s.st_Unit inner join Products p on p.Pcode = s.st_Pcode where s.st_Qty > 0 group by p.ProductName,s.st_Qty,u.UnitName", MainClass.con);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                pname.DataPropertyName = dt.Columns["ProductName"].ToString();
                quantity.DataPropertyName = dt.Columns["st_Qty"].ToString();
                unit.DataPropertyName = dt.Columns["UnitName"].ToString();
                dgv.DataSource = dt;
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadShopInventory(DataGridView dgv, DataGridViewColumn pname, DataGridViewColumn quantity, DataGridViewColumn unit, string data = null)
        {
            try
            {
                SqlCommand cmd = null;
                MainClass.con.Open();
                if (data != null)
                {
                    cmd = new SqlCommand("select p.ProductName,s.sh_Qty,u.UnitName from ShopStocks s inner join Units u on u.UnitID = s.sh_Unit inner join Products p on p.Pcode = s.sh_Pcode where p.ProductName like '%" + data + "%' and s.sh_Qty > 0 group by p.ProductName,s.sh_Qty,u.UnitName ", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("select p.ProductName,s.sh_Qty,u.UnitName from ShopStocks s inner join Units u on u.UnitID = s.sh_Unit inner join Products p on p.Pcode = s.sh_Pcode where s.sh_Qty > 0 group by p.ProductName,s.sh_Qty,u.UnitName", MainClass.con);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                pname.DataPropertyName = dt.Columns["ProductName"].ToString();
                quantity.DataPropertyName = dt.Columns["sh_Qty"].ToString();
                unit.DataPropertyName = dt.Columns["UnitName"].ToString();
                dgv.DataSource = dt;
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MessageBox.Show(ex.Message);
            }
        }




        private void btnClose_Click(object sender, EventArgs e)
        {
            Dashboard isi = new Dashboard();
            MainClass.showWindow(isi, this, MDI.ActiveForm);
        }

   

        private void ShopInventory_Load(object sender, EventArgs e)
        {
            LoadGodownInventory(DGVGodown, GPname, gQty, gUnit);
            LoadShopInventory(DGVShop, SPname, SQty, SUnit);
            
       
        }

        

        
     

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadShopInventory(DGVShop, SPname, SQty, SUnit,txtSearch.Text);
        }

        private void dgvInventory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            decimal decValue;

            if (e.Value == null)

                return;

            if (decimal.TryParse(e.Value.ToString(), out decValue) == false)

                return;



            e.Value = Math.Round(decValue, 2);
        }

        private void txtGodownSearch_TextChanged(object sender, EventArgs e)
        {
            LoadGodownInventory(DGVGodown, GPname, gQty, gUnit, txtGodownSearch.Text);
        }
    }
}
