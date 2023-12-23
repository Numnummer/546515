using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GameClient.Game1ProtocolHelper;

namespace GameClient
{
    public class Server
    {
        private readonly Socket _socket;
        private readonly Dictionary<Socket, string> _clients = new();
        private readonly Mutex _mutex;
        public Server(IPAddress address, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(new IPEndPoint(address, port));
            _mutex = new Mutex(false, "game_mutex1");
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
            byte[] buffer = new byte[4096];
            _ = await socket.ReceiveAsync(buffer);
            var contentList = new List<byte>();
            while (socket.Connected)
            {
                if (IsQueryValid(buffer))
                {
                    if (buffer[CommandByteIndex]==CommandHi)
                    {
                        var content = Encoding.UTF8.GetString(GamePackageHelper.GetContent(buffer));
                        await BroadcastMessageAsync(SpecialCommandNewPlayer, content);
                        continue;
                    }
                    if (buffer[CommandByteIndex]==CommandBye)
                    {
                        await socket.DisconnectAsync(false);
                        _clients.Remove(socket);
                        var content = Encoding.UTF8.GetString(GamePackageHelper.GetContent(buffer));
                        await BroadcastMessageAsync(SpecialCommandPlayerLeaved, content);
                        return;
                    }
                    if (buffer[CommandByteIndex]==CommandSay)
                    {
                        contentList.AddRange(GamePackageHelper.GetContent(buffer));
                        if (IsQueryFull(buffer))
                        {
                            var content = Encoding.UTF8.GetString(GamePackageHelper.GetContent(contentList.ToArray()));
                            await BroadcastMessageAsync(SpecialCommandNewScore, content);
                            contentList.Clear();
                        }
                    }
                }
            }
        }

        private async Task BroadcastMessageAsync(byte specialCommand, string content)
        {
            var message = GamePackageHelper.MakeMessage(specialCommand, content);
            foreach (var client in _clients.Keys)
            {
                _mutex.WaitOne();
                await client.SendAsync(message, SocketFlags.None);
                _mutex.ReleaseMutex();
            }
        }
    }
}
