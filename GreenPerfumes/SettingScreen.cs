using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace GreenPerfumes
{
    public partial class SettingScreen : Form
    {
        int edit = 0;
        ProductEntry frm = new ProductEntry();
        public SettingScreen()
        {
            InitializeComponent();
        }

        void ShowUsers(DataGridView dgv, DataGridViewColumn NameGV, DataGridViewColumn UsernameGV, DataGridViewColumn PasswordGV, DataGridViewColumn RoleGv)
        {
            try
            {
                MainClass.con.Open();
                SqlCommand cmd = new SqlCommand("select * from Users order by Name", MainClass.con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                NameGV.DataPropertyName = dt.Columns["Name"].ToString();
                UsernameGV.DataPropertyName = dt.Columns["Username"].ToString();
                PasswordGV.DataPropertyName = dt.Columns["Password"].ToString();
                RoleGv.DataPropertyName = dt.Columns["Role"].ToString();
                dgv.DataSource = dt;
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MessageBox.Show(ex.Message);
            }
        }
        void Clear()
        {
            txtName.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            cboRole.SelectedIndex = 0;
        }

        private void SettingScreen_Load(object sender, EventArgs e)
        {
            ShowUsers(Datagridview1, NameGV, UsernameGV, PasswordGV, RoleGV);
            cboRole.SelectedIndex = 0;
            MainClass.HideAllTabsOnTabControl(tabControl1);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (edit == 0)
            {
                if (txtName.Text == "" && txtUsername.Text == "" && txtPassword.Text == "" && cboRole.SelectedIndex == 0)
                {
                    MessageBox.Show("Please Input Details");
                }
                else
                {
                    if (txtPassword.Text != txtConfirmPassword.Text)
                    {
                        MessageBox.Show("Password Mismatched");
                    }
                    else
                    {
                        try
                        {
                            MainClass.con.Open();
                            SqlCommand cmd = new SqlCommand("insert into Users (Name,Username,Password,Role) values(@Name,@Username,@Password,@Role)", MainClass.con);
                            cmd.Parameters.AddWithValue("@Name", txtName.Text);
                            cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                            cmd.Parameters.AddWithValue("@Password", txtConfirmPassword.Text);
                            cmd.Parameters.AddWithValue("@Role", cboRole.SelectedItem);
                            cmd.ExecuteNonQuery();
                            MainClass.con.Close();
                            MessageBox.Show("User Inserted Successfully.");
                            Clear();
                            ShowUsers(Datagridview1, NameGV, UsernameGV, PasswordGV, RoleGV);
                        }
                        catch (Exception ex)
                        {
                            MainClass.con.Close();
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            else
            {
                if(edit == 1)
                {
                    if (txtPassword.Text != txtConfirmPassword.Text)
                    {
                        MessageBox.Show("Password Mismatched");
                    }
                    else
                    {
                        try
                        {
                            MainClass.con.Open();
                            SqlCommand cmd = new SqlCommand("update Users set Name = @Name, @Password = @Password, Role = @Role where Username = @Username" ,MainClass.con);
                            cmd.Parameters.AddWithValue("@Name", txtName.Text);
                            cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                            cmd.Parameters.AddWithValue("@Password", txtConfirmPassword.Text);
                            cmd.Parameters.AddWithValue("@Role", cboRole.SelectedItem);
                            cmd.ExecuteNonQuery();
                            MainClass.con.Close();
                            MessageBox.Show("User Updated Successfully.");
                            Clear();
                            ShowUsers(Datagridview1, NameGV, UsernameGV, PasswordGV, RoleGV);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "" || txtUsername.Text != "" || txtPassword.Text != "" || cboRole.SelectedIndex != 0 || txtConfirmPassword.Text != "")
            {
                Clear();
            }
            else
            {
                Dashboard ds = new Dashboard();
                MainClass.showWindow(ds, this, MDI.ActiveForm);
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dashboard ds = new Dashboard();
            MainClass.showWindow(ds, this, MDI.ActiveForm);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Datagridview1 != null)
            {
                if(Datagridview1.Rows.Count > 0)
                {
                    if(Datagridview1.SelectedRows.Count == 1)
                    {
                        try
                        {
                            MainClass.con.Open();
                            SqlCommand cmd = new SqlCommand("delete from Users where Username = @Username", MainClass.con);
                            cmd.Parameters.AddWithValue("@Username", Datagridview1.CurrentRow.Cells[1].Value.ToString());
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Record Deleted Successfully");
                            MainClass.con.Close();
                            ShowUsers(Datagridview1, NameGV, UsernameGV, PasswordGV, RoleGV);
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
            txtName.Text = Datagridview1.CurrentRow.Cells[0].Value.ToString();
            txtUsername.Text = Datagridview1.CurrentRow.Cells[1].Value.ToString();
            txtPassword.Text = Datagridview1.CurrentRow.Cells[2].Value.ToString();
            txtConfirmPassword.Text = Datagridview1.CurrentRow.Cells[2].Value.ToString();
            cboRole.SelectedItem = Datagridview1.CurrentRow.Cells[3].Value.ToString();
        }



        private void btnUserSettings_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void btnDatabaseSettings_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }


        public static DataTable GetDataTabletFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                if (csv_file_path.EndsWith(".csv"))
                {
                    using (Microsoft.VisualBasic.FileIO.TextFieldParser csvReader = new Microsoft.VisualBasic.FileIO.TextFieldParser(csv_file_path))
                    {
                        csvReader.SetDelimiters(new string[] { "," });
                        csvReader.HasFieldsEnclosedInQuotes = true;
                        //read column
                        string[] colFields = csvReader.ReadFields();
                        foreach (string column in colFields)
                        {
                            DataColumn datecolumn = new DataColumn(column);
                            datecolumn.AllowDBNull = true;
                            csvData.Columns.Add(datecolumn);
                        }
                        while (!csvReader.EndOfData)
                        {
                            string[] fieldData = csvReader.ReadFields();
                            for (int i = 0; i < fieldData.Length; i++)
                            {
                                if (fieldData[i] == "")
                                {
                                    fieldData[i] = null;
                                }
                            }
                            csvData.Rows.Add(fieldData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return csvData;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void btnBackupBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtBackup.Text = fbd.SelectedPath;
                btnBackupBrowse.Enabled = true;
            }
        }

        private void btnRestoreBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SQL SERVER database backup files|*.bak";
            ofd.Title = "Database Restore";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtRestore.Text = ofd.FileName;
                btnRestore.Enabled = true;
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {

            {

                String Database = MainClass.con.Database.ToString();
                try
                {
                    MainClass.con.Open();
                    if (txtBackup.Text == "")
                    {
                        MessageBox.Show("Please Locate The Backup File", "Error",MessageBoxButtons.OK);
                        return;
                    }
                    else
                    {
                        string q = "BACKUP DATABASE[" + Database + "] TO DISK = '" + txtBackup.Text + "\\" + "Database" + "-" + DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + ".bak'";
                        SqlCommand cmd = new SqlCommand(q, MainClass.con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data Backup Success", "Success", MessageBoxButtons.OK);
                    }
                    MainClass.con.Close();
                }

                catch (Exception ex)
                {
                    MainClass.con.Close();
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {

            {
                MainClass.con.Open();
                String Database = MainClass.con.Database.ToString();
                try
                {
                    string sql1 = string.Format("ALTER DATABASE [" + Database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                    SqlCommand cmd = new SqlCommand(sql1, MainClass.con);
                    cmd.ExecuteNonQuery();

                    string sql2 = string.Format("USE MASTER RESTORE DATABASE [" + Database + "] FROM DISK='" + txtRestore.Text + "' WITH REPLACE;");
                    SqlCommand cmd2 = new SqlCommand(sql2, MainClass.con);
                    cmd2.ExecuteNonQuery();

                    string sql3 = string.Format("ALTER DATABASE [" + Database + "] SET MULTI_USER");
                    SqlCommand cmd3 = new SqlCommand(sql3, MainClass.con);
                    cmd3.ExecuteNonQuery();

                    MessageBox.Show("Database Restored successfully", "Restore Database successs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally { MainClass.con.Close(); }
            }
        }





      
    }
}
