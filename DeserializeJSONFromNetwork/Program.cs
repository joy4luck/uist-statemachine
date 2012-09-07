using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace DeserializeJSONFromNetwork
{
    class SensorData
    {
        public double[] corners; // length 4: corners 0-3
        public bool[] touched; // length 5: fingers 0-4
        public double[] f0; // length 3: coordinates x,y,z
        public double[] f1; // length 3: coordinates x,y,z
        public double[] f2; // length 3: coordinates x,y,z
        public double[] f3; // length 3: coordinates x,y,z

        public override string ToString()
        {
            List<string> output = new List<string>();
            output.Add("corners: ");
            output.Add(corners.PrintArray());
            output.Add(touched.PrintArray());
            output.Add(f0.PrintArray());
            output.Add(f1.PrintArray());
            output.Add(f2.PrintArray());
            output.Add(f3.PrintArray());
            StringBuilder outstring = new StringBuilder();
            outstring.Append("{");
            outstring.Append(String.Join(",", String.Join(",", output)));
            outstring.Append("}");
            return outstring.ToString();
        }

        /* Counts how many of the first fingers are currently touching.
         */
        public int FingerCount()
        {
            int c = 0;
            for (int i = 0; i < 5; i++)
            {
                if (touched[i])
                {
                    c++;
                }
                else
                {
                    return c;
                }
            }
            return c;
        }

        /* Returns the distance, in touchpad pixels, between two fingers currently touching.
         * 
         */
        public double Distance()
        {
            double dx = f0[0] - f1[0];
            double dy = f0[1] - f1[1];
            return System.Math.Sqrt(dx * dx + dy * dy);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            WebClient webClient = new WebClient();
            string IPaddress = webClient.DownloadString("http://transgame.csail.mit.edu:9537/?varname=jedeyeserver");
            TcpClient client = new TcpClient(IPaddress, 1101);
            TextReader reader = new StreamReader(client.GetStream());
            DrawClass dc = new DrawClass();
            while (true)
            {
                string data = reader.ReadLine();
                //Console.WriteLine(data);
                SensorData sensor = Newtonsoft.Json.JsonConvert.DeserializeObject<SensorData>(data);
                dc.HandleSensorData(sensor);
                Console.WriteLine(dc.state);
                //dc.UpdatePlot(sensor);
                //Console.WriteLine(dc.ToString());
                //Console.WriteLine(sensor);
            }
        }
    }
}
