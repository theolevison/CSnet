using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSnet;

namespace UltimWBMSTests.UDPTests
{
    public class ModuleDataTests
    {
        public ModuleDataTests() { }

        [Fact]
        public void GetCGV_AccurateData_Returns4DecimalPlaces()
        {
            //Arrange
            ModuleData md = new ModuleData();
            const int BMSPacket0Id = 0;
            const int Time = 0;
            byte[] BMSPacket0 = Convert.FromHexString("00212D8EC4AFED8CF28CEB8CEA8DF08CF58CF18CE883F18CF08C0000E9D0000000006C2BEAD100000000000011DFA3227022322215901F2293228022151B8322EA2200001749E203B20499041492");
            md.UpdateData(BMSPacket0, BMSPacket0Id, Time);
            const int DoubleConvertedToUIntPreserving4DecimalPlaces = 36077; //8CED

            //Act
            byte[] CGVs = md.GetCGV();
            UInt16 CGV1 = BitConverter.ToUInt16(CGVs, 0);

            //Assert
            Assert.Equal(DoubleConvertedToUIntPreserving4DecimalPlaces, CGV1);
        }
    }
}
