using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace RoomBookingSystem
{
    public partial class AddBookScreen : Form
    {
        public AddBookScreen()
        {
            InitializeComponent();
        }


        private void checkBoxClassA_Clicked(object sender, EventArgs e)
        {
            checkBoxClassB.CheckState = CheckState.Unchecked;
            checkBoxClassC.CheckState = CheckState.Unchecked;
        }

        private void checkBoxClassB_Clicked(object sender, EventArgs e)
        {
            checkBoxClassA.CheckState = CheckState.Unchecked;
            checkBoxClassC.CheckState = CheckState.Unchecked;
        }

        private void checkBoxClassC_Clicked(object sender, EventArgs e)
        {
            checkBoxClassA.CheckState = CheckState.Unchecked;
            checkBoxClassB.CheckState = CheckState.Unchecked;
        }

        private async void submitButton_Click(object sender, EventArgs e)
        {
            if(textBoxName.Text!="" && textBoxIDNumber.Text!="" && roomList.Text!="" && purpose.Text!="" && noPerson.Text!="" && (checkBoxClassA.CheckState==CheckState.Checked || checkBoxClassB.CheckState == CheckState.Checked || checkBoxClassC.CheckState == CheckState.Checked))
            {
                string name = textBoxName.Text;
                int idNumber = int.Parse(textBoxIDNumber.Text);
                string room = roomList.Text;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
                string date = datePicker.Value.ToShortDateString();
                string time = "";
                string _purpose = purpose.Text;
                string numPerson = noPerson.Text;
                if (checkBoxClassA.Checked)
                {
                    time = "00";
                }
                else if (checkBoxClassB.Checked)
                {
                    time = "01";
                }
                else if (checkBoxClassC.Checked)
                {
                    time = "02";
                }

                FunctionsClass functions = new FunctionsClass();
                int cond = await functions.checkRoomTaken(date, room, time);

                if (cond == 0)
                {
                    new PopupMessage("Sorry, The expected seat is already reserved on that day!").ShowDialog();
                }
                else
                {
                    try
                    {
                        string[] resp = await functions.newBooking(name, idNumber.ToString(), room, date, time, _purpose, numPerson);
                        new PopupMessage("Reservation made successfully! Your Reservation Number is " + resp[1]).ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        new PopupMessage("Error making reservation!" + ex).ShowDialog();
                    }
                    finally
                    {
                        textBoxName.Text = "";
                        textBoxIDNumber.Text = "";
                        roomList.Text = "";
                        purpose.Text = "";
                        noPerson.Text = "";
                        checkBoxClassA.CheckState = CheckState.Unchecked;
                        checkBoxClassB.CheckState = CheckState.Unchecked;
                        checkBoxClassC.CheckState = CheckState.Unchecked;
                    }
                }
            }
            else
            {
                new PopupMessage("Enter all required information!").ShowDialog();
            }
        }

     
    }
}