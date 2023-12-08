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

            //sp = new SerialPort("COM6", 19200, Parity.Even, 7, StopBits.One);
            //sp.ReadBufferSize = 1024;
            //sp.ReceivedBytesThreshold = 5;
            //sp.DataReceived += Sp_DataReceived;
            Controller c = new Controller(new byte[2] { (byte)'0', (byte)'1' });
            GroupZeroStatusCommand gzs = new GroupZeroStatusCommand((byte)'1', c);
            ProcessVariableCommand pvc = new ProcessVariableCommand((byte)'1', c);
            SetpointCommand spc = new SetpointCommand((byte)'1', c);
            var output = gzs.Read();
            output = pvc.Write(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            output = pvc.Read();
            output = spc.Write('1', new byte[] { 49, 49, 49, 49, 49 });
            output = spc.Read();
            //sp.Open();
            string check = "";
            while (check != "quit")
            {
                //sp.Write(output, 0, output.Length);
                check = Console.ReadLine();
            }
            //sp.Close();
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
