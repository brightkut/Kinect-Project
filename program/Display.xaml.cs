using Microsoft.Kinect;
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
using UnityEngine;
using Newtonsoft.Json;
using Microsoft.Speech.Recognition;
using Kinect_v2_Learning;
using System.Runtime.InteropServices;
using KinectBackgroundRemoval;

namespace Project3
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class Display : Window
    {
        /// <summary>
        /// Declare Sensor
        /// </summary>
        private BackgroundRemovalTool _backgroundRemovalTool;

        private KinectSensor sensor;

        /// <summary>
        /// Stream for 32b-16b conversion.  
        /// <summary>
        private KinectAudioStream convertStream = null;

        /// <summary>
        /// Speech recognition engine using audio data from Kinect.
        /// <summary>
        private SpeechRecognitionEngine speechEngine = null;


        /// <summary>
        /// Speech utterance confidence below which we treat speech as if it hadn't been heard
        /// </summary>
        private const double ConfidenceThreshold = 0.05;

        private Vocabulary vocabulary = new Vocabulary();



        //code draw skeleton<start>
        Body[] _bodies;

        //<end>

        /// <summary>
        /// Declare MultiSourceFrameReader
        /// </summary>
        private MultiSourceFrameReader multiSourceFrameReader;

        /// <summary>
        /// Store Body data
        /// </summary>
        private delegate void Update_NameStatus();

        private Update_NameStatus update_name_status;


        private delegate void Update_Name2Status();

        private Update_Name2Status update_name2_status;
        private delegate void Update_Name3Status(String data);

        private Update_Name3Status update_name3_status;
        private delegate void Update_Name4Status(String data);

        private Update_Name4Status update_name4_status;

        public int round = 0;



        public Display()
        {
            InitializeComponent();

            //Get Kinect Sensor
            sensor = KinectSensor.GetDefault();

            //To check the Kinect is connected .
            if (sensor != null)
            {

                sensor.Open();


                multiSourceFrameReader = sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body | FrameSourceTypes.Depth | FrameSourceTypes.BodyIndex);
                // multiSourceFrameReader = sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Body);
                multiSourceFrameReader.MultiSourceFrameArrived += MultiSourceFrameReader_MultiSourceFrameArrived;

                //Initialize the background removal tool.
                _backgroundRemovalTool = new BackgroundRemovalTool(sensor.CoordinateMapper);
            }



            //Speech
            InitalizeSpeechRecognition();

            //update name 
            update_name_status += UpdateNameStatus;
            update_name2_status += UpdateName2Status;
            update_name3_status += UpdateName3Status;
            update_name4_status += UpdateName4Status;
            Console.WriteLine(Room.room_name);
            Console.WriteLine(Room.playerIndex);
            myName.Content =  Room.room_name;
            n.Content = MainWindow.name;
           MainWindow.socket.Emit("c1",Room.room_name,Room.playerIndex,round);
           
            

            MainWindow.socket.On("first", () => {
            
                Console.WriteLine("im first");
                if (Room.playerIndex == 0)
                {

                    update_name_status.Invoke();
                }


            });

            MainWindow.socket.On("second", (a) => {
               // stat.Content = "Status: " + a.ToString();
                Console.WriteLine("im sec");
                if (Room.playerIndex == 1)
                {
                    update_name2_status.Invoke();
                }


            });
            MainWindow.socket.On("chatb", (v) => {

                update_name3_status.Invoke(v.ToString());
                Console.WriteLine("send: " + v.ToString());


            });
            MainWindow.socket.On("sb", (m) =>{
                update_name4_status.Invoke(m.ToString());


            });

            /*
             MainWindow.socket.On("cr1", (a) => {
                 // Console.WriteLine(a);
                 // Console.WriteLine(a.ToString());
                 Console.WriteLine("come in 1");
                
                 update_name_status.Invoke(a.ToString());
             });


            MainWindow.socket.On("cr2", (b) => {
                Console.WriteLine("come in this");

                if (Room.playerIndex == 1)
                {
                    update_name2_status.Invoke(b.ToString());
                }
                else
                {

                    update_name_status.Invoke(b.ToString());
                }

            });
            */
           
           

            /*
            try
            {
                MainWindow.socket.On("show", (joint) =>
                {

                    Body body = JsonConvert.DeserializeObject<Body>(joint.ToString());
                    canvas2.DrawSkeleton(body);
                    Console.WriteLine("draw 2 already");
                    */



            //object jss = joint;

            //string d = joint.ToString();
            // Console.WriteLine(d);

            //Body b = (Body) d;
            //  Body b = (Body)jss;




            // canvas2.DrawSkeleton(b)
            /*
            List<Body> collection = new List<Body>((IEnumerable<Body>)jss);


            IList<Body> bod2 = Newtonsoft.Json.JsonConvert.DeserializeObject(t);
            foreach (var b0d2 in collection)
            {
                canvas2.DrawSkeleton(b);
            }

            */




            /*
                });
            }
            catch (Exception)
            {
                Console.WriteLine("error part receive");
                MainWindow.socket.Emit("closed", MainWindow.name);
                MainWindow.socket.On("alclose", () =>
                {
                    MainWindow.socket.Disconnect();
                });
                */
        }

        private void UpdateNameStatus()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Update_NameStatus(UpdateNameStatus));
            }
            else
            {
                pstat.Content = "Wait";
                //vo.Content = "Vocabulary :        " + data;
               
               
                //  Opname.Content = "Your Opponent name : " + data.ToString();



            }

        }
        private void UpdateName2Status()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Update_Name2Status(UpdateName2Status));
            }
            else
            {
                pstat.Content = "2 Connected";

                // stat.Content = "Your Turn";
                // vo.Content = data;
            }
              




            }

        
        private void UpdateName3Status(String data)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Update_Name3Status(UpdateName3Status), data);
            }
            else
            {

                vo.Content = data;

               
             
             


                //  Opname.Content = "Your Opponent name : " + data.ToString();



            }

        }
        private void UpdateName4Status(String data)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Update_Name4Status(UpdateName4Status), data);
            }
            else
            {
                msg.Text += MainWindow.name+"  :  "+data + "\n";
                msg.ScrollToEnd();






                //  Opname.Content = "Your Opponent name : " + data.ToString();



            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

       
           
          
        }



        private void MultiSourceFrameReader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {



            //Get FreameRefernece
            var source = e.FrameReference.AcquireFrame();

            using (var color = source.ColorFrameReference.AcquireFrame())
            using (var depthFrame = source.DepthFrameReference.AcquireFrame())
            using (var bodyFrame = source.BodyFrameReference.AcquireFrame())
            using (var bodyIndexFrame = source.BodyIndexFrameReference.AcquireFrame())
            {


                if (color != null && depthFrame != null && bodyIndexFrame != null)

                {
                    //canvas.Children.Clear();


                    //Display the ColorFrame
                    // ImgColor.Source = color.ToBitmap();

                   ImgColor.Source = _backgroundRemovalTool.GreenScreen(color, depthFrame, bodyIndexFrame);
                    
                   
                    //array that need to send is _bodies
                    _bodies = new Body[bodyFrame.BodyFrameSource.BodyCount];
                    bodyFrame.GetAndRefreshBodyData(_bodies);

                    // Console.WriteLine("create body already");
                    foreach (var body in _bodies)
                    {

                        if (color != null && body != null)
                        {
                            //  Console.WriteLine("body not null");

                            // Draw skeleton.

                            if (body.IsTracked)
                            {
                                // Console.WriteLine("in scope for draw");
                                //  Console.WriteLine(body);
                                // canvas.DrawSkeleton(body);
                                // Console.WriteLine("draw already");
                                // Console.WriteLine("gesture position");
                                Detecting(body);
                                // Console.WriteLine("detect already");



                            }




                        }

                    }
                    // string bd = Newtonsoft.Json.JsonConvert.SerializeObject(_bodies[0]);
                    // MainWindow.socket.Emit("display", bd);
                }
                /*

                foreach (var Bodyindex in body)
                {
                    if (Bodyindex != null)
                    {
                        if (Bodyindex.IsTracked)
                        {
                            Detecting(Bodyindex);
                        }
                    }

                }
                */
            }


        }
        private void InitalizeSpeechRecognition()
        {

            // grab the audio stream
            IReadOnlyList<AudioBeam> audioBeamList = this.sensor.AudioSource.AudioBeams;
            System.IO.Stream audioStream = audioBeamList[0].OpenInputStream();

            // create the convert stream
            this.convertStream = new KinectAudioStream(audioStream);

            //RecognizerInfo
            RecognizerInfo recognizerInfo = TryGetKinectRecognizer();

            if (null != recognizerInfo)
            {
                //Using KinectRecognizer();
                this.speechEngine = new SpeechRecognitionEngine(recognizerInfo.Id);

                var grammarBuilder = new GrammarBuilder { Culture = recognizerInfo.Culture };

                //把語音字典放進去
                grammarBuilder.Append(vocabulary.Speech_Dictionary);

                //Grammar
                var grammar = new Grammar(grammarBuilder);

                //載入文法
                this.speechEngine.LoadGrammar(grammar);

                // let the convertStream know speech is going active
                this.convertStream.SpeechActive = true;

                // For long recognition sessions (a few hours or more), it may be beneficial to turn off adaptation of the acoustic model. 
                // This will prevent recognition accuracy from degrading over time.
                speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                this.speechEngine.SetInputToDefaultAudioDevice();
                //this.speechEngine.SetInputToAudioStream(this.convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                this.speechEngine.RecognizeAsync(RecognizeMode.Multiple);

                //語音辨識事件，語音辨識拒絕事件
                this.speechEngine.SpeechRecognized += SpeechRecognized;
                this.speechEngine.SpeechRecognitionRejected += SpeechRejected;
            }
        }

        #region RecognizerInfo
        public static RecognizerInfo TryGetKinectRecognizer()
        {
            IEnumerable<RecognizerInfo> recognizers;

            // This is required to catch the case when an expected recognizer is not installed.
            // By default - the x86 Speech Runtime is always expected. 
            try
            {
                recognizers = SpeechRecognitionEngine.InstalledRecognizers();
            }
            catch (COMException)
            {
                return null;
            }

            foreach (RecognizerInfo recognizer in recognizers)
            {
                Console.WriteLine("電腦上的辨識引擎" + SpeechRecognitionEngine.InstalledRecognizers());
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) &&
                    "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
                else
                {
                    return recognizer;
                }
            }
            return null;
        }
        #endregion

        /// <summary>
        /// 語音辨識拒絕事件
        /// </summary>
        private void SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {

        }

        /// <summary>
        /// 語音辨識事件
        /// </summary>
        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //Show Sppech Confidence number on screen
            Console.WriteLine("準確度:" + e.Result.Confidence.ToString());

            //設定語音信度門檻值
            if (e.Result.Confidence >= ConfidenceThreshold)
            {
               // Console.WriteLine(e.Result.Text);

                //語音辨識跟單字比對
               
                Console.WriteLine(e.Result.Semantics.Value.ToString().ToLower());
                vo.Content = e.Result.Semantics.Value.ToString();




            }
        }
    

    private void Detecting(Body body)
        {

            //頭部
            var Head = body.Joints[JointType.Head];//頭
            var Neck = body.Joints[JointType.Neck];//脖子
                                                   //手臂兩側
            var RightHandTip = body.Joints[JointType.HandTipRight];//右手頂部
            var LeftHandTip = body.Joints[JointType.HandTipLeft];//左手頂部
            var RightHand = body.Joints[JointType.HandRight];//右手
            var LeftHand = body.Joints[JointType.HandLeft];//左手
            var RightThumb = body.Joints[JointType.ThumbRight];//右手掌
            var LeftThumb = body.Joints[JointType.ThumbLeft];//左手掌
            var RightWrist = body.Joints[JointType.WristRight];//右手腕
            var LeftWrist = body.Joints[JointType.WristRight];//左手腕
            var RightElbow = body.Joints[JointType.ElbowRight];//右手臂
            var LeftElbow = body.Joints[JointType.HandLeft];//左手臂
            var RightShoulder = body.Joints[JointType.ShoulderRight];//右肩膀
            var LeftShoulder = body.Joints[JointType.ShoulderLeft];//左肩膀
                                                                   //中間
            var SpineShoulder = body.Joints[JointType.SpineShoulder];//脊柱肩膀
            var SpineMid = body.Joints[JointType.SpineMid];//脊柱中間
            var SpineBase = body.Joints[JointType.SpineBase];//脊柱基礎
                                                             //下半身
            var RightHip = body.Joints[JointType.HipRight];//右髖關節
            var LeftHip = body.Joints[JointType.HipLeft];//左髖關節
            var RightKnee = body.Joints[JointType.KneeRight];//右膝蓋
            var LeftKnee = body.Joints[JointType.KneeLeft];//左膝蓋
            var RightAnkle = body.Joints[JointType.AnkleRight];//右腳踝
            var LeftAnkle = body.Joints[JointType.AnkleLeft];//左腳踝
            var RightFoot = body.Joints[JointType.FootRight];//右腳
            var LeftFoot = body.Joints[JointType.FootLeft];//左腳
            /*
            Console.WriteLine("Position X = " + LeftShoulder.Position.X + "Position Y = " + LeftShoulder.Position.Y + "Position Z = " + LeftShoulder.Position.Z);

            Console.WriteLine("Position X = " + RightShoulder.Position.X + "Position Y = " + RightShoulder.Position.Y + "Position Z = " + RightShoulder.Position.Z);

            Console.WriteLine("Position X = " + LeftElbow.Position.X + "Position Y = " + LeftElbow.Position.Y + "Position Z = " + LeftElbow.Position.Z);

            Console.WriteLine("Position X = " + RightElbow.Position.X + "Position Y = " + RightElbow.Position.Y + "Position Z = " + RightElbow.Position.Z);

            Console.WriteLine("Position X = " + LeftHip.Position.X + "Position Y = " + LeftHip.Position.Y + "Position Z = " + LeftHip.Position.Z);

            Console.WriteLine("Position X = " + RightHip.Position.X + "Position Y = " + RightHip.Position.Y + "Position Z = " + RightHip.Position.Z);

            Console.WriteLine("Position X = " + Head.Position.X + "Position Y = " + Head.Position.Y + "Position Z = " + Head.Position.Z);
            */
           // if(-0.27>=LeftShoulder.Position.X && LeftShoulder.Position.X>=-0.32 && LeftShoulder.Position.Y<=-0.28 &&LeftShoulder.Position.Y>=-0.34 && RightShoulder.Position.X<=-0.031 &&RightShoulder.Position.X>=-0.039 && RightShoulder.Position.Y<=-0.18 && RightShoulder.Position.Y>=-0.21 && LeftElbow.Position.X<=-0.065 && LeftElbow.Position.X>=-0.069&& LeftElbow.Position.Y<=-0.66&& LeftElbow.Position.Y>=-0.72 && RightElbow.Position.X>=0.065 &&RightElbow.Position.X<=0.1 &&RightElbow.Position.Y<=-0.23&& RightElbow.Position.Y>=-0.35&&LeftHip.Position.X<=-0.17&&LeftHip.Position.X>=-0.175&&LeftHip.Position.Y<=-0.52&&LeftHip.Position.Y>=-0.55&& RightHip.Position.X<=-0.017&&RightHip.Position.X>=-0.22&&RightHip.Position.Y<=-0.5&&RightHip.Position.Y>=-0.52&&Head.Position.X<=-0.245&&Head.Position.X>=-0.25&&Head.Position.X<=-0.07&&Head.Position.X>=-0.11)
           // {
             //   Console.WriteLine("Home");
           // }
           if(LeftShoulder.Position.X <LeftElbow.Position.X && RightShoulder.Position.X<RightElbow.Position.X&& LeftHandTip.Position.Y > Head.Position.Y && RightHandTip.Position.Y > Head.Position.Y)
            {
                Console.WriteLine("Home");
                //  MainWindow.socket.Emit("v", round,Room.playerIndex,"Home",Room.room_name);
                //  MainWindow.socket.Emit("chat", "Home",Room.room_name);
                vo.Content = "Home";

            }
            else if ( body.HandRightState == HandState.Open && body.HandLeftState!= HandState.Open && LeftHandTip.Position.Y < Head.Position.Y && RightHandTip.Position.Y < Head.Position.Y )
            {
                Console.WriteLine("Hello Hi");
                vo.Content = "Hello";
                //  MainWindow.socket.Emit("chat", "Hello", Room.room_name);
            }
            else if (body.HandLeftState != HandState.Closed && body.HandRightState == HandState.Closed)
            {
                Console.WriteLine("Zero");
               // vo.Content = "Zero";
                MainWindow.socket.Emit("s", vo.Content, Room.room_name);
                //   MainWindow.socket.Emit("chat", "Zero", Room.room_name);
            }
            else if(body.HandRightState == HandState.Open && body.HandLeftState == HandState.Open && LeftShoulder.Position.Y <= LeftHandTip.Position.Y && RightShoulder.Position.Y <= RightHandTip.Position.Y && LeftHandTip.Position.Y < Head.Position.Y && RightHandTip.Position.Y < Head.Position.Y)
            {
                vo.Content = "Surrender";


            }
            else if (RightWrist.Position.Y > Head.Position.Y && body.HandRightState == HandState.Open)
            {
                vo.Content = "Hand up";

                 
            }else if (body.HandRightState== HandState.Lasso || body.HandLeftState == HandState.Lasso)
            {
                vo.Content = "Fight";
            }
          


            /*
            if (Head.Position.Y < RightHand.Position.Y || Head.Position.Y < LeftHand.Position.Y)
            {
                txtPosture.Text = "Raise your hand";
            }
            else if (body.HandLeftState == HandState.Open)
            {
                txtPosture.Text = "Open Left Hand";
            }
            */

            /*
            if (body.HandLeftState == HandState.Lasso || body.HandRightState == HandState.Lasso)
            {
                txtPosture.Text = "Lasso";
            }
            else if ((body.HandLeftState == HandState.Open || body.HandRightState == HandState.Open))
            {
                txtPosture.Text = "Paper";

            }
            else if (body.HandLeftState == HandState.Closed || body.HandRightState == HandState.Closed)
            {
                txtPosture.Text = "Rock";
            }

            else
            {
                txtPosture.Text = null;
            }
            */

        }
      

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                MainWindow.socket.Emit("closed", MainWindow.name);
                MainWindow.socket.On("alclose", () =>
                 {
                     MainWindow.socket.Disconnect();
                 });
                if (sender != null)
                {
                    sensor.Close();
                }

                if (multiSourceFrameReader != null)
                {
                    multiSourceFrameReader.Dispose();
                    multiSourceFrameReader = null;
                }
            }catch(Exception )
            {
                Console.WriteLine("error closing");
                MainWindow.socket.Emit("closed", MainWindow.name);
                MainWindow.socket.On("alclose", () =>
                {
                    MainWindow.socket.Disconnect();
                });
            }
        }

        private void txtPosture_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
            Tutorial t = new Tutorial();
            t.Show();
            this.Close();


        }
    }

}
