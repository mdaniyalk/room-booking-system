using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RoomBookingSystem
{
    public partial class ViewBookScreen : Form
    {
        public ViewBookScreen()
        {
            InitializeComponent();
        }


        private async void searchButton_ClickAsync(object sender, EventArgs e)
        {
            if (textBoxIdRoom.Text != "")
            {
                int bookId = int.Parse(textBoxIdRoom.Text);

                FunctionsClass functions = new FunctionsClass();
                await functions.viewBooking(bookId.ToString());
                
            }
            else
            {
                new PopupMessage("Enter IdRoom number!").ShowDialog();
            }
        }

        
    }
}
