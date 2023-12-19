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
        public static readonly byte LastByte = 0x01;
        public static bool IsQueryValid(byte[] buffer) =>
            buffer.SequenceEqual(BasePackageStart)
                && buffer[^1]==LastByte;
    }
}
