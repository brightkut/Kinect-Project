using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        public static String name;
        public static String room_name;

        public static Socket socket;
        public static Boolean state = false;


        /// <summary>
        /// Update Server Status
        /// </summary>
        private delegate void Update_ServerStatus(String data);

        private Update_ServerStatus update_status;


        /// <summary>
        /// Update User
        /// </summary>
        private delegate void Update_User();

        private Update_User update_User;

        /// <summary>
        /// Update Two User
        /// </summary>
        private delegate void UpdateTwoUser();

        private UpdateTwoUser updateTwoUser;
       
       
        
        public MainWindow()
        {
            InitializeComponent();
            socket = IO.Socket("https://kinect-app3.herokuapp.com/");
            
          //  start_button.IsEnabled = false;


            //Update connection
            update_status += UpdateServerStatus;

            //Update user
            update_User += UpdateUserStatus;

            //Update two user
            updateTwoUser += UpdateTwoUserStatus;


            //event first
            socket.On(Socket.EVENT_CONNECT, () =>
            {

                socket.Emit("open","gui connect");

            });
            socket.On("alconnect", (connect) =>
             {
                 update_status.Invoke(connect.ToString());

             });


          
            socket.On("userSet", () =>
             {
                 
                 update_User.Invoke();
                 

             });
            socket.On("ready", () =>
             {

                 updateTwoUser.Invoke();

             });
 
        }



        //Update Status
        private void UpdateServerStatus(String data)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Update_ServerStatus(UpdateServerStatus), data);
            }
            else
            {
                //  server_status.Content = data.ToString();
                var bc = new BrushConverter();
                server_status.Background = (Brush)bc.ConvertFrom("#FF71EC1C");
              
            }
            
        }

        //Update UserStatus
        private void UpdateUserStatus() {

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Update_User(UpdateUserStatus));
            }
            else
            {
                //player_status.Content = "waiting for another player";
             

            }
        }

        //Update UserStatus
        private void UpdateTwoUserStatus()
        {

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Update_User(UpdateTwoUserStatus));
            }
            else
            {

                //player_status.Content = "Ready ";
                start_button.IsEnabled = true;

            }
        }




        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            //state = true;
            name = user.Text;
            if (name!="") { 
            Room room = new Room();
            room.Show();
            this.Close();
             }
            // Display display = new Display();
            //  display.Show();
            // this.Close();
            // socket.Emit("start", name);
        }

        private void connect_button_Click(object sender, RoutedEventArgs e)
        {
           // name = user.Text;
           // room_name = room.Text;
          //  socket.Emit("setUsrRoom", room.Text,user.Text);

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
       
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*
            if (state == false)
            {
                socket.Emit("closed",room_name, name);

                socket.On("alclose", () =>
                 {
                     socket.Disconnect();
                 });
            }
            */
        }
      
    }
}
