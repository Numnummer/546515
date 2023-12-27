using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static GameClient.Game1ProtocolHelper;

namespace GameClient
{
    public class GamePackageHelper
    {
        public static byte[] MakeMessage(byte command, byte specialCommand, string content)
        {
            var byteContent = Encoding.UTF8.GetBytes(content);
            var pack = new byte[PackLength];
            Array.Copy(BasePackageStart, pack, BasePackageStart.Length);
            pack[CommandByteIndex] = command;
            pack[SpecialCommandByteIndex] = specialCommand;
            pack[FullnessIndex]=FullPack;
            for (var i = 0; i < byteContent.Length; i++)
            {
                pack[i + BaseLength] = byteContent[i];
            }
            pack[^1]=LastByte;
            return pack;
        }
        public static byte[] MakeHiMessage(byte specialCommand, string content)
            => MakeMessage(CommandHi, specialCommand, content);
        public static byte[] MakeByeMessage(byte specialCommand, string content)
            => MakeMessage(CommandBye, specialCommand, content);
        public static byte[] MakeMessageWithName(byte command, byte specialCommand, string name, string content)
            => MakeMessage(command, specialCommand, name+" "+content);

        public static byte[] GetContent(byte[] bytes)
            => bytes.Skip(BaseLength).Take(bytes.Length-BaseLength-1).ToArray();
        public static (string, string) GetNameAndContent(byte[] bytes)
        {
            var content = GetContent(bytes);
            var fullString = Encoding.UTF8.GetString(content);
            var splitted = fullString.Split(' ');
            return (splitted[0], splitted[1]);
        }
    }
}
