using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _200BytePacket
{
    public class Packet
    {
        public string senderIP;
        public short messageCode;
        public int senderID;
        public int recipientID;
        public int buffersize = 200;
        public string recipientIP;
        public byte[] lightState = { 0,0,0,0};

        //CONSTRUCT PACKET OBJECT FROM BYTE[]
        public Packet(byte[] packet)
        {

            //UNPACK SENT BYTES
            string[] x1 = new string[4];
            
            for (int i = 0; i < 4; i++)
            {
                x1[i] = packet[i].ToString();
            }
            recipientIP = string.Join(".", x1);
            int bufferIndex = 4;

            //EXTRACT THE RECIPIENT ID
            byte[] temp1 = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                temp1[i] = packet[bufferIndex];
                bufferIndex++;
            }
            recipientID = BitConverter.ToInt32(temp1,0);

            //EXTRACT THE SENDER ID
            byte[] temp2 = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                temp2[i] = packet[bufferIndex];
                bufferIndex++;
            }
            senderID = BitConverter.ToInt32(temp2, 0);

            //EXTRACT THE SENDER ID
            byte[] temp3 = new byte[2];
            for (int i = 0; i < 2; i++)
            {
                temp3[i] = packet[bufferIndex];
                bufferIndex++;
            }
            messageCode = BitConverter.ToInt16(temp3, 0);

            //EXTRACT SENDER IP

            string[] x2 = new string[4];

            for (int i = 0; i < 4; i++)
            {
                x2[i] = packet[bufferIndex].ToString();
                bufferIndex++;
            }
            senderIP = string.Join(".", x1);

            //EXTRACT LIGHTSTATE
            for (int i = 0; i < 4; i++)
            {
                lightState[i] = packet[bufferIndex];
                bufferIndex++;
            }

        }

        public Packet(string recipIP,int recipID,int sendID,short messCode,string sendIP)
        {
            recipientID = recipID;
            recipientIP = recipIP;
            senderID = sendID;
            senderIP = sendIP;
            messageCode = messCode;
        }
        //CONVERTS THE PACKET OBJECT INTO A STRING OF BYTES
        public byte[] toBytes()
        {
            byte[] packet = new byte[buffersize];
            //64 bytes or 4 blocks

            string[] ipStrings = recipientIP.Split('.'); //Split with . as separator
            //FIRST FOUR BYTES = RECIPIENT IP. MUST HAVE FOR THE PROXY;
            packet[0] = Byte.Parse(ipStrings[0]);
            packet[1] = Byte.Parse(ipStrings[1]);   //Think about this.  It assumes the user
            packet[2] = Byte.Parse(ipStrings[2]);   //has entered the IP corrrectly, and 
            packet[3] = Byte.Parse(ipStrings[3]);   //sends the numbers without the bytes.

            int bufferIndex = 4;
            //NEXT 4 BYTES = RECIPIENT ID

            byte[] recipIDBytes = BitConverter.GetBytes(recipientID);
            //LOOP FOR RECIP ID
            for (int i = 0; i < 4; i++)
            {
                packet[bufferIndex] = recipIDBytes[i];
                bufferIndex++;
            }
            //NEXT 4 = SENDER ID
            byte[] SenderIDBytes = BitConverter.GetBytes(senderID);
            //LOOP FOR SENDER ID
            for (int i = 0; i < 4; i++)
            {
                packet[bufferIndex] = SenderIDBytes[i];
                bufferIndex++;
            }
            //NEXT 2 = MESSAGE CODE
            byte[] MessageCodeBytes = BitConverter.GetBytes(messageCode);
            //LOOP FOR MESSAGE CODE 
            for (int i = 0; i < 2; i++)
            {
                packet[bufferIndex]= MessageCodeBytes[i];
                bufferIndex++;
            }
            //add sender IP

            string[] SenderIPStrings = recipientIP.Split('.');
            for (int i = 0; i < 4; i++)
            {
                packet[bufferIndex] = Byte.Parse(SenderIPStrings[i]);
                bufferIndex++;
            }
            //THE LIGHTSTATE ARRAY
            for (int i = 0; i < 4; i++)
            {
                packet[bufferIndex] = lightState[i];
                bufferIndex++;
            }

            //Encrypt data portion

            return packet;    //End of packet (even though it is always 200 bytes)
        }

    }
}
