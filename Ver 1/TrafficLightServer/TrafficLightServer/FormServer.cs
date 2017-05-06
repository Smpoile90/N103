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


//************************************************************************//
// This project makes an extremely simple server to connect to the other  //
// traffic light clients.  Because of the personal firewall on the lab    //
// computers being switched on, the server cannot use a listening socket  //
// accept incomming connections.  So the server to actually connects to a //
// sort of proxy (running in my office) that accepts the incomming        //
// connection.                                                            //    
// By Nigel.                                                              //
//                                                                        //
// Please use this code, sich as it is,  for any eduactional or non       //
// profit making research porposes on the conditions that.                //
//                                                                        //
// 1.    You may only use it for educational and related research         //
//      pusposes.                                                         //
//                                                                        //
// 2.   You leave my name on it.                                          //
//                                                                        //
// 3.   You correct at least 10% of the typig and spekking mistskes.      //
//                                                                        //
// © Nigel Barlow nigel@soc.plymouth.ac.uk 2016                           //
//************************************************************************//

namespace TrafficLightServer
{

    //New wrapper class.
    public delegate void UI_UpdateHandler(String message);

    public partial class FormServer : Form
    {
        public FormServer()
        {
            InitializeComponent();
            IPList = new List<string>();
            IDList = new List<int>();
            requestUpdate = new Thread(autoUpdate);
            requestUpdate.Start();
        }


        //******************************************************//
        // Nigel Networking attributes.                         //
        //******************************************************//
        private int              serverPort       = 5000;
        private int              bufferSize       = 200;
        private TcpClient        socketClient     = null;
        private String           serverName       = "eeyore.fost.plymouth.ac.uk";  //A computer in my office.
        private NetworkStream    connectionStream = null;
        private BinaryReader     inStream         = null;
        private BinaryWriter     outStream        = null;
        private ThreadConnection threadConnection = null;
        private int localID = 0;
        public static List<string> IPList;
        public static List<int> IDList;
        string theNameOfThis = "plymServer";
        Thread requestUpdate;


        //*******************************************************************//
        // This one is needed so that we can post messages back to the form's//
        // thread and don't violate C#'s threading rile that says you can    //
        // only touch the UI components from the form's thread.              //
        //*******************************************************************//
        private SynchronizationContext uiContext = null;

        private void autoUpdate()
        {
            bool x = true;
            while (true)
            {
                //check the object from the autoUpdate thread   
                lsbActiveConnections.Invoke((MethodInvoker)delegate {x  = lsbActiveConnections.SelectedIndex.Equals(-1); });
                    while (x == false)
                    {
                    sendQuery();
                    Thread.Sleep(500);
                    }
 
                
            }
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
                    listBoxOutput.Items.Add("Your local IP is " +address.ToString());
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
                listBoxOutput.Items.Add(ee.Message);				 	       //can't make it, we can just access
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
                inStream         = new BinaryReader(connectionStream);
                outStream        = new BinaryWriter(connectionStream);

                listBoxOutput.Items.Add("Socket connected to " + serverName);

                labelStatus.BackColor = Color.Green;
                labelStatus.Text = "Connected to " + serverName;


                //**********************************************************//
                // Discale connect button (we can only connect once) and    //
                // enable other components.                                 //
                //**********************************************************//
                buttonConnect.Enabled       = false;
                comboBoxLightColour.Enabled = true;

                //INSTANTIATING THE CONNECTION OBJECT
                threadConnection = new ThreadConnection(uiContext, socketClient, this);
  

                //WHERE THE THREAD THAT RUNS THIS CONNECTION BEGINS
                Thread threadRunner = new Thread(new ThreadStart(threadConnection.run));
                threadRunner.Start();

                Console.WriteLine("Created new connection class");

            }
        }






