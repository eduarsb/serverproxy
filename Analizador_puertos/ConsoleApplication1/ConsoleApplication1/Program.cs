using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace servidorPuertos
{
    class Program
    {

        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;

            Console.WriteLine("Esperando conexiones");
            
           
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            int i = 0;
            List<IPAddress> direcciones = new List<IPAddress>();
            Boolean flag = true;
            while (flag)
            {
                System.Net.Sockets.UdpClient server = new System.Net.Sockets.UdpClient(3000);
                byte[] data = new byte[1024];
                data = server.Receive(ref sender);
                server.Close();
                string stringData = Encoding.ASCII.GetString(data, 0, data.Length);

                Console.WriteLine("Respondiendo desde " + sender.Address + Environment.NewLine + "Mensaje: " + stringData);
                Console.WriteLine();
                direcciones.Add(sender.Address);

                Console.WriteLine("Deseas esperar otra conexion? Y/N");
                cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.N)
                {
                    flag = false;
                }
                else
                {
                    Console.WriteLine("Esperando otra conexion");
                    Console.WriteLine("");
                }

            }
           

            Console.WriteLine("Escribe la direccion y el rango de puertos a escanear, ejemplo: 192.168.1.1 1-25");
            String texto = Console.ReadLine();
           
            string[] aux = texto.Split(' ');
            IPAddress direccion = IPAddress.Parse(aux[0]);
            String[] puertos = aux[1].Split('-');
            int pInicial = Int32.Parse(puertos[0]);
            int pFinal = Int32.Parse(puertos[1]); 

            try
            {
                if (direcciones.Count > 0)
                //if (true)
                {
                    //int total = direcciones.Count;
                    int rango = ((pFinal - pInicial) + 1) / (direcciones.Count + 1);
                 
                    foreach (IPAddress d in direcciones)
                    {
                        int pf = pInicial + rango;
                        String scan = direccion + " " + pInicial + "-" + pf;
                        Console.WriteLine(scan);
                        string result = Cliente(d, scan);
                        Console.WriteLine(result);
                        pInicial = pf + 1;
                    }

                    for (int CurrPort = pInicial; CurrPort <= pFinal; CurrPort++)
                    {
                        TcpClient TcpScan = new TcpClient();
                        try
                        {
                            TcpScan.Connect(direccion, CurrPort);
                            Console.WriteLine("Port " + CurrPort + " open");
                        }
                        catch
                        {
                            Console.WriteLine("Port " + CurrPort + " closed");
                        }
                    }
                    
                }
                else
                {
                    for (int CurrPort = pInicial; CurrPort <= pFinal; CurrPort++)
                    {
                        TcpClient TcpScan = new TcpClient();
                        try
                        {
                            TcpScan.Connect(direccion, CurrPort);
                            Console.WriteLine("Port " + CurrPort + " open");
                        }
                        catch
                        {
                            Console.WriteLine("Port " + CurrPort + " closed");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();            
        }

        private static string Cliente(IPAddress servidor, String ip)
        {
            try
            {
                Byte[] bytesSent = Encoding.ASCII.GetBytes(ip);
                Byte[] bytesReceived = new Byte[256];

                // Crear socket ip, puerto
                IPEndPoint ipe = new IPEndPoint(servidor, 3500);
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.Connect(ipe);

                if (s == null) return "Connexion falló!";

                s.Send(bytesSent, bytesSent.Length, 0);

                try
                {
                    byte[] data = new byte[1024];
                    int receivedDataLength = s.Receive(data);
                    string stringData = Encoding.ASCII.GetString(data, 0, receivedDataLength);
                    Console.WriteLine(stringData);

                    s.Shutdown(SocketShutdown.Both);
                    s.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            catch (Exception se)
            {
                Console.WriteLine("Error en conexión" + se.StackTrace);
            }
            return "";
        }

        
    }
}
