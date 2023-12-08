using System;
using System.Collections.Generic;
using System.IO.Hashing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _545Controller
{
    public class Controller
    {
        public byte[] StationAddress = new byte[2];
        
        public Controller(byte[] _stationAddress)
        {
            StationAddress = _stationAddress;
        }

        public byte[] BuildPacket(byte[] data)
        {
            List<byte> packet = new List<byte>();
            packet.Add(Constants.STX);
            packet.AddRange(StationAddress);
            packet.AddRange(data);
            packet.Add(Constants.ETX);
            var tmp = packet.ToArray();
            packet.AddRange(Crc32.Hash(tmp));
            return packet.ToArray();
        }
    }

    public class ResponseDataFrame
    {
        public byte[] Parameter = new byte[2];
        public byte LoopNumber;

        public static byte[] StringToByteArray(string s)
        {
            byte[] buffer = new byte[s.Length];
            int count = 0;
            foreach (var letter in s)
            {
                buffer[count++] = (byte)letter;
            }
            return buffer;
        }

        public ResponseDataFrame(byte[] parameter, byte loopNumber)
        {
            Parameter = parameter;
            LoopNumber = loopNumber;
        }
    }

    public class GroupZeroStatusResponse : ResponseDataFrame
    {
        public byte[] pv = new byte[5];
        public byte[] targetSetpoint = new byte[5];
        public byte[] actualSetpoint = new byte[5];
        public byte[] controlOutput = new byte[4];
        public byte status;
        public byte pidSetAndNumber;
        public byte digitalInputs;
        public byte paramChanges;

        #region Properties
        public string PV
        {
            get
            { 
                StringBuilder stringBuilder = new StringBuilder();
                for(int i = 0; i < pv.Length; i++)
                {
                    stringBuilder.Append((char)pv[i]);
                }
                return stringBuilder.ToString();
            }
        }

        public string TargetSetpoint
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < targetSetpoint.Length; i++)
                {
                    stringBuilder.Append((char)targetSetpoint[i]);
                }
                return stringBuilder.ToString();
            }
        }

        public string ActualSetpoint
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < actualSetpoint.Length; i++)
                {
                    stringBuilder.Append((char)actualSetpoint[i]);
                }
                return stringBuilder.ToString();
            }
        }

        public byte[] ControlOutput
        {
            get { return controlOutput; }
        }

        public bool Pretune
        {
            get
            {
                return (status & 0x20) == 0x20;
            }
        }
        public bool Auto
        {
            get
            {
                return (status & 0x10) == 0x10;
            }
        }
        public bool LocalSetpoint
        {
            get
            {
                return (status & 0x08) == 0x08;
            }
        }

        public bool Alarm2
        {
            get
            {
                return (status & 0x04) == 0x04;
            }
        }

        public bool Alarm1
        {
            get
            {
                return (status & 0x02) == 0x02;
            }
        }

        public bool Fault
        {
            get
            {
                return (status & 0x01) == 0x01;
            }
        }

        public int PIDSet
        {
            get
            {
                var tmp = pidSetAndNumber >> 3;
                tmp = tmp & 0x07;
                return tmp;
            }
        }

        public int SetPointNumber
        {
            get
            {
                int tmp = pidSetAndNumber;
                tmp = tmp & 0x07;
                return tmp;
            }
        }

        public bool DIN1
        {
            get
            {
                var tmp = digitalInputs >> 0;
                tmp = tmp & 0x01;
                return tmp == 1;
            }
        }
        public bool DIN2
        {
            get
            {
                var tmp = digitalInputs >> 1;
                tmp = tmp & 0x01;
                return tmp == 1;
            }
        }
        public bool DIN3
        {
            get
            {
                var tmp = digitalInputs >> 2;
                tmp = tmp & 0x01;
                return tmp == 1;
            }
        }
        public bool DIN4
        {
            get
            {
                var tmp = digitalInputs >> 3;
                tmp = tmp & 0x01;
                return tmp == 1;
            }
        }
        public bool DIN5
        {
            get
            {
                var tmp = digitalInputs >> 4;
                tmp = tmp & 0x01;
                return tmp == 1;
            }
        }

        public bool SetupParamChanged
        {
            get
            {
                var tmp = paramChanges >> 0;
                tmp = tmp & 0x01;
                return tmp == 1;
            }
        }
        public bool OperationParamChanged
        {
            get
            {
                var tmp = paramChanges >> 1;
                tmp = tmp & 0x01;
                return tmp == 1;
            }
        }

        public bool DisplayParamChanged
        {
            get
            {
                var tmp = paramChanges >> 2;
                tmp = tmp & 0x01;
                return tmp == 1;
            }
        }
        #endregion

        public GroupZeroStatusResponse(byte loopNumber, byte[] data) : base(StringToByteArray("00"), loopNumber)
        {
            int count = 0;
            for (; count < 5; count++)
            {
                pv[count] = data[count];
            }
            for (; count < 10; count++)
            {
                targetSetpoint[count - 5] = data[count];
            }
            for (; count < 15; count++)
            {
                actualSetpoint[count - 10] = data[count];
            }
            for (; count < 19; count++)
            {
                controlOutput[count - 15] = data[count];
            }
            status = data[19];
            pidSetAndNumber = data[20];
            digitalInputs = data[21];
            paramChanges = data[22];
        }
    }

    public class ProcessVariableResponse : ResponseDataFrame
    {
        public byte[] pv = new byte[5];
        public string PV
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < pv.Length; i++)
                {
                    stringBuilder.Append((char)pv[i]);
                }
                return stringBuilder.ToString();
            }
        }

        public ProcessVariableResponse(byte loopNumber, byte[] data) : base(StringToByteArray("01"), loopNumber)
        {
            pv = data;
        }
    }

    public class SetpointResponse : ResponseDataFrame
    {
        public byte setPoint;
        public byte[] setPointValue = new byte[5];
        public string SetPointValue
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < setPointValue.Length; i++)
                {
                    stringBuilder.Append((char)setPointValue[i]);
                }
                return stringBuilder.ToString();
            }
        }

        public byte SetPoint
        {
            get
            {
                return setPoint;
            }
        }

        public SetpointResponse(byte loopNumber, byte[] data) : base(StringToByteArray("02"), loopNumber)
        {
            setPoint = data[0];
            setPointValue = data[1..];
        }
    }

    public class CommandDataFrame
    {
        public byte ReadWrite;
        public byte[] Parameter = new byte[2];
        public byte LoopNumber;
        public Controller Controller;

        public CommandDataFrame(byte[] parameter, byte loopNumber, Controller controller)
        {
            Parameter = parameter;
            LoopNumber = loopNumber;
            Controller = controller;
        }

        public string ParamCode
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < Parameter.Length; i++)
                {
                    stringBuilder.Append((char)Parameter[i]);
                }
                return stringBuilder.ToString();
            }
        }

        public static byte[] StringToByteArray(string s)
        {
            byte[] buffer = new byte[s.Length];
            int count = 0;
            foreach(var letter in s)
            {
                buffer[count++] = (byte)letter;
            }
            return buffer;
        }

        public byte[] Read()
        {
            return new byte[4] {Constants.DC1, Parameter[0], Parameter[1], LoopNumber };
        }

        public byte[] Write()
        {
            return new byte[4] { Constants.DC2, Parameter[0], Parameter[1], LoopNumber };
        }
    }

    public class ProcessVariableCommand : CommandDataFrame
    {
        public ProcessVariableCommand(byte loopNumber, Controller controller) : base(StringToByteArray("01"), loopNumber, controller)
        {

        }

        new public byte[] Read()
        {
            return Controller.BuildPacket(base.Read());
        }

        public byte[] Write(byte[] pv)
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(Write());
            buffer.AddRange(pv);
            return Controller.BuildPacket(buffer.ToArray());
        }

    }

    public class SetpointCommand : CommandDataFrame
    {
        public SetpointCommand(byte loopNumber, Controller controller) : base(StringToByteArray("02"), loopNumber, controller)
        {

        }

        public byte[] Read(char setPoint)
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(Read());
            buffer.Add((byte)setPoint);
            return Controller.BuildPacket(buffer.ToArray());
        }

        public byte[] Write(char setPoint, byte[] setPointValue)
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(Write());
            buffer.Add((byte)setPoint);
            buffer.AddRange(setPointValue);
            return Controller.BuildPacket(buffer.ToArray());
        }
    }

    public class GroupZeroStatusCommand : CommandDataFrame
    {
        public GroupZeroStatusCommand(byte loopNumber, Controller controller) : base(StringToByteArray("00"), loopNumber, controller)
        {
            
        }

        new public byte[] Read()
        {
            return Controller.BuildPacket(base.Read());
        }
    }
}
