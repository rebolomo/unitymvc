////////////////////////////////////////////////////
//// File Name :        FindTargetHelper.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :     2015.8.24
//// Content :           ProtoBuf)
////////////////////////////////////////////////////
using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Runtime.Serialization;

//using ProtoBuf;

namespace UnityMVC.Protocol.Helper
{
    public class SerializeHelper
    {
        public static byte[] Serialize<T>(T data)
        {
            byte[] buffer = null;
//            using (MemoryStream stream = new MemoryStream())
//            {
//                Serializer.Serialize(stream, data);
//                stream.Position = 0; //streamRead
//                int length = (int)stream.Length;
//                buffer = new byte[stream.Length];
//                stream.Read(buffer, 0, length);
//            }
            return buffer;
        }

        public static byte[] Serialize(object data)
        {
            byte[] buffer = null;
//            using (MemoryStream stream = new MemoryStream())
//            {
//                Serializer.Serialize(stream, data);
//                stream.Position = 0; //streamRead
//                int length = (int)stream.Length;
//                buffer = new byte[stream.Length];
//                stream.Read(buffer, 0, length);
//            }
            return buffer;
        }

        public static T Deserialize<T>(byte[] buffer)
        {
//            T data;
//            using (MemoryStream stream = new MemoryStream(buffer))
//            {
//                data = Serializer.Deserialize<T>(stream);
//            }
//            return data;
            return default(T);
        }
    }
}

