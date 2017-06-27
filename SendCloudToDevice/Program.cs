using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace SendCloudToDevice
{
    class Program
    {
        // the following will change
        static string connectionString = "HostName=DannyS.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=63kQ8I2TzRau8ou6uRjK5Cp3oel1acGCPxcniJWtwIA=";
        static ServiceClient serviceClient;

        static void Main(string[] args)
        {
            Console.WriteLine("Send Cloud-to-Device message\n");
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            Console.WriteLine("Press any key to send a C2D message.");
            Console.ReadLine();
            SendCloudToDeviceMessageAsync().Wait();
            Console.ReadLine();
        }

        private async static Task SendCloudToDeviceMessageAsync()
        {
            //string message = "{\"Command\":\"\",\"Team\":\"\",\"Parameters\":\"\"}";
            string message = "{\"Command\":\"Ping\",\"Team\":\"team01\",\"Parameters\":\"Pinging coffeepot\"}"; var commandMessage = new Message(Encoding.ASCII.GetBytes(message));
            //string message = "{\"Command\":\"Brew\",\"Team\":\"team01\",\"Parameters\":\"\"}"; var commandMessage = new Message(Encoding.ASCII.GetBytes(message));
            commandMessage.Ack = DeliveryAcknowledgement.Full;
            ReceiveFeedbackAsync();
            await serviceClient.SendAsync("coffeepot", commandMessage);
        }

        private async static void ReceiveFeedbackAsync()
        {
            var feedbackReceiver = serviceClient.GetFeedbackReceiver();

            Console.WriteLine("\nReceiving c2d feedback from service");
            while (true)
            {
                var feedbackBatch = await feedbackReceiver.ReceiveAsync();
                if (feedbackBatch == null) continue;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Received feedback: {0}", string.Join(", ", feedbackBatch.Records.Select(f => f.StatusCode)));
                Console.ResetColor();

                await feedbackReceiver.CompleteAsync(feedbackBatch);
            }
        }
    }
}
