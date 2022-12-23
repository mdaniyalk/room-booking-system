using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace room_booking_system
{
    public partial class CancelBookScreen : Form
    {
        public CancelBookScreen()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
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
                        SqlConnection connection = new SqlConnection(functions.connectionString);
                        string query = @"DELETE FROM ReservationTable WHERE (ReservationID= '" + reservationId + "' AND IdNumberID= '" + IdNumberId + "')";
                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                        new PopupMessage("Reservation Cancellation Success!").ShowDialog();
                        connection.Close();
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
