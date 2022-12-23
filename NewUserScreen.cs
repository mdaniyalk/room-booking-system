using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace room_booking_system
{
    public partial class NewUserScreen : Form
    {
        public NewUserScreen()
        {
            InitializeComponent();
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            string idNum = textBoxIdNumber.Text;
            string uname = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            if (name != "" && idNum != "" && uname != "" && password != "")
            {
                FunctionsClass functions = new FunctionsClass();

                if (functions.checkUserTakenAsync(uname) == 0)
                {
                    new PopupMessage("Sorry, The username is already taken!").ShowDialog();
                }
                else
                {
                    try
                    {
                        SqlConnection connection = new SqlConnection(functions.connectionString);
                        string query = "INSERT INTO UserTable (Name, idNumber, Username, Password) VALUES ('" + name + "','" + idNum + "','" + uname + "','" + password + "')";
                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                        new PopupMessage("Account registered successfully! Please use username and password to login..").ShowDialog();
                        connection.Close();
                        Profile profile = new Profile();
                        profile.name = name;
                        profile.idNumber = idNum;
                        profile.username = uname;
                    }
                    catch (Exception ex)
                    {
                        new PopupMessage("Error registering account!" + ex).ShowDialog();
                    }
                    finally
                    {
                        textBoxName.Text = "";
                        textBoxid.Text = "";
                        textBoxUsername.Text = "";
                        textBoxPassword.Text = "";
                    }
                }   
            }
            else
            {
                new PopupMessage("Enter all required information!").ShowDialog();
            }
        }

        private void txtBckToLog_Click(object sender, EventArgs e)
        {
            new LoginScreen().Show();
            this.Hide();
        }

       
    }
}
