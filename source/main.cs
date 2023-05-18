// Have fun :) - Solaris
using System;
using System.IO.Ports;
using System.Text;

namespace UltrashipU2
{
    class Program
    {
        static void Main(string[] args)
        {

            // Configure serial port
            SerialPort port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
            port.ReadTimeout = 1000;
            port.Open();

            while (true)
            {
                // Read scale data
                byte[] buffer = new byte[10];
                int bytesRead = 0;
                try
                {
                    bytesRead = port.Read(buffer, 0, buffer.Length);
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("Waiting...");
                    continue;
                }
                //Decoding
                string response = BitConverter.ToString(buffer, 0, bytesRead);

                if (response == "02")
                {
                    Console.WriteLine("---SEND button pressed---");
                }
                else if (response.StartsWith("0B-44"))
                {
                    double Weigh;
                    if (double.TryParse(Encoding.ASCII.GetString(buffer, 2, bytesRead - 2), out Weigh))
                    {
                        Console.WriteLine("Peso: {0}", Weigh);
                        //Note: You could add the measure unit of your choice here.
                    }
                    else
                    {
                        Console.WriteLine("Decode error");
                    }
                }
                else if (response.StartsWith("47-4B-03"))
                {
                    Console.WriteLine("---End of data transmission---");
                }
                else
                {
                    Console.WriteLine("Unknown response: {0}", response);
                }
            }

        }
    }
}
