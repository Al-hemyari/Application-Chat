using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Net.IO
{
    class PacketBuilder 
    {
        MemoryStream _ms;
        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);

        }
        public void WriteMessage(string msg)
        {
            int msgLength = msg.Length;
            _ms.Write(BitConverter.GetBytes(msgLength),0,0);
            _ms.Write(Encoding.ASCII.GetBytes(msg),0,0);

        }
        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}
