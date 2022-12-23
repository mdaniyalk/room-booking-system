using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Threading.Tasks;

namespace room_booking_system
{
    public partial class RescheduleBookScreen : Form
    {
        public RescheduleBookScreen()
        {
            InitializeComponent();
        }


        private void validateButton_Click(object sender, EventArgs e)
        {
            if (textBoxBookId.Text != "")
            {
                int reservationID = int.Parse(textBoxBookId.Text);
                int reservationNum = 0;
                FunctionsClass functions = new FunctionsClass();

                try
                {
                    SqlConnection connection = new SqlConnection(functions.connectionString);
                    connection.Open();
                    string query1 = @"SELECT ReservationID FROM ReservationTable WHERE ReservationID LIKE '" + reservationID + "'";
                    try
                    {
                        using (SqlCommand command1 = new SqlCommand(query1, connection))
                        {
                            using (SqlDataReader reader = command1.ExecuteReader())
                            {
                                reader.Read();
                                reservationNum = reader.GetInt32(0);
                                reader.Close();
                            }
                        }
                        if (reservationNum != 0)
                        {
                            new PopupMessage("Reservation Validated Successfully!").ShowDialog();
                        }
                        connection.Close();
                    }
                    catch
                    {
                        new PopupMessage("Reservation is invalid!").ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    new PopupMessage("Validation Error!" + ex).ShowDialog();
                }
            }
            else
            {
                new PopupMessage("Enter Reservation ID!").ShowDialog();
            }
        }

        private async Task updateButton_ClickAsync(object sender, EventArgs e)
        {
            int reservationID = int.Parse(textBoxBookId.Text);
            string roomName = newRoom.Text;
            string date = datePicker.Value.ToShortDateString();

            FunctionsClass functions = new FunctionsClass();

            if (textBoxBookId.Text != "" && newRoom.Text != "")
            {
                string[] time = {"00", "01", "02"};
                for (int i = 0; i< 3; i++)
                {
                    int cond = await functions.checkRoomTaken(date, roomName, time[i];
                    if (cond == 1)
                    {
                        functions.rescheduleRoom(reservationID, date, roomName, time[i]);
                        new PopupMessage("Reschedule success!").ShowDialog();
                    }
                    else if (cond == 0 && i == 2) 
                    {
                        new PopupMessage("Sorry, The expected room is already reserved on that day!").ShowDialog();
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
