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
                
                // Decoding
                string response = BitConverter.ToString(buffer, 0, bytesRead);

                switch (response)
                {
                    case "02":
                        Console.WriteLine("---SEND button pressed---");
                        break;
                    case string r when r.StartsWith("0B-44"):
                        double weight;
                        if (double.TryParse(Encoding.ASCII.GetString(buffer, 2, bytesRead - 2), out weight))
                        {
                            Console.WriteLine("Weight: {0}", weight);
                            // Note: You could add the measure unit of your choice here.
                        }
                        else
                        {
                            Console.WriteLine("Decode error");
                        }
                        break;
                    case "47-4B-03": //Update this vaule! My scale was set to g and Kg
                        Console.WriteLine("---End of data transmission---");
                        break;
                    default:
                        Console.WriteLine("Unknown response: {0}", response);
                        break;
                }
            }
        }
    }
}
