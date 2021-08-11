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
    public partial class LoginScreen : Form
    {
        SqlDataReader dr;
        public static string User_NAME = "";
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void Clear()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            try
            {
                bool found = false;
                MainClass.con.Open();
                SqlCommand cmd = new SqlCommand("select * from Users where Username = @Username and Password = @Password ", MainClass.con);
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                dr = cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    found = true;
                    User_NAME = dr["Name"].ToString();
                }
                else
                {
                    found = false;
                    MessageBox.Show("Invalid Details", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Clear();
                    txtUsername.Focus();
                }
                dr.Close();
                MainClass.con.Close();
                if (found == true)
                {
                    MessageBox.Show("Welcome " + User_NAME);
                    Dashboard das = new Dashboard();
                    MainClass.showWindow(das, this, MDI.ActiveForm);
                }
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void cbPass_CheckedChanged(object sender, EventArgs e)
        {
            if(cbPass.Checked == true)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }
    }
}
