using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FacialRecog
{
    class Program
    {
        private static string apiKey = "fb0e516976164428bba74979f28e4269";
        private static string apiRoot = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0";
        private static readonly IFaceServiceClient faceServiceClient =
            new FaceServiceClient(apiKey, apiRoot);


        static void Main(string[] args)
        {
            // get file from url
            using (WebClient client = new WebClient())
            {
                var url = "https://raw.githubusercontent.com/MissionMarsFourthHorizon/Mission-Briefings/master/CognitiveServices/images/CrewPhoto.jpg";
                var fileName = @"c:\file13\crewphoto.jpg";
                client.DownloadFile(new Uri(url), fileName);
                UploadAndDetectFaces(fileName);
                Console.ReadLine();

            }
        }
 
        private async static void UploadAndDetectFaces(string imageFilePath)
        {
            try
            {
                using (Stream imageFileStream = File.OpenRead(imageFilePath))
                {
                    var faces = await faceServiceClient.DetectAsync(imageFileStream);
                    var faceRects = faces.Select(face => face.FaceRectangle);
                    Console.WriteLine("Found {0} faces", faceRects.Count());
                }
            }
            catch (Exception)
            {
                Console.WriteLine("unable to parse faces");
            }
        }
    }
}
