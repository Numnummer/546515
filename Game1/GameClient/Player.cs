using System.Net;
using System.Net.Sockets;

namespace GameClient
{
    public class Player
    {
        private readonly Server? _server;
        private readonly Client _client;
        public Player(Mode mode, string name)
        {
            switch (mode)
            {
                case Mode.Server:
                    _server=new Server(new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 5007));
                    _=_server.ListenAsync();
                    _client=new Client(new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 5007), name);
                    break;
                case Mode.Client:
                    _server=null;
                    _client=new Client(new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 5007), name);
                    break;
            }
            if (_client==null)
            {
                throw new ArgumentException("Invalid mode", nameof(mode));
            }
            _=_client.StartWorkAsync();
        }

        public void BindOnNewPlayer(Action<string> action) =>
            _client.OnNewPlayer+=action;
        public void BindOnNewScore(Action<string, string> action) =>
            _client.OnNewScore+=action;
        public void BindOnPlayerLeaved(Action<string> action) =>
            _client.OnPlayerLeaved+=action;
        public async Task SendScoreAsync(string score) =>
            await _client.SendScoreAsync(score);
        public async Task EndWorkAsync() =>
            await _client.EndWorkAsync();
    }
}
