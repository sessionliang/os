using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Text;

namespace BaiRong.Core.IO
{
    public class EncodedStringWriter : StringWriter
    {
        private Encoding encoding;

        public EncodedStringWriter(StringBuilder builder, Encoding encoding)
            : base(builder)
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get
            {
                return this.encoding;
            }
        }
    }
}
