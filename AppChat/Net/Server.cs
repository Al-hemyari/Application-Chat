using AppChat.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AppChat.Net
{
     
    class Server
    {
        TcpClient _client;
        public PacketReader PacketReader;

        public event Action msgReceivedEvent;
        public event Action userDisconnectEvent;
        public event Action connectedEvent;
        
        public Server()
        {
            _client = new TcpClient();
        }
        public void ConnectToServer(string username)
        {
            if (!_client.Connected)
            {
                _client.Connect("192.168.43.92", 81);
                PacketReader = new PacketReader(_client.GetStream());
                var connectPacket = new PacketBuilder();
                connectPacket.WriteOpCode(0);
                connectPacket.WriteMessage(username);
                _client.Client.Send(connectPacket.GetPacketBytes());    
            }
            ReadPackets();
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode=PacketReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;

                        case 5:
                            userDisconnectEvent?.Invoke();
                            break;
                        case 10:
                            userDisconnectEvent?.Invoke();
                            break;
                           
                        default:
                            Console.WriteLine("ha ..yes");
                            break;
                    }
                }
            });
        }
        public void SendMessageToServer(string message)
        {
            var messagePacket=new PacketBuilder();
            messagePacket.WriteMessage(message);
            messagePacket.WriteOpCode(5);
            _client.Client.Send(messagePacket.GetPacketBytes());

        }
    }
}
