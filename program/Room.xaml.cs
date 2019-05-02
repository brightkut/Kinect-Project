using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace Project3
{
    /// <summary>
    /// Interaction logic for Room.xaml
    /// </summary>
   
    public partial class Room : Window
    {
        private delegate void Update_RoomStatus(String data);

        private Update_RoomStatus update_room_status;
        private delegate void Update_JoinStatus(String data);

        private Update_JoinStatus update_join_status;

        public static string room_name;
        DataTable dt;
        public string r;
        public string people;
        public int c;
        public int index;
        public static int playerIndex ;



        public Room()
        {
            InitializeComponent();
            listview1.ItemsSource = CreateTable().DefaultView;

            update_room_status += UpdateRoomStatus;
            update_join_status += UpdateJoinStatus;

          //  var gridView = new GridView();

            //  gridView.Columns.Add(new GridViewColumn
            //  {
            //   Header = "Room",
            //   DisplayMemberBinding = new Binding("Room")
            // });
            //  gridView.Columns.Add(new GridViewColumn
            // {
            //     Header = "People",
            //     DisplayMemberBinding = new Binding("People")
            // });


            //add to list view
            // this.listView.Items.Add(new RoomList { Room = "a", People = 2 });
            MainWindow.socket.On("notsame", (a) => {
                update_room_status.Invoke(a.ToString());


            });
            MainWindow.socket.On("j", (v) => {
                playerIndex = Int32.Parse(v.ToString());
                update_join_status.Invoke(v.ToString());
               
                Console.WriteLine("player index" +  playerIndex);
                Console.WriteLine("change page");
                
                


            });
            MainWindow.socket.On("e", () => {
                MessageBox.Show("This room is Full");



            });





        }
        public DataTable CreateTable()
        {
            dt = new DataTable();

            dt.Columns.Add("RoomName", typeof(string));
           
           // dt.Rows.Add("A", 1234);
           // dt.Rows.Add("B", 120);
           // dt.Rows.Add("C", 789);
            return dt;

        }
        private void UpdateRoomStatus(String data)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Update_RoomStatus(UpdateRoomStatus), data);
            }
            else
            {
                //add
                //  this.listView.Items.Add(new RoomList {Room = room.Text });
                dt.Rows.Add(data);
               
      
            }

        }
        private void UpdateJoinStatus(String data)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Update_JoinStatus(UpdateJoinStatus), data);
            }
            else
            {
                room_name = room.Text;
                Display display = new Display();
                display.Show();
                this.Close();


                // dt.Rows[index].SetField(1, Int32.Parse(data));


            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string a = listView.SelectedItem;
            //  Console.WriteLine(a);
           //  r = dt.Rows[listview1.SelectedIndex][0].ToString();
            // people = dt.Rows[listview1.SelectedIndex][1].ToString();
           // index = listview1.SelectedIndex;
            

         //  c = Int32.Parse(people);
           // if (c < 2)
           // {


                //  dt.Rows[listview1.SelectedIndex].SetField(1, c + 1);
              //  room_name = r;
                MainWindow.socket.Emit("join",room.Text);
               // Display display = new Display();
              //  display.Show();
              //  this.Close();
               // Console.WriteLine(r);
               // Console.WriteLine(c);
          //  }
         

           
        
          
  
      
           
            
      
           

        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (room.Text != "")
            { 
           
            MainWindow.socket.Emit("creRoom", room.Text);

                room.Clear();

             }
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
            

        }
    }
}
