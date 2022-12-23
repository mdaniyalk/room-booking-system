using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;

namespace RoomBookingSystem
{
    public partial class RescheduleBookScreen : Form
    {
        public RescheduleBookScreen()
        {
            InitializeComponent();
        }


        private async void validateButton_Click(object sender, EventArgs e)
        {
            if (textBoxBookId.Text != "")
            {
                int bookId = int.Parse(textBoxBookId.Text);
                
                FunctionsClass functions = new FunctionsClass();
                
                int resp = await functions.validateBooking(bookId.ToString());
                if (resp == 1)
                {
                    new PopupMessage("Reservation Validated Successfully!").ShowDialog();
                }
                else
                {
                    new PopupMessage("Validation Error!").ShowDialog();
                }
                
                
            }
            else
            {
                new PopupMessage("Enter Reservation ID!").ShowDialog();
            }
        }

        private async void updateButton_ClickAsync(object sender, EventArgs e)
        {
            int reservationID = int.Parse(textBoxBookId.Text);
            string roomName = newRoom.Text;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            string date = datePicker.Value.ToShortDateString();

            FunctionsClass functions = new FunctionsClass();

            if (textBoxBookId.Text != "" && newRoom.Text != "")
            {
                string[] time = {"00", "01", "02"};
                for (int i = 0; i< 3; i++)
                {
                    int cond = await functions.checkRoomTaken(date, roomName, time[i]);
                    if (cond == 1)
                    {
                        await functions.rescheduleBooking(reservationID.ToString(), date, roomName, time[i]);
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
