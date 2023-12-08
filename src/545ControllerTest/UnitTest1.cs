using _545Controller;
using NUnit.Framework.Internal;

namespace _545ControllerTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GroupZeroStatusResponseTest()
        {
            GroupZeroStatusResponse gzsr = new GroupZeroStatusResponse(1, new byte[]
                { (byte)'0', (byte)'0', (byte)'1', (byte)'0', (byte)'0', //CurrentPV
                (byte)'0', (byte)'0', (byte)'1', (byte)'0', (byte)'0', //Target Setpoint
            (byte)'0', (byte)'0', (byte)'1', (byte)'0', (byte)'0', //Actual Setpoint
                    1, 1, 1, 1, //Control Output
                    0x19, //Status
                    0x1F, //Current Pid set and Setpoint number
                    0x00, //DIN State
                    0x04 }); //Param Changes 

            Assert.AreEqual(gzsr.PV, "00100");
            Assert.AreEqual(gzsr.TargetSetpoint, "00100");
            Assert.AreEqual(gzsr.ActualSetpoint, "00100");
            Assert.AreEqual(gzsr.controlOutput, new byte[] {1,1,1,1});

            Assert.AreEqual(gzsr.Pretune, false);
            Assert.AreEqual(gzsr.Auto, true);
            Assert.AreEqual(gzsr.LocalSetpoint, true);
            Assert.AreEqual(gzsr.Alarm2, false);
            Assert.AreEqual(gzsr.Alarm1, false);
            Assert.AreEqual(gzsr.Fault, true);

            Assert.AreEqual(gzsr.PIDSet, 3);
            Assert.AreEqual(gzsr.SetPointNumber, 7);

            Assert.AreEqual(gzsr.DIN1, false);
            Assert.AreEqual(gzsr.DIN2, false);
            Assert.AreEqual(gzsr.DIN3, false);
            Assert.AreEqual(gzsr.DIN4, false);
            Assert.AreEqual(gzsr.DIN5, false);

            Assert.AreEqual(gzsr.DisplayParamChanged, true);
            Assert.AreEqual(gzsr.OperationParamChanged, false);
            Assert.AreEqual(gzsr.SetupParamChanged, false);
        }
    }
}