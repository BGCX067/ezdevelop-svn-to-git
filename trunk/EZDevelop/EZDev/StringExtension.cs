using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZDev
{
    public static class StringExtension
    {
        public static byte[] ToByteArray(this string self)
        {
            return self.ToCharArray().Cast<byte>().ToArray();
        }

    }
}
