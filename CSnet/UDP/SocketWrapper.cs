using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CSnet
{
    public class SocketWrapper : ISocket
    {
        private Socket server;
        private EndPoint remoteEnd;

        public SocketWrapper(string IPAdd, int Port)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(new IPEndPoint(IPAddress.Parse(IPAdd), Port));
            remoteEnd = new IPEndPoint(IPAddress.Any, 0);
        }

        public int ReceiveFrom(byte[] data)
        {
            return server.ReceiveFrom(data, ref remoteEnd); //will this work without ref-ing the data??
        }

        public void Send(byte[] data)
        {
            server.SendTo(data, remoteEnd);
        }
    }
}
