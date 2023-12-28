using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static GameClient.Game1ProtocolHelper;

namespace GameClient
{
    public class Client
    {
        private readonly Socket _socket;
        private readonly string _name;
        public event Action<string> OnNewPlayer = (string name) => { };
        public event Action<string, string> OnNewScore = (string name, string score) => { };
        public event Action<string> OnPlayerLeaved = (string name) => { };
        private IPEndPoint _endPoint;
        public Client(IPEndPoint endPoint, string name)
        {
            _name = name;
            _endPoint = endPoint;
            _socket=new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public async Task StartWorkAsync()
        {
            await _socket.ConnectAsync(_endPoint);
            var message = GamePackageHelper.MakeHiMessage(SpecialCommandNewPlayer, _name);
            var c = await _socket.SendAsync(message);
            _ =Task.Run(async () =>
            {
                while (_socket.Connected)
                {
                    await ListenAsync();
                }
            });
        }
        public async Task SendScoreAsync(string score)
        {
            var message = GamePackageHelper.MakeMessageWithName(CommandSay, SpecialCommandNewScore, _name, score);
            await _socket.SendAsync(message);
        }
        public async Task EndWorkAsync()
        {
            var message = GamePackageHelper.MakeByeMessage(SpecialCommandPlayerLeaved, _name);
            await _socket.SendAsync(message);
            _socket.Close();
        }
        public async Task ListenAsync()
        {
            var buffer = new byte[PackLength];
            try
            {
                await _socket.ReceiveAsync(buffer);
            }
            catch (Exception ex)
            {

                throw;
            }

            if (buffer[SpecialCommandByteIndex]==SpecialCommandNewPlayer)
            {
                var nameBytes = GamePackageHelper.GetContent(buffer);
                var name = Encoding.UTF8.GetString(nameBytes);
                OnNewPlayer(name);
                return;
            }
            if (buffer[SpecialCommandByteIndex]==SpecialCommandNewScore)
            {
                var nameAndScore = GamePackageHelper.GetNameAndContent(buffer);
                OnNewScore(nameAndScore.Item1, nameAndScore.Item2);
                return;
            }
            if (buffer[SpecialCommandByteIndex]==SpecialCommandPlayerLeaved)
            {
                var nameBytes = GamePackageHelper.GetContent(buffer);
                var name = Encoding.UTF8.GetString(nameBytes);
                OnPlayerLeaved(name);
                return;
            }
        }
    }
}
