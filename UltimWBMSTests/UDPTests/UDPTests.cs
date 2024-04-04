using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSnet;
using FakeItEasy;


namespace UltimWBMSTests.UDPTests
{
    public class UDPTests
    {
        private readonly ISocket server;
        public UDPTests()
        {
            server = A.Fake<ISocket>();
        }

        [Fact]
        public void UDPListener_Start_ReturnsFF()
        {
            //Arrange
            UDPListener listener = new UDPListener(server, );

            //Act


            //Assert
        }
    }
}
