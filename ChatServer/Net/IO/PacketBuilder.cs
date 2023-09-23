using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Net.IO
{
    class PacketBuilder
    {
        MemoryStream ms;
        public PacketBuilder()
        {
            ms = new MemoryStream();

        }
        public void WriteOpCode(byte opcode)
        {
            ms.WriteByte(opcode);
        }
        public void WriteMessage(string msg)
        {
            var msgLength = msg.Length;
            ms.Write(BitConverter.GetBytes(msgLength));
            ms.Write(Encoding.Unicode.GetBytes(msg));
        }
        public byte[] GetPacketByte()
        {
            return ms.ToArray();
        }
    }
}
