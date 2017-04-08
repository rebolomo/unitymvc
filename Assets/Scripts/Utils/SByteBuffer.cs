////////////////////////////////////////////////////
//// File Name :        SByteBuffer.cs
//// Tables :              nothing
//// Autor :               kid
//// Create Date :    	2015.8.24
//// Content :           ���ݶ�ȡ����
////////////////////////////////////////////////////
namespace UnityMVC.Utils
{
    using System;
    using UnityEngine;
    using System.IO;
    using System.Text;
    using System.Collections;

    public class SByteBuffer
    {
        MemoryStream stream = null;
        BinaryWriter writer = null;
        BinaryReader reader = null;

        public SByteBuffer()
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }

        public SByteBuffer(byte[] data)
        {
            if (data != null)
            {
                stream = new MemoryStream(data);
                reader = new BinaryReader(stream);
            }
            else
            {
                stream = new MemoryStream();
                writer = new BinaryWriter(stream);
            }
        }

        public void Close()
        {
            if (writer != null) writer.Close();
            if (reader != null) reader.Close();

            stream.Close();
            writer = null;
            reader = null;
            stream = null;
        }

        public void WriteByte(byte v)
        {
            writer.Write(v);
        }

        public void WriteInt(int v)
        {
            writer.Write(v);
        }

        public void WriteShort(ushort v)
        {
            writer.Write(v);
        }

        public void WriteLong(long v)
        {
            writer.Write(v);
        }

        public void WriteFloat(float v)
        {
            writer.Write(v);
        }
        public void WriteBool(bool v)
        {
            writer.Write(v);
        }
        public void WriteVector3(Vector3 v)
        {
            WriteFloat(v.x);
            WriteFloat(v.y);
            WriteFloat(v.z);
        }
        public void WriteVector3_2(Vector3? v)
        {
            if (v != null)
            {
                Vector3 vv = (Vector3)v;
                WriteFloat(vv.x);
                WriteFloat(vv.y);
                WriteFloat(vv.z);
            }
        }
        public void WriteDouble(double v)
        {
            writer.Write(v);
        }

        public void WriteString(string v)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(v);
            writer.Write((ushort)bytes.Length);
            writer.Write(bytes);
        }

        public void WriteBytes(byte[] v)
        {
            writer.Write((ushort)v.Length);
            writer.Write(v);
        }
        public void Write(Type tp, object obj)
        {
            if (tp == null)
                return;
            if (tp == typeof(byte))
            {
                WriteByte((byte)obj);
            }
            else if (tp == typeof(int))
            {
                WriteInt((int)obj);
            }
            else if (tp == typeof(ushort) || tp == typeof(short))
            {
                WriteShort((ushort)obj);
            }
            else if (tp == typeof(long) || tp == typeof(Int64))
            {
                WriteLong((long)obj);
            }
            else if (tp == typeof(float))
            {
                WriteFloat((float)obj);
            }
            else if (tp == typeof(string))
            {
                WriteString((string)obj);
            }
            else if (tp == typeof(double))
            {
                WriteDouble((double)obj);
            }
            else if (tp == typeof(Vector3))
            {
                WriteVector3((Vector3)obj);
            }
            else if (tp == typeof(Vector3?))
            {
                WriteVector3_2((Vector3?)obj);
            }
            else if (tp == typeof(bool))
            {
                WriteBool((bool)obj);
            }
            else
            {
                if (tp != null)
                {
                    throw new Exception("unknow type write:" + tp.Name);
                }
                else
                {
                    throw new Exception("unknow type write: tp is null !!");
                }
            }
        }
        public object Read(Type tp)
        {
            if (tp == typeof(byte))
            {
                return ReadByte();
            }
            else if (tp == typeof(int))
            {
                return ReadInt();
            }
            else if (tp == typeof(ushort) || tp == typeof(short))
            {
                return ReadShort();
            }
            else if (tp == typeof(long) || tp == typeof(Int64))
            {
                return ReadInt64();
            }
            else if (tp == typeof(float))
            {
                return ReadFloat();
            }
            else if (tp == typeof(double))
            {
                return ReadDouble();
            }
            else if (tp == typeof(string))
            {
                return ReadString();
            }
            else if (tp == typeof(Vector3))
            {
                return ReadVector3();
            }
            return null;
        }
        public byte ReadByte()
        {
            return reader.ReadByte();
        }

        public int ReadInt()
        {
            return reader.ReadInt32();
        }

        public ushort ReadShort()
        {
            return (ushort)reader.ReadInt16();
        }

        public Int64 ReadInt64()
        {
            return reader.ReadInt64();
        }

        public byte[] ReadBytes()
        {
            ushort len = ReadShort();
            byte[] buffer = new byte[len];
            return reader.ReadBytes(len);
        }

        public float ReadFloat()
        {
            return reader.ReadSingle();
        }
        public Vector3 ReadVector3()
        {
            float x = ReadFloat();
            float y = ReadFloat();
            float z = ReadFloat();
            Vector3 v = new Vector3(x, y, z);
            return v;
        }

        public double ReadDouble()
        {
            return reader.ReadDouble();
        }

        public string ReadString()
        {
            ushort len = ReadShort();
            byte[] buffer = new byte[len];
            buffer = reader.ReadBytes(len);
            return Encoding.UTF8.GetString(buffer);
        }

        public byte[] ToBytes()
        {
            writer.Flush();
            return stream.ToArray();
        }

        public byte[] ToCompressionBytes()
        {
            byte[] result = ToBytes();
            return result;
        }

        public void Flush()
        {
            writer.Flush();
        }
    }
}
