using System.IO;
using System.Text;
using System;

namespace SIP.Net
{
    public class ByteBuffer
    {

        MemoryStream m_stream = null;
        BinaryWriter m_writer = null;
        BinaryReader m_reader = null;

        public ByteBuffer()
        {
            m_stream = new MemoryStream();
            m_writer = new BinaryWriter(m_stream);
        }

        public ByteBuffer(byte[] data)
        {
            if(data != null)
            {
                m_stream = new MemoryStream(data);
                m_reader = new BinaryReader(m_stream);
            }
            else
            {
                m_stream = new MemoryStream();
                m_writer = new BinaryWriter(m_stream);
            }
        }

        public void Close()
        {
            if(m_writer != null)
            {
                m_writer.Close();
            }
            if(m_reader != null)
            {
                m_reader.Close();
            }

            m_stream.Close();
            m_writer = null;
            m_reader = null;
            m_stream = null;
        }

        public void WriteByte(byte v)
        {
            m_writer.Write(v);
        }

        public void WriteInt(int v)
        {
            m_writer.Write((int)v);
        }

        public void WriteShort(ushort v)
        {
            m_writer.Write((ushort)v);
        }

        public void WriteLong(long v)
        {
            m_writer.Write((long)v);
        }

        public void WriteFloat(float v)
        {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            m_writer.Write(BitConverter.ToString(temp, 0));
        }

        public void WriteDouble(double v)
        {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            m_writer.Write(BitConverter.ToDouble(temp, 0));
        }

        public void WriteString(string v)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(v);
            m_writer.Write((ushort)bytes.Length);
            m_writer.Write(bytes);
        }

        public void WriteBytes(byte[] v)
        {
            m_writer.Write((int)v.Length);
            m_writer.Write(v);
        }

        public byte ReadByte()
        {
            return m_reader.ReadByte();
        }

        public int ReadInt()
        {
            return (int)m_reader.ReadInt32();
        }

        public ushort ReadShort()
        {
            return (ushort)m_reader.ReadInt16();
        }

        public long ReadLong()
        {
            return (long)m_reader.ReadInt64();
        }

        public float ReadFloat()
        {
            byte[] temp = BitConverter.GetBytes(m_reader.ReadSingle());
            Array.Reverse(temp);
            return BitConverter.ToSingle(temp, 0);
        }

        public double ReadDouble()
        {
            byte[] temp = BitConverter.GetBytes(m_reader.ReadDouble());
            Array.Reverse(temp);
            return BitConverter.ToDouble(temp, 0);
        }

        public string ReadString()
        {
            ushort len = ReadShort();
            byte[] buffer = new byte[len];
            buffer = m_reader.ReadBytes(len);
            return Encoding.UTF8.GetString(buffer);
        }

        public byte[] ReadBytes()
        {
            int len = ReadInt();
            return m_reader.ReadBytes(len);
        }

        public byte[] ToBytes()
        {
            m_writer.Flush();
            return m_stream.ToArray();
        }

        public void Flush()
        {
            m_writer.Flush();
        }
    }
}

