using System;
using System.Net.Sockets;

namespace echo_server
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine(socket);
            socket.Close();
        }
    }
}
