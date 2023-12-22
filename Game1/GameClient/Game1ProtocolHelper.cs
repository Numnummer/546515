using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient
{
    public static class Game1ProtocolHelper
    {
        public static readonly byte[] BasePackageStart =
        {
            0x07, 0x07,
            0x08, 0x09,
            0x17, 0x17
        };
        public const int BaseLength = 8;
        public static readonly byte LastByte = 0x01;
        public const int CommandByteIndex = 6;
        public const int SpecialCommandByteIndex = 7;
        public const byte CommandHi = 0x21;
        public const byte CommandSay = 0x22;
        public const byte CommandBye = 0x23;
        public const byte CommandServer = 0x01;
        public const byte SpecialCommandNewPlayer = 0x30;
        public const byte SpecialCommandNewScore = 0x31;
        public const byte SpecialCommandPlayerLeaved = 0x32;
        public static bool IsQueryValid(byte[] buffer) =>
            buffer.SequenceEqual(BasePackageStart)
                && buffer[^1]==LastByte;
    }
}
