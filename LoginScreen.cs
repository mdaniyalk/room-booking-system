﻿using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoomBookingSystem
{
    public partial class LoginScreen : Form
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private async void iconButton1_ClickAsync(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            if (username == "" || password == "")
            {
                new PopupMessage("Enter Credentials!").ShowDialog();
                txtPassword.Text = "";
            }
            else if (username == "admin" && password == "password")
            {
                new MainMenuScreen().Show();
                new PopupMessage("Log In Success!").ShowDialog();
                this.Hide();
            }
            else
            {
                try
                {
                    FunctionsClass functions = new FunctionsClass();
                    int response = await functions.validateLogin(username, password);
                    if (response == 0)
                    {
                        new PopupMessage("Invalid Credentials!").ShowDialog();
                    }
                    else
                    {
                        new UserMenuScreen().Show();
                        new PopupMessage("Log In Success!").ShowDialog();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    new PopupMessage("Login Error!" + ex).ShowDialog();
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    txtUsername.Focus();
                }
            }
        }

        private void txtExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtNewUsr_Click(object sender, EventArgs e)
        {
            new MainMenuScreen().Show();
            this.Hide();
        }
    }
}
