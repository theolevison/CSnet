using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSnet
{
    public interface ISocket
    {
        void Send(byte[] data);
        int ReceiveFrom(byte[] data);
    }
}
