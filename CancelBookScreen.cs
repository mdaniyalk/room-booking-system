using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace room_booking_system
{
    public partial class CancelBookScreen : Form
    {
        public CancelBookScreen()
        {
            InitializeComponent();
        }

        private async Task cancelButton_ClickAsync(object sender, EventArgs e)
        {
            if (checkBoxAgree.CheckState == CheckState.Unchecked)
            {
                new PopupMessage("Please agree you understand that you can not reverse this cancellation").ShowDialog();
            }
            else
            {
                if(textBoxBookId.Text != "" && textBoxIdNumber.Text != "")
                {
                    int reservationId = int.Parse(textBoxBookId.Text);
                    int IdNumberId = int.Parse(textBoxIdNumber.Text);

                    FunctionsClass functions = new FunctionsClass();

                    try
                    {
                        int resp = await functions.cancelBooking(reservationId.ToString(), IdNumberId.ToString());
                        new PopupMessage("Reservation Cancellation Success!").ShowDialog();
                    }
                    catch(Exception ex)
                    {
                        new PopupMessage("Reservation not found!" + ex).ShowDialog();
                    }
                    finally
                    {
                        textBoxBookId.Text = "";
                        textBoxIdNumber.Text = "";
                        checkBoxAgree.CheckState = CheckState.Unchecked;
                    }
                }
                else
                {
                    new PopupMessage("Please input all required details!").ShowDialog();
                }
            }
        }
    }
}
