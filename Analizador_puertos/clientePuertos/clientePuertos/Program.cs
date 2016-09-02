using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace clientePuertos
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.Sockets.UdpClient sock = new System.Net.Sockets.UdpClient();
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 3000);
            byte[] data = Encoding.ASCII.GetBytes("Hola Servidor UDP!!");
            sock.Send(data, data.Length, iep);
            sock.Close();

            Console.WriteLine("Mensaje enviado.");

            Console.WriteLine("Esperando para conexión...!!");

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 3500);
            Socket socktcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socktcp.Bind(localEndPoint);
            socktcp.Listen(10);
            Socket handler = socktcp.Accept();
            Console.WriteLine("Conexión recibida de " + ((IPEndPoint)handler.RemoteEndPoint).Address.ToString());
            String datos = null;

            byte[] bytes;


            while (true)
            {
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                datos += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                

                if (datos.Length > 0)
                {
                    Console.WriteLine("Explorando puertos de: {0}", datos);

                    string[] aux = datos.Split(' ');
                    IPAddress direccion = IPAddress.Parse(aux[0]);
                    String[] puertos = aux[1].Split('-');
                    int pInicial = Int32.Parse(puertos[0]);
                    int pFinal = Int32.Parse(puertos[1]);

                    String p = null;
                    for (int CurrPort = pInicial; CurrPort <= pFinal; CurrPort++)
                    {
                        TcpClient TcpScan = new TcpClient();
                        try
                        {
                            TcpScan.Connect(direccion, CurrPort);
                            Console.WriteLine("Port " + CurrPort + " open");
                            p += "Port " + CurrPort + " open \n";
                        }
                        catch
                        {
                            Console.WriteLine("Port " + CurrPort + " closed");
                            p += "Port " + CurrPort + " closed \n";
                        }
                    }

                    byte[] msg = Encoding.ASCII.GetBytes(p);
                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                    break;
                }


            }

            Console.ReadLine();

        }


        
    }
}
