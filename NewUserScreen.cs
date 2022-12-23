using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RoomBookingSystem
{
    public partial class NewUserScreen : Form
    {
        public NewUserScreen()
        {
            InitializeComponent();
        }

        private async void registerButton_ClickAsync(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            string idNum = textBoxIdNumber.Text;
            string uname = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            if (name != "" && idNum != "" && uname != "" && password != "")
            {
                FunctionsClass functions = new FunctionsClass();
                int response = await functions.checkUserTakenAsync(uname);
                if (response == 0)
                {
                    new PopupMessage("Sorry, The username is already taken!").ShowDialog();
                }
                else
                {
                    try
                    {
                        int resp = await functions.newUser(name, idNum, uname, password);
                        new PopupMessage("Account registered successfully! Please use username and password to login..").ShowDialog();
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
                        textBoxIdNumber.Text = "";
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
