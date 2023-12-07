using System;
using System.Collections.Generic;
using System.IO.Hashing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _545Controller
{
    public class SendPacket
    {
        public byte[] StationAddress = new byte[2];
        public DataFrame df;
        
        public SendPacket(byte[] _stationAddress, DataFrame _df)
        {
            StationAddress = _stationAddress;
            df = _df;
        }

        public byte[] GetWireBytes()
        {
            List<byte> packet = new List<byte>();
            packet.Add(Constants.STX);
            packet.AddRange(StationAddress);
            packet.AddRange(df.GetWireBytes());
            packet.Add(Constants.ETX);
            var tmp = packet.ToArray();
            packet.AddRange(Crc32.Hash(tmp));
            return packet.ToArray();
        }
    }

    public class DataFrame
    {
        public byte ReadWrite;
        public byte[] Parameter = new byte[2];
        public byte LoopNumber;

        public DataFrame(byte readWrite, byte[] parameter, byte loopNumber)
        {
            ReadWrite = readWrite;
            Parameter = parameter;
            LoopNumber = loopNumber;
        }

        public byte[] GetWireBytes()
        {
            return new byte[4] {ReadWrite, Parameter[0], Parameter[1], LoopNumber };
        }
    }

    public class GroupZeroStatus : DataFrame
    {
        public GroupZeroStatus(byte readWrite, byte loopNumber) : base(readWrite, new byte[2] {(byte)'0', (byte)'0' }, loopNumber)
        {
            
        }

        public byte[] GetWireBytes()
        {
            return base.GetWireBytes();
        }
    }
}
