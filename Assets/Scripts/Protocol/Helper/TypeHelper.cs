using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace UnityMVC.Protocol.Helper
{

    public static class TypeHelper
    {
        public static byte getByte(this Int32 i)
        {
            return (byte)i;
        }

        public static int getInt32(this string str)
        {
            return Convert.ToInt32(str);
        }

        public static string ToStringValue(this byte[] bts)
        {
            //string str = SerializeHelper.Deserialize(bts) as string;
            return string.Empty;
        }
    }
}
