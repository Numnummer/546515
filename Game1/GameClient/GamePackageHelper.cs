using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameClient.Game1ProtocolHelper;

namespace GameClient
{
    public class GamePackageHelper
    {
        public static byte[] MakeMessage(byte specialCommand, string content)
        {
            var byteContent = Encoding.UTF8.GetBytes(content);
            var pack = new byte[BaseLength+byteContent.Length+1];
            Array.Copy(BasePackageStart, pack, BasePackageStart.Length);
            pack[CommandByteIndex] = CommandServer;
            pack[SpecialCommandByteIndex] = specialCommand;
            for (var i = 0; i < byteContent.Length; i++)
            {
                pack[i + BaseLength] = byteContent[i];
            }
            pack[^1]=LastByte;
            return pack;
        }
        public static byte[] GetContent(byte[] bytes)
            => bytes.Skip(BaseLength).Take(bytes.Length-1).ToArray();
    }
}
