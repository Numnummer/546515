using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameClient
{
    public class Server
    {
        private readonly Socket _socket;
        private readonly Dictionary<Socket, string> _clients = new();
        public Server(IPAddress address, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(new IPEndPoint(address, port));
        }
        public async Task ListenAsync()
        {
            _socket.Listen();
            var connectionSocket = await _socket.AcceptAsync();
            _clients.Add(connectionSocket, $"player{_clients.Count+1}");
            _=Task.Run(async () =>
            {
                await ProcessClient(connectionSocket);
            });
        }
        public async Task ProcessClient(Socket socket)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await socket.ReceiveAsync(buffer);
        }
    }
}
