using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace Imprinno.Core.Handlers
{
    public class NetworkHandler
    {
        public string GetIPAddress(HttpContext context)
        {
            return context.Connection.RemoteIpAddress.ToString();
        }

        public double GetInternetSpeed()
        {
            double speed = 0; //kbps
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            var nic = nics.Single(n => n.Name == "Local Area Connection"); // Select desired NIC
            var reads = Enumerable.Empty<double>();
            var sw = new Stopwatch();
            var lastBr = nic.GetIPv4Statistics().BytesReceived;
            for (var i = 0; i < 1000; i++)
            {
                sw.Restart();
                Thread.Sleep(100);
                var elapsed = sw.Elapsed.TotalSeconds;
                var br = nic.GetIPv4Statistics().BytesReceived;

                var local = (br - lastBr) / elapsed;
                lastBr = br;

                // Keep last 20, ~2 seconds
                reads = new[] { local }.Concat(reads).Take(20);

                if (i % 10 == 0)
                { // ~1 second
                    var bSec = reads.Sum() / reads.Count();
                    var kbps = (bSec * 8) / 1024;
                    //Console.WriteLine("Kb/s ~ " + kbps);
                    speed = kbps;
                }
            }

            return speed;
        }
    }
}