        //*********************************************************************//
        // The item in the combo box has been changed.  Transmit it.           // 
        //*********************************************************************//
        private void comboBoxLightColour_SelectedIndexChanged(object sender, EventArgs e)
        {
            String toSendIP = lsbActiveConnections.SelectedItem.ToString();
            String colour =  (String) comboBoxLightColour.SelectedItem;
            sendString(colour, toSendIP);
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
        private void sendString(String stringToSend, String sendToIP)
        {
            stringToSend = theNameOfThis + " " + stringToSend;
            try
            {
                byte[] packet = new byte[bufferSize];
                string[] ipStrings = sendToIP.Split('.'); //Split with . as separator

                packet[0] = Byte.Parse(ipStrings[0]);
                packet[1] = Byte.Parse(ipStrings[1]);   //Think about this.  It assumes the user
                packet[2] = Byte.Parse(ipStrings[2]);   //has entered the IP corrrectly, and 
                packet[3] = Byte.Parse(ipStrings[3]);   //sends the numbers without the bytes.

                int bufferIndex = 4;                    //Start assembling message

                //**************************************************************//
                // Turn the string into an array of characters.                 //
                //**************************************************************//
                int length   = stringToSend.Length;
                char[] chars = stringToSend.ToCharArray();


                //**************************************************************//
                // Then turn each character into a byte and copy into my packet.//
                //**************************************************************//
                for (int i = 0; i < length; i++)
                {
                    byte b = (byte)chars[i];
                    packet[bufferIndex] = b;
                    bufferIndex++;
                }

                packet[bufferIndex] = 0;    //End of packet (even though it is always 200 bytes)

                outStream.Write(packet, 0, bufferSize);
                listBoxOutput.Invoke((MethodInvoker)delegate{listBoxOutput.Items.Add("Sent " + stringToSend);}); 
            }
            catch (Exception doh)
            {
                listBoxOutput.Invoke((MethodInvoker)delegate { listBoxOutput.Items.Add("An error occurred: " + doh.Message); });
            }

        }


        //*********************************************************************//
        // Message was posted back to us.  This is to get over the C# threading//
        // rules whereby we can only touch the UI components from the thread   //
        // that created them, which is the form's main thread.                 // 
        //*********************************************************************//
        public void MessageReceived(Object message)
        {
            String command = (String)message;

            listBoxOutput.Items.Add(command);
            inputControl(command);
        }




        //*********************************************************************//
        // Form closing.  If the connection thread was ever created then kill  //
        // it off.                                                             //                               
        //*********************************************************************//
        private void FormServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (threadConnection != null) threadConnection.StopThread();
        }


        //BUTTON THAT ADDS CLIENTS TO THE ACTIVE CLIENT LISTBOX 
        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress address;
            if ( IPAddress.TryParse(textBoxIPToAdd.Text,out address))
            {
                lsbActiveConnections.Items.Add(textBoxIPToAdd.Text);
                IPList.Add(textBoxIPToAdd.Text);
                IDList.Add(localID);
                localID++;
            }
        }

        private void lsbActiveConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            //On selected index change query the state of the light
            sendQuery();
        }

        private void sendQuery()
        {
            string toSendIP = "";
            lsbActiveConnections.Invoke((MethodInvoker)delegate {toSendIP = lsbActiveConnections.SelectedItem.ToString(); });
            sendString("Query", toSendIP);
        }

        private void sendTakover()
        {
            string toSendIP = lsbActiveConnections.SelectedItem.ToString();
            sendString("Control", toSendIP);
        }

        private void inputControl(string command)
        {
            if (command == null) return;    // Nothing to do.

            if (command.Contains(theNameOfThis))
            {
                return;
            }

            if (command.Contains("Red"))
            {
                labelRed.Visible = true;
                labelAmber.Visible = false;
                labelGreen.Visible = false;
            }
            if (command.Contains("Amber"))
            {
                labelRed.Visible = false;
                labelAmber.Visible = true;
                labelGreen.Visible = false;
            }
            if (command.Contains("Green"))
            {
                labelRed.Visible = false;
                labelAmber.Visible = false;
                labelGreen.Visible = true;
            }
            if (command.Contains("Off"))
            {
                labelRed.Visible = false;
                labelAmber.Visible = false;
                labelGreen.Visible = false;
            }
            if (command.Contains("True"))
            {
                labelOverride.BackColor = Color.Lime;
            }
            else if (command.Contains("False"))
            {
                labelOverride.BackColor = Color.Red;
            }

        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            sendTakover();
        }
    }   // End of classy class.
}       // End of namespace
