using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server {
    class SynchronousSocketListener {
        // Client로부터 오는 데이터
        public static string data = null;

        public static void StartListening() {
            // 데이터를 위한 buffer
            byte[] bytes = new Byte[1024];

            // socket을 위한 local endpoint 작성
            // Dns.GetHostName은 앱을 실행하는 호스트의 이름을 반환
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while(true) {
                    Console.WriteLine("연결을 기다리고 있습니다...");
                    Socket handler = listener.Accept();
                    data = null;

                    while(true) {
                        int byteRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, byteRec);
                        if (data.IndexOf("<EOF>") > -1) {
                            break;
                        }
                    }

                    Console.WriteLine("Text received : {0}", data);
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            
            Console.WriteLine("Press ENTER to continue...");
            Console.Read();
        }

        public static int Main(string[] args) {
            StartListening();
            return 0;
        }
    }
}
