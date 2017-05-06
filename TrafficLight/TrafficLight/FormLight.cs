using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using _200BytePacket;


//************************************************************************//
// This project makes an extremely simple traffic light.  Because of the  //
// personal firewall on the lab computers being switched on, this         //
// actually connects to a sort of proxy (running in my office) that       //
// accepts the incomming  connection.                                     //    
// By Nigel.                                                              //
//                                                                        //
// Please use this code, such as it is,  for any eduactional or non       //
// profit making research porposes on the conditions that.                //
//                                                                        //
// 1.    You may only use it for educational and related research         //
//      pusposes.                                                         //
//                                                                        //
// 2.   You leave my name on it.                                          //
//                                                                        //
// 3.   You correct at least 10% of the typing and spelling mistakes.      //
//                                                                        //
// © Nigel Barlow nigel@soc.plymouth.ac.uk 2016                           //
//************************************************************************//

namespace TrafficLight
{
    public partial class FormTrafficLight : Form
    {
        public FormTrafficLight()
        {
            InitializeComponent();
            Thread autoMatic = new Thread(automatedOperation);
            autoMatic.Start();
        }


        //******************************************************//
        // Nigel Networking attributes.                         //
        //******************************************************//
        private int              serverPort       = 5000;
        private int              bufferSize       = 200;
        private TcpClient        socketClient     = null;
        private String           serverName       = "eeyore.fost.plymouth.ac.uk";  //A computer in my office.
        //private String serverName = "127.0.0.1";
        private NetworkStream    connectionStream = null;
        private BinaryReader     inStream         = null;
        private BinaryWriter     outStream        = null;
        private ThreadConnection threadConnection = null;
        private bool manualOverride = false;
        Semaphore changing = new Semaphore(1, 1);
        private int myID = 2;
        private string myIP = "141.163.100.5";

        

        //*******************************************************************//
        // This one is needed so that we can post messages back to the form's//
        // thread and don't violate C#'s threading rule that says you can    //
        // only touch the UI components from the form's thread.              //
        //*******************************************************************//
        SynchronizationContext uiContext = null;

        public void automatedOperation()
        {
            while (true)
            {


                while (manualOverride == false)
                {
                    change();//need to send a message to self;
                    Thread.Sleep(10000);
                }
            }
        }

        public void manualChange()
        {
            manualOverride = !manualOverride;
        }

        //pretty much finshed.
        //should be only point at which lights get touched
        public void change()
        {
            changing.WaitOne();
            //if true the lights are off
            if (labelRed.Visible == false && labelAmber.Visible == false && labelGreen.Visible == false)
            {
                labelRed.Invoke((MethodInvoker)delegate { labelRed.Visible = true; });
                //labelRed.Visible = true;
            }
            //if the lights are red change means amber
            else if (labelRed.Visible == true)
            {
                //red and amber phase
                labelAmber.Invoke((MethodInvoker)delegate { labelAmber.Visible = true; });
                //labelAmber.Visible = true;
                //wait 2 seconds
                Thread.Sleep(2000);
                labelAmber.Invoke((MethodInvoker)delegate { labelAmber.Visible = false; });
                //labelAmber.Visible = false;
                labelRed.Invoke((MethodInvoker)delegate { labelRed.Visible = false; });
                //labelRed.Visible = false;
                labelGreen.Invoke((MethodInvoker)delegate { labelGreen.Visible = true; });
                //labelGreen.Visible = true;
            }
            else if (labelGreen.Visible == true)
            {
                labelAmber.Invoke((MethodInvoker)delegate { labelAmber.Visible = true; });
                //labelAmber.Visible = true;
                labelGreen.Invoke((MethodInvoker)delegate { labelGreen.Visible = false; });
                //labelGreen.Visible = false;
                //wait 2 seconds
                Thread.Sleep(2000);
                labelAmber.Invoke((MethodInvoker)delegate { labelAmber.Visible = false; });
                //labelAmber.Visible = false;
                labelRed.Invoke((MethodInvoker)delegate { labelRed.Visible = true; });
                //labelRed.Visible = true;
            }

            changing.Release();
        }


        //*********************************************************************//
        // Form load.  Display an IP. Or a series of IPs.                      //                               
        //*********************************************************************//
        private void Form1_Load(object sender, EventArgs e)
        {
            //******************************************************************//
            //All this to find out IP number.                                   //
            //******************************************************************//
            IPHostEntry localHostInfo = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());



            foreach (IPAddress address in localHostInfo.AddressList)
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    listBoxOutput.Items.Add("Your local IP is " + address.ToString());
                }



