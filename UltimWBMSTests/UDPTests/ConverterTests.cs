using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSnet;
using FluentAssertions;

namespace UltimWBMSTests.UDPTests
{
    public class ConverterTests
    {
        [Fact]
        public void DoubleToInt16Byte_Number_ReturnsInt16Bytes()
        {
            //Arrange

            //Act
            byte[] output = Converter.DoubleToInt16Byte(1.2345);

            //Assert
            output.Should().Equal(0x7B, 0x00);
        }
        [Fact]
        public void DoubleToInt16Byte_LongDouble_ReturnsShortenedInt16Bytes()
        {
            //Arrange

            //Act
            byte[] output = Converter.DoubleToInt16Byte(4.5876572672624627626526576);

            //Assert
            output.Should().HaveCount(2);
        }
        [Fact]
        public void DoubleToInt16Byte_SmallDouble_ReturnsZero()
        {
            //Arrange

            //Act
            byte[] output = Converter.DoubleToInt16Byte(0.000000004);

            //Assert
            output.Should().Equal(0x00, 0x00);
        }
        [Fact]
        public void IsLitteEndian()
        {
            BitConverter.IsLittleEndian.Should().BeTrue();
        }
    }
}
