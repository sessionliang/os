using System;
using System.Collections.Generic;
using System.Text;

namespace BaiRong.Core
{
    public class Base64Encoder
    {
        // Fields
        private int blockCount;
        private int length;
        private int length2;
        private int paddingCount;
        private byte[] source;

        // Methods
        public Base64Encoder()
        {
        }

        public Base64Encoder(byte[] input)
        {
            this.source = input;
            this.length = input.Length;
            if ((this.length % 3) == 0)
            {
                this.paddingCount = 0;
                this.blockCount = this.length / 3;
            }
            else
            {
                this.paddingCount = 3 - (this.length % 3);
                this.blockCount = (this.length + this.paddingCount) / 3;
            }
            this.length2 = this.length + this.paddingCount;
        }

        public char[] GetEncoded()
        {
            int x;
            byte[] source2 = new byte[this.length2];
            for (x = 0; x < this.length2; x++)
            {
                if (x < this.length)
                {
                    source2[x] = this.source[x];
                }
                else
                {
                    source2[x] = 0;
                }
            }
            byte[] buffer = new byte[this.blockCount * 4];
            char[] result = new char[this.blockCount * 4];
            for (x = 0; x < this.blockCount; x++)
            {
                byte b1 = source2[x * 3];
                byte b2 = source2[(x * 3) + 1];
                byte b3 = source2[(x * 3) + 2];
                byte temp1 = (byte)((b1 & 0xfc) >> 2);
                byte temp = (byte)((b1 & 3) << 4);
                byte temp2 = (byte)((b2 & 240) >> 4);
                temp2 = (byte)(temp2 + temp);
                temp = (byte)((b2 & 15) << 2);
                byte temp3 = (byte)((b3 & 0xc0) >> 6);
                temp3 = (byte)(temp3 + temp);
                byte temp4 = (byte)(b3 & 0x3f);
                buffer[x * 4] = temp1;
                buffer[(x * 4) + 1] = temp2;
                buffer[(x * 4) + 2] = temp3;
                buffer[(x * 4) + 3] = temp4;
            }
            for (x = 0; x < (this.blockCount * 4); x++)
            {
                result[x] = this.sixbit2char(buffer[x]);
            }
            switch (this.paddingCount)
            {
                case 0:
                    return result;

                case 1:
                    result[(this.blockCount * 4) - 1] = '=';
                    return result;

                case 2:
                    result[(this.blockCount * 4) - 1] = '=';
                    result[(this.blockCount * 4) - 2] = '=';
                    return result;
            }
            return result;
        }

        private char sixbit2char(byte b)
        {
            char[] lookupTable = new char[] { 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 
            'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 
            'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/'
         };
            if ((b >= 0) && (b <= 0x3f))
            {
                return lookupTable[b];
            }
            return ' ';
        }
    }

 

}
