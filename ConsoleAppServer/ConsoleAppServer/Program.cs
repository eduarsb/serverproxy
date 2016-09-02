using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Management;
using System.IO;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace ConsoleAppServer
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Esperando al cliente");
            TcpListener tcpListener = new TcpListener(3000);
            tcpListener.Start();
 
            Socket socketForClient = tcpListener.AcceptSocket();
 
            if (socketForClient.Connected)
            {
                Console.WriteLine("Cliente conectado.");
                NetworkStream networkStream = new NetworkStream(socketForClient);
                StreamWriter streamWriter = new StreamWriter(networkStream); 
                StreamReader streamReader = new StreamReader(networkStream);

                string msg = "Conectado con el servidor. Escribe ? para obtener informacion \n";
                Boolean flag = true;
  
                try
                {
                    streamWriter.WriteLine(msg);
                    streamWriter.Flush();
                    while (flag)
                    {
                        msg = streamReader.ReadLine();
                        Console.WriteLine(msg);
                        switch (msg)
                        {
                            case "sistema": msg = GetSistema();
                                break;
                            case "red": msg = GetRed();
                                break;
                            case "discos": msg = GetDiscos();
                                break;
                            case "procesos": msg = GetProcesos();
                                break;
                            case "salir": flag = false;
                                msg = "Adios";
                                break;
                            case "?": msg = "Los comandos disponbles son: sistema, red, discos, procesos y salir";
                                break;
                            default: msg = "Comando no valido";
                                break;

                        }
                        streamWriter.WriteLine(msg);
                        streamWriter.Flush();
                        
                    }
                }
                finally
                {
                    //Cerramos las conexiones
                    streamReader.Close();
                    streamWriter.Close();
                    networkStream.Close();
                    socketForClient.Close();
                }
            }


        }

        public static string GetSistema()
        {
            string info;

            info = "Usuario: "+ Environment.UserName +"\n\r";
            info += "Dominio: "+Environment.UserDomainName +"\n\r";
            info += "SO Version: "+ Environment.OSVersion.ToString() +"\n\r";

            DateTime localDate = DateTime.Now;
            var culture = new CultureInfo("es-MX");
            info += "es-MX "+ localDate.ToString(culture)+"\n\r";

            ManagementObject Mo = new ManagementObject("Win32_Processor.DeviceID='CPU0'");
            uint sp = (uint)(Mo["CurrentClockSpeed"]);
            info += "Velocidad del Procesador: " + sp + " Mhz\n\r";
            Mo.Dispose();

            return info;
        }

        public static string GetRed()
        {
            string info;

            /* Tarjeta de red */
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            info = "Interface information for      " + computerProperties.HostName + " " + computerProperties.DomainName + "\n\r";
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                info += adapter.Description + "\n\r";
                info += String.Empty.PadLeft(adapter.Description.Length, '=') + "\n\r";
                info += "  Interface type .......................... : " + adapter.NetworkInterfaceType + "\n\r";
                info += "  Physical Address ........................ : " + adapter.GetPhysicalAddress().ToString() + "\n\r";
                info += "  Is receive only.......................... : " + adapter.IsReceiveOnly + "\n\r";
                info += "  Multicast................................ : " + adapter.SupportsMulticast + "\n\r";
                info += "\n\r";
            }

            return info;
        }

        public static string GetDiscos()
        {
            string info ="";

            /* Unidades de disco */
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                info += "Drive " + d.Name + "\n\r";
                info += "  Drive type " + d.DriveType + "\n\r";
                if (d.IsReady == true)
                {
                    info += "  Volume label: " + d.VolumeLabel + "\n\r";
                    info += "  File system: " + d.DriveFormat + "\n\r";
                    info += "  Available space to current user: " + d.AvailableFreeSpace + " bytes\n\r";
                    info += "  Total available space:           " + d.TotalFreeSpace + " bytes\n\r";
                    info += "  Total size of drive:             " + d.TotalSize + " bytes\n\r";
                }
            }

            return info;
        }

        public static string GetProcesos()
        {
            string info = "";

            Process[] localAll = Process.GetProcesses();
            foreach (Process myProcess in localAll)
            {
                info += myProcess.ProcessName + "\n\r";
            }

            return info;
        }
    }
}
