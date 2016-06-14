using System;
using System.Collections.Generic;
using System.Text;

namespace BaiRong.Core
{
    public class Base64Decoder
    {
        // Fields
        private int blockCount;
        public static Base64Decoder Decoder = new Base64Decoder();
        private int length;
        private int length2;
        private int length3;
        private int paddingCount;
        private char[] source;

        // Methods
        private byte char2sixbit(char c)
        {
            char[] lookupTable = new char[] { 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 
            'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 
            'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/'
         };
            if (c != '=')
            {
                for (int x = 0; x < 0x40; x++)
                {
                    if (lookupTable[x] == c)
                    {
                        return (byte)x;
                    }
                }
            }
            return 0;
        }

        public byte[] GetDecoded(string strInput)
        {
            int x;
            this.init(strInput.ToCharArray());
            byte[] buffer = new byte[this.length];
            byte[] buffer2 = new byte[this.length2];
            for (x = 0; x < this.length; x++)
            {
                buffer[x] = this.char2sixbit(this.source[x]);
            }
            for (x = 0; x < this.blockCount; x++)
            {
                byte temp1 = buffer[x * 4];
                byte temp2 = buffer[(x * 4) + 1];
                byte temp3 = buffer[(x * 4) + 2];
                byte temp4 = buffer[(x * 4) + 3];
                byte b = (byte)(temp1 << 2);
                byte b1 = (byte)((temp2 & 0x30) >> 4);
                b1 = (byte)(b1 + b);
                b = (byte)((temp2 & 15) << 4);
                byte b2 = (byte)((temp3 & 60) >> 2);
                b2 = (byte)(b2 + b);
                b = (byte)((temp3 & 3) << 6);
                byte b3 = temp4;
                b3 = (byte)(b3 + b);
                buffer2[x * 3] = b1;
                buffer2[(x * 3) + 1] = b2;
                buffer2[(x * 3) + 2] = b3;
            }
            this.length3 = this.length2 - this.paddingCount;
            byte[] result = new byte[this.length3];
            for (x = 0; x < this.length3; x++)
            {
                result[x] = buffer2[x];
            }
            return result;
        }

        private void init(char[] input)
        {
            int temp = 0;
            this.source = input;
            this.length = input.Length;
            for (int x = 0; x < 2; x++)
            {
                if (input[(this.length - x) - 1] == '=')
                {
                    temp++;
                }
            }
            this.paddingCount = temp;
            this.blockCount = this.length / 4;
            this.length2 = this.blockCount * 3;
        }
    }


}
