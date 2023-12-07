using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _545Controller
{
    internal class Constants
    {
        public static readonly byte SOH = 0x01;
        public static readonly byte STX = 0x02;
        public static readonly byte ETX = 0x03;
        public static readonly byte EOT = 0x04;
        public static readonly byte ACK = 0x06;
        public static readonly byte DC1 = 0x11;
        public static readonly byte DC2 = 0x12;
    }
}
