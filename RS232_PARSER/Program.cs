using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace RS232_PARSER
{
    class Program
    {
        [DllImport("FlexBisonWrapper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern unsafe int CallParserFromDLL(int argc, IntPtr[] argv);

        static SerialPort _serialPort;
        static bool serial_enabled = false ; //setting the application to the serial port data transfer / simulated data

        static string app_version = "v1.0";
        public static unsafe void Main()
        {
            if(serial_enabled)
            {
                // serial port initalization
                if (InitSerialPort())
                {
                    _serialPort.Open();

                    Console.WriteLine("---------------------------");
                    Console.WriteLine("  RS232 Parser started " + app_version);
                    Console.WriteLine("  Press Enter to Exit   ");
                    Console.WriteLine("---------------------------");

                    string line = Console.ReadLine();
                    if (line == "\n")
                    {
                        CloseSerialPort();
                        Environment.Exit(0);
                    }
                    else
                    {
                        DLLcallToParser(line);
                    }
                }
                else
                {
                    Console.WriteLine("Error occured, application will close...");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            // simulate data transfer with entering data to the console
            else
            {
                Console.WriteLine("---------------------------");
                Console.WriteLine("  RS232 Parser started " + app_version);
                Console.WriteLine("  Simulate data transfer");
                Console.WriteLine("  Enter data to console");
                Console.WriteLine("---------------------------");


                bool quit = false;
                while (!quit)
                { 
                     Console.WriteLine("\n#####################################");
                    Console.WriteLine("Enter data here:");
                    string line = Console.ReadLine();
                    if (line == "")
                    {
                        quit = true;
                    }
                    else
                    {
                        DLLcallToParser(line);
                    }
                }
                if (quit)
                {
                    Environment.Exit(0);
                }
            }
        }


        #region Serial Port
        /// <summary>
        /// Initializing the serial port with the given settings
        /// </summary>
        /// <returns></returns>
        private static Boolean InitSerialPort()
        {
            try
            {
                // Create a new SerialPort object with default settings
                _serialPort = new SerialPort();

                // configure the basic properties
                _serialPort.PortName = "COM4";
                _serialPort.BaudRate = 9600;
                _serialPort.Parity = Parity.None;
                _serialPort.DataBits = 8;
                _serialPort.StopBits = StopBits.One;
                _serialPort.Handshake = Handshake.None;

                // Set the read/write timeouts
                _serialPort.ReadTimeout = 500;
                _serialPort.WriteTimeout = 500;

                // end of line char: CR - \x0D || LF - \x0A || CRLF - \x0D\x0A
                _serialPort.NewLine = "\x0D\x0A";

                // init data handler
                _serialPort.DataReceived += PortOnDataReceived;

                //successful init
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Data handler method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void PortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = sender as SerialPort;
            string line = serialPort.ReadLine();
            Console.WriteLine("PortData incoming...: " + line);
            DLLcallToParser(line);
        }

        /// <summary>
        /// Closing the Serial Port
        /// </summary>
        private static void CloseSerialPort()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
            _serialPort.DataReceived -= PortOnDataReceived;
            _serialPort.Dispose();
            _serialPort = null;
            Console.WriteLine("Serial port closed.");
        }
        #endregion

        #region DLL call
        /// <summary>
        /// Converting string to a byte array
        /// </summary>
        /// <remarks>https://social.msdn.microsoft.com/Forums/vstudio/en-US/76b41560-ef79-405e-b3fb-8ed30cc7d075/how-can-i-call-c-function-int-mainint-argc-char-argv-from-c-code-?forum=netfxbcl</remarks>
        /// <param name="s">Input string</param>
        /// <returns></returns>
        private static IntPtr StringToByteArray(string s)
        {
            IntPtr p = new IntPtr();
            byte[] b = new byte[s.Length + 1];
            int i;
            for (i = 0; i < s.Length; i++)
                b[i] = (byte)s.ToCharArray()[i];
            b[s.Length] = 0;
            p = Marshal.AllocCoTaskMem(s.Length + 1);
            Marshal.Copy(b, 0, p, s.Length + 1);
            return p;
        }

        /// <summary>
        /// This method calls the DLL where the FLEX+BISON is wrapped
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        private static int DLLcallToParser(string Data)
        {
            IntPtr[] argv = new IntPtr[1];
            argv[0] = StringToByteArray(Data + "\n");
            return CallParserFromDLL(1, argv);
        }
        #endregion


    }

}

