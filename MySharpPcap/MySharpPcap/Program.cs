using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SharpPcap;
using PacketDotNet;
using SharpPcap.LibPcap;  

namespace MySharpPcap
{
    class Program
    {
        static void Main(string[] args)
        {

            printDeviceList();

            WinCapHelper.WinCapInstance._logAction = Console.WriteLine;
            WinCapHelper.WinCapInstance.Listen();

            while(true)
            {
                Thread.Sleep(1000);

            }
        }

        static void printDeviceList()
        {
            // Print SharpPcap version 
            string ver = SharpPcap.Version.VersionString;
            Console.WriteLine("SharpPcap {0}, Example1.IfList.cs", ver);

            // Retrieve the device list
            CaptureDeviceList devices = CaptureDeviceList.Instance;

            // If no devices were found print an error
            if (devices.Count < 1)
            {
                Console.WriteLine("No devices were found on this machine");
                return;
            }

            Console.WriteLine("\nThe following devices are available on this machine:");
            Console.WriteLine("----------------------------------------------------\n");

            // Print out the available network devices
            foreach (ICaptureDevice dev in devices)
                Console.WriteLine("{0}\n", dev.ToString());

            Console.Write("Hit 'Enter' to exit...");
            Console.ReadLine();
        }
    }
}
