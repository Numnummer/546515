using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameClient
{
    public class Client
    {
        private readonly Socket _socket;
        private readonly string _name;
        private IPEndPoint _endPoint;
        public Client(IPEndPoint endPoint, string name)
        {
            _name = name;
            _endPoint = endPoint;
            _socket=new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }
        public async Task StartWorkAsync()
        {
            await _socket.ConnectAsync(_endPoint);
            var message = GamePackageHelper.MakeMessage(Game1ProtocolHelper.SpecialCommandNewPlayer, _name);
            await _socket.SendAsync(message);
        }
        public async Task SendScoreAsync(string score)
        {
            var message = GamePackageHelper.MakeMessage(Game1ProtocolHelper.SpecialCommandNewScore, score);
            await _socket.SendAsync(message);
        }
        public async Task EndWorkAsync()
        {
            var message = GamePackageHelper.MakeMessage(Game1ProtocolHelper.SpecialCommandPlayerLeaved, _name);
            await _socket.SendAsync(message);
            _socket.Close();
        }
    }
}