            //******************************************************************//
            // Get the SynchronizationContext for the current thread (the form's//
            // thread).                                                         //
            //******************************************************************//
            uiContext = SynchronizationContext.Current;
            if (uiContext == null)
                listBoxOutput.Items.Add("No context for this thread");
            else
                listBoxOutput.Items.Add("We got a context");
        }



        //*********************************************************************//
        // Form closing.  If the connection thread was ever created then kill  //
        // it off.                                                             //                               
        //*********************************************************************//
        private void FormTrafficLight_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (threadConnection != null) threadConnection.StopThread();
        }


        //POSTS MESSAGE FROM LISTEN THREAD TO MAIN FORM THREAD
        public void MessageReceived(Object message)
        {
            Packet command = (Packet)message;
            messageManager(command);
        }
        //DETERMINES WHAT TO DO WITH MESSAGES BASED ON MESSAGE CODE
        private void messageManager(Packet message)
        {
            switch(message.messageCode)
            {
                //query
                case 0:
                    queryResponse(message);
                    break;
                    //query response makes no sense on this end
                case 1:
                    break;
                    //Override
                case 2:  manualOverride = !manualOverride; 
                    break;
                    //OFF
                case 3:this.Close();
                    break;
                    //Case 4 = change lights
                case 4:
                    if (manualOverride ==true)
                    {
                        //starts a new thread to change the light leaving from thread open for keepalives
                        Thread changer = new Thread(change);
                        changer.Start();
                    }
                    
                    break;
                case 5:
                    break;
            }
        }


        //*********************************************************************//
        // The OnClick for the "connect"command button.  Create a new client   //
        // socket.   Much of this code is exception processing.                //
        //*********************************************************************//
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                socketClient = new TcpClient(serverName, serverPort);
            }
            catch (Exception ee)
            {
                listBoxOutput.Items.Add("Error in connecting to server");     //Console is a sealed object; we
                listBoxOutput.Items.Add(ee.Message);				 	      //can't make it, we can just access
                labelStatus.Text = "Error " + ee.Message;
                labelStatus.BackColor = Color.Red;
            }

            if (socketClient == null)
            {
                listBoxOutput.Items.Add("Socket not connected");

            }
            else
            {

                //**************************************************//
                // Make some streams.  They have rather more        //
                // capabilities than just a socket.  With this type //
                // of socket, we can't read from it and write to it //
                // directly.                                        //
                //**************************************************//
                connectionStream = socketClient.GetStream();
                inStream  = new BinaryReader(connectionStream);
                outStream = new BinaryWriter(connectionStream);

                listBoxOutput.Items.Add("Socket connected to " + serverName);

                labelStatus.BackColor = Color.Green;
                labelStatus.Text = "Connected to " + serverName;


                //**********************************************************//
                // Discale connect button (we can only connect once) and    //
                // enable other components.                                 //
                //**********************************************************//
                buttonConnect.Enabled    = false;
                buttonCarArrived.Enabled = true;


                //***********************************************************//
                //We have now accepted a connection.                         //
                //                                                           //
                //There are several ways to do this next bit.   Here I make a//
                //network stream and use it to create two other streams, an  //
                //input and an output stream.   Life gets easier at that     //
                //point.                                                     //
                //***********************************************************//
                threadConnection = new ThreadConnection(uiContext, socketClient, this);


                //***********************************************************//
                // Create a new Thread to manage the connection that receives//
                // data.  If you are a Java programmer, this looks like a    //
                // load of hokum cokum..                                     //
                //***********************************************************//
                Thread threadRunner = new Thread(new ThreadStart(threadConnection.Run));
                threadRunner.Start();

                Console.WriteLine("Created new connection class");
            }
        }

        //**********************************************************************//
        // Button cluck for the car arrived button.  All it does is send the    //
        // string "Car" to the server.                                          //
        //**********************************************************************//
        private void buttonCarArrived_Click(object sender, EventArgs e)
        {
            //sendString("Car", textBoxLightIP.Text);
        }

        private void queryResponse(Packet message)
        {
            //create a new packet responding to the sender
            Packet responsePacket = new Packet(message.senderIP, message.senderID, myID, 1, myIP);
            if (labelRed.Visible)
            {
                responsePacket.lightState[0] = 2;
            }
            if (labelAmber.Visible)
            {
                responsePacket.lightState[1] = 2;
            }
            if (labelGreen.Visible)
            {
                responsePacket.lightState[2] = 2;
            }
            if (manualOverride)
            {
                responsePacket.lightState[3] = 2;
            }
            sendPacket(responsePacket);
        }

        private void sendPacket(Packet toSend)
        {
            byte[] packet = toSend.toBytes();

            outStream.Write(packet, 0, bufferSize);
        }

        //**********************************************************************//
        // Send a string to the IP you give.  The string and IP are bundled up  //
        // into one of there rather quirky Nigel style packets.                 // 
        //                                                                      //
        // This uses the pre-defined stream outStream.  If this strean doesn't  //
        // exist then this method will bomb.                                    //
        //                                                                      //
        // It also does the networking synchronously, in the form's main        //
        // Thread.  This is not good practise; all networking should really be  //
        // asynchronous.                                                        //
        //**********************************************************************//
        //private void sendString(String stringToSend, String sendToIP)
        //{
        //    stringToSend = theNameOfThis + " " + stringToSend;
        //    try
        //    {
        //        byte[] packet = new byte[bufferSize];
        //        String[] ipStrings = sendToIP.Split('.'); //Split with . as separator

        //        packet[0] = Byte.Parse(ipStrings[0]);
        //        packet[1] = Byte.Parse(ipStrings[1]);   //Think about this.  It assumes the user
        //        packet[2] = Byte.Parse(ipStrings[2]);   //has entered the IP corrrectly, and 
        //        packet[3] = Byte.Parse(ipStrings[3]);   //sends the numbers without the bytes.

        //        int bufferIndex = 4;                    //Start assembling message

        //        //**************************************************************//
        //        // Turn the string into an array of characters.                 //
        //        //**************************************************************//
        //        int length = stringToSend.Length;
        //        char[] chars = stringToSend.ToCharArray();


        //        //**************************************************************//
        //        // Then turn each character into a byte and copy into my packet.//
        //        //**************************************************************//
        //        for (int i = 0; i < length; i++)
        //        {
        //            byte b = (byte)chars[i];
        //            packet[bufferIndex] = b;
        //            bufferIndex++;
        //        }

        //        packet[bufferIndex] = 0;    //End of packet (even though it is always 200 bytes)

        //        outStream.Write(packet, 0, bufferSize);
        //        listBoxOutput.Items.Add("Sent " + stringToSend);
        //    }
        //    catch (Exception doh)
        //    {
        //        listBoxOutput.Items.Add("An error occurred: " + doh.Message);
        //    }

        //}


    }   // End of classy class.
}   // End of namespace
