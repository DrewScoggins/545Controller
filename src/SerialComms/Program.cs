using System.IO.Ports;
using System.Text;

namespace _545Controller
{
    internal class Program
    {
        static SerialPort sp;
        public static void Read()
        {
            while (true)
            {
                try
                {
                    
                }
                catch (TimeoutException) { }
            }
        }

        static void Main(string[] args)
        {
            sp = new SerialPort("COM6", 19200, Parity.Even, 7, StopBits.One);
            sp.ReadBufferSize = 1024;
            sp.ReceivedBytesThreshold = 5;
            sp.DataReceived += Sp_DataReceived;
            GroupZeroStatus gzs = new GroupZeroStatus(Constants.DC1, (byte)'1');
            SendPacket s = new SendPacket(new byte[2] { (byte)'0', (byte)'1' }, gzs);
            sp.Open();
            string check = "";
            while (check != "quit")
            {
                sp.Write(s.GetWireBytes(), 0, s.GetWireBytes().Length);
                check = Console.ReadLine();
            }
            sp.Close();
        }

        private static void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var buffer = new byte[sp.BytesToRead];
            var message = sp.Read(buffer, 0, sp.BytesToRead);
            string content = Encoding.ASCII.GetString(buffer);
            Console.WriteLine(content);
        }
    }
}
