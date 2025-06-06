﻿using DATABASE_PROJECT.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
namespace DATABASE_PROJECT
{
    public partial class User_Profile : Form
    {
        private database _db;
        public string cnic;
        public User_Profile(database db, string cnic)
        {
            _db = db;
            this.cnic = cnic;
            InitializeComponent();

            // Check if the database connection is open
            if (_db.con().State != ConnectionState.Open)
            {
                _db.con().Open(); // Open the connection if it's not already open
            }
;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label_displayName.Text = "";
            label_displayCNIC.Text = "";
            label_displayEmail.Text = "";
            label_displayPhoneNo.Text = "";
            label_displayPassword.Text = "";


            OracleCommand command = _db.con().CreateCommand();
            command.CommandText = "SELECT DISTINCT ut.person_name, ut.cnic, ut.email, ut.phone_num, ld.password " +
                       "FROM user_table ut " +
                       "INNER JOIN LOGIN_DETAILS ld ON ut.loginid = ld.login_id " +
                       "WHERE cnic = :CNIC";
            command.Parameters.Add(new OracleParameter("CNIC", this.cnic));

            OracleDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                label_displayName.Text = reader["person_name"].ToString();
                label_displayCNIC.Text = reader["cnic"].ToString(); 
                label_displayEmail.Text = reader["email"].ToString(); 
                label_displayPhoneNo.Text = reader["phone_num"].ToString(); 
                label_displayPassword.Text = reader["password"].ToString(); 

            }
            else
            {
                MessageBox.Show("User not found.");
                this.Hide();
                LOGIN login = new LOGIN(_db);
                login.Show();
            }

            reader.Close();
            command.Dispose();
        }

        private void User_Profile_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete your account?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                OracleCommand command = _db.con().CreateCommand();
                command.CommandText = "DELETE FROM LOGIN_DETAILS " +
                                      "WHERE login_id = (SELECT loginid FROM user_table WHERE cnic = :CNIC)";
                command.Parameters.Add(new OracleParameter("CNIC", this.cnic));

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Account Deleted");
                    LOGIN user = new LOGIN(_db);
                    this.Hide();
                    user.Show();
                }
            }

        }

        private void button_back_Click(object sender, EventArgs e)
        {
            Main_UserView user = new Main_UserView(_db, cnic);
            user.Show();

            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void name_label_Click(object sender, EventArgs e)
        {

        }

        private void label_displayName_Click(object sender, EventArgs e)
        {

        }

        private void label_displayCNIC_Click(object sender, EventArgs e)
        {

        }

        private void label_displayEmail_Click(object sender, EventArgs e)
        {

        }

        private void label_displayPhoneNo_Click(object sender, EventArgs e)
        {

        }
        private void label_displayPassword_Click(object sender, EventArgs e)
        {

        }
    }
}
