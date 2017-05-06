using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Net103ClassLib
{
    [Serializable]
    public class Packet
    {
        public string message;
        public int packetInt;
        public bool packetBool;
        public string SenderID;
        public PacketType packetType;

        public Packet(PacketType type, string senderID,string message)
        {
            this.message = message;
            this.SenderID = senderID;
            this.packetType = type;
        }

        public Packet(byte[] packetbytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(packetbytes);
            Packet p = (Packet)bf.Deserialize(ms);

            ms.Close();
            this.packetInt = p.packetInt;
            this.packetBool = p.packetBool;
            this.SenderID = p.SenderID;
            this.packetType = p.packetType;

        }

        public byte[] ToBytes()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, this);
            byte[] bytes = ms.ToArray();
            ms.Close();
            return bytes;
        }

        public enum PacketType
        {
            Query,
            Command
        }

    }
}
