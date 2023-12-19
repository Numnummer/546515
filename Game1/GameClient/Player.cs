using System.Net.Sockets;

namespace GameClient
{
    public class Player
    {
        private readonly Socket _socket;
        private readonly Dictionary<Socket, string> _players = new();
        public Player()
        {

        }
    }
}
