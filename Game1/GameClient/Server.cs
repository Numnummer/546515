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
using static System.Formats.Asn1.AsnWriter;

namespace GameClient
{
    public class Server
    {
        private readonly Socket _socket;
        private readonly Dictionary<Socket, string> _clients = new();
        private readonly Dictionary<string, string> _scores = new();
        private readonly Mutex _mutex;
        public Server(IPEndPoint endPoint)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(endPoint);
            _mutex = new Mutex(false, "game_mutex1");
        }
        public async Task ListenAsync()
        {
            try
            {
                _socket.Listen();
            }
            catch (Exception e) { await Console.Out.WriteLineAsync(e.Message); }

            while (_socket.IsBound)
            {
                var connectionSocket = await _socket.AcceptAsync();
                _=Task.Run(async () =>
                {
                    await ProcessClient(connectionSocket);
                });
            }
        }
        public async Task ProcessClient(Socket socket)
        {
            while (socket.Connected)
            {
                byte[] buffer = new byte[PackLength];
                _ = await socket.ReceiveAsync(buffer);
                var contentList = new List<byte>();

                if (IsQueryValid(buffer))
                {
                    if (buffer[CommandByteIndex]==CommandHi)
                    {
                        await SendAllClients(socket);
                        await SendAllScores(socket);
                        var content = Encoding.UTF8.GetString(GamePackageHelper.GetContent(buffer));
                        await BroadcastMessageAsync(SpecialCommandNewPlayer, content);
                        _clients.Add(socket, content);
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
                    if (buffer[CommandByteIndex]==CommandSay
                        && buffer[SpecialCommandByteIndex]==SpecialCommandNewScore)
                    {
                        contentList.AddRange(buffer);
                        if (IsQueryFull(buffer))
                        {
                            if (!_scores.ContainsKey(_clients[socket]))
                            {
                                _scores.Add(_clients[socket], GamePackageHelper.GetNameAndContent(contentList.ToArray()).Item2);
                            }
                            else
                            {
                                _scores[_clients[socket]]=GamePackageHelper.GetNameAndContent(contentList.ToArray()).Item2;
                            }
                            var content = Encoding.UTF8.GetString(GamePackageHelper.GetContent(contentList.ToArray()));
                            await BroadcastMessageAsync(SpecialCommandNewScore, content);
                            contentList.Clear();
                        }
                    }
                }
            }
        }

        private async Task SendAllScores(Socket socket)
        {
            foreach (var score in _scores)
            {
                var content = GamePackageHelper.MakeMessageWithName(CommandServer, SpecialCommandNewScore, score.Key, score.Value);
                await socket.SendAsync(content);
                //await Console.Out.WriteLineAsync(score.Key+" "+score.Value);
            }
        }

        private async Task SendAllClients(Socket socket)
        {
            foreach (var client in _clients.Values)
            {
                var content = GamePackageHelper.MakeMessage(CommandServer, SpecialCommandNewPlayer, client);
                await socket.SendAsync(content);
            }
        }

        private async Task BroadcastMessageAsync(byte specialCommand, string content)
        {
            var message = GamePackageHelper.MakeMessage(CommandServer, specialCommand, content);
            foreach (var client in _clients.Keys)
            {
                _mutex.WaitOne();
                await client.SendAsync(message, SocketFlags.None);
                _mutex.ReleaseMutex();
            }
        }
    }
}
