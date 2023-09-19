using System;

using PCSC;
namespace nfc_reader
{

    class Program
    {
        static void Main(string[] args)
        {
            var contextFactory = ContextFactory.Instance;
            using (var context = contextFactory.Establish(SCardScope.System))
            {
                var readerNames = context.GetReaders();

                if (readerNames.Length == 0)
                {
                    Console.WriteLine("No NFC reader found.");
                    return;
                }

                Console.WriteLine("Available NFC readers:");
                foreach (var readerName in readerNames)
                {
                    Console.WriteLine(readerName);
                }

                // Select a reader (you can choose one from the list)
                var selectedReader = readerNames[0]; // Replace with the desired reader name

                Console.WriteLine("Using NFC reader: " + selectedReader);

                //using (var reader = context.ConnectReader(selectedReader, SCardShareMode.Shared, SCardProtocol.Any))
                //{
                
                context.Establish(SCardScope.System);
                
                Console.WriteLine("Connected to NFC reader: " + selectedReader);

                // Send an APDU command to read data from the NFC card
                byte[] command = new byte[] { 0xFF, 0xCA, 0x00, 0x00, 0x00 }; // Example APDU command
                var responseBuffer = new byte[256];
                //var responseLength = reader.Transmit(command, responseBuffer);

                if (responseLength >= 2 && responseBuffer[responseLength - 2] == 0x90 && responseBuffer[responseLength - 1] == 0x00)
                {
                    byte[] responseData = new byte[responseLength - 2]; // Exclude the status bytes
                    Array.Copy(responseBuffer, responseData, responseLength - 2);

                    string textData = BitConverter.ToString(responseData);
                    Console.WriteLine("Read data from NFC card: " + textData);
                }
                else
                {
                    Console.WriteLine("Failed to read data from NFC card.");
                }

                // To keep the application running and prevent it from exiting immediately
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                //}
            }
        }
    }
}

