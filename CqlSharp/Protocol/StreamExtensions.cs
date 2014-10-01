﻿// CqlSharp - CqlSharp
// Copyright (c) 2014 Joost Reuzel
//   
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
// http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using CqlSharp.Memory;

namespace CqlSharp.Protocol
{
    /// <summary>
    /// Extensions to the Stream class to read and write primitive values as used in the Binary Protocol
    /// </summary>
    internal static class StreamExtensions
    {
        /// <summary>
        /// Writes a short.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="data"> The data. </param>
        public static void WriteShort(this Stream stream, ushort data)
        {
            //byte[] buffer = BitConverter.GetBytes(data);
            //if (BitConverter.IsLittleEndian) Array.Reverse(buffer);
            //stream.Write(buffer, 0, buffer.Length);

            stream.WriteByte((byte)(data >> 8));
            stream.WriteByte((byte)(data));
        }

        /// <summary>
        /// Writes an int.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="data"> The data. </param>
        public static void WriteInt(this Stream stream, int data)
        {
            //byte[] buffer = BitConverter.GetBytes(data);
            //if (BitConverter.IsLittleEndian) Array.Reverse(buffer);
            //stream.Write(buffer, 0, buffer.Length);


            stream.WriteByte((byte)(data >> 24));
            stream.WriteByte((byte)(data >> 16));
            stream.WriteByte((byte)(data >> 8));
            stream.WriteByte((byte)(data));
        }

        /// <summary>
        /// Writes an long.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="data"> The data. </param>
        public static void WriteLong(this Stream stream, long data)
        {
            //byte[] buffer = BitConverter.GetBytes(data);
            //if (BitConverter.IsLittleEndian) Array.Reverse(buffer);
            //stream.Write(buffer, 0, buffer.Length);

            stream.WriteByte((byte)(data >> 56));
            stream.WriteByte((byte)(data >> 48)); 
            stream.WriteByte((byte)(data >> 40));
            stream.WriteByte((byte)(data >> 32));
            stream.WriteByte((byte)(data >> 24));
            stream.WriteByte((byte)(data >> 16));
            stream.WriteByte((byte)(data >> 8));
            stream.WriteByte((byte)(data));
        }

        /// <summary>
        /// Writes a string.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="data"> The data. </param>
        public static void WriteString(this Stream stream, string data)
        {
            //byte[] bufStr = Encoding.UTF8.GetBytes(data);
            //var len = (short)bufStr.Length;
            //stream.WriteShort(len);
            //stream.Write(bufStr, 0, len);

            int len = Encoding.UTF8.GetByteCount(data);
            stream.WriteShort((ushort)len);

            byte[] bufStr = MemoryPool.Instance.Take(len);
            Encoding.UTF8.GetBytes(data, 0, data.Length, bufStr, 0);
            stream.Write(bufStr, 0, len);
            MemoryPool.Instance.Return(bufStr);
        }

        /// <summary>
        /// Writes a list of strings.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="data"> The data. </param>
        public static void WriteStringList(this Stream stream, IList<string> data)
        {
            stream.WriteShort((ushort)data.Count);
            foreach(string s in data)
                stream.WriteString(s);
        }

        /// <summary>
        /// Writes a long string.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="data"> The data. </param>
        public static void WriteLongString(this Stream stream, string data)
        {
            //byte[] bufStr = Encoding.UTF8.GetBytes(data);
            //int len = bufStr.Length;
            //stream.WriteInt(len);
            //stream.Write(bufStr, 0, len);

            int len = Encoding.UTF8.GetByteCount(data);
            stream.WriteInt(len);

            byte[] bufStr = MemoryPool.Instance.Take(len);
            Encoding.UTF8.GetBytes(data, 0, data.Length, bufStr, 0);
            stream.Write(bufStr, 0, len);
            MemoryPool.Instance.Return(bufStr);
        }

        /// <summary>
        /// Writes a string map.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="dic"> The dic. </param>
        public static void WriteStringMap(this Stream stream, IDictionary<string, string> dic)
        {
            stream.WriteShort((ushort)dic.Count);
            foreach(var kvp in dic)
            {
                stream.WriteString(kvp.Key);
                stream.WriteString(kvp.Value);
            }
        }

        /// <summary>
        /// Writes a short byte array.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="data"> The data. </param>
        public static void WriteShortByteArray(this Stream stream, byte[] data)
        {
            if(data == null)
                stream.WriteShort(0);
            else
            {
                var len = (ushort)data.Length;
                stream.WriteShort(len);
                stream.Write(data, 0, len);
            }
        }

        /// <summary>
        /// Writes a byte array.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="data"> The data. </param>
        public static void WriteByteArray(this Stream stream, byte[] data)
        {
            if(data == null)
                stream.WriteInt(-1);
            else
            {
                int len = data.Length;
                stream.WriteInt(len);
                stream.Write(data, 0, len);
            }
        }

        /// <summary>
        /// Writes an IP address and port
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="endpoint"> The endpoint. </param>
        public static void WriteInet(this Stream stream, IPEndPoint endpoint)
        {
            byte[] ip = endpoint.Address.GetAddressBytes();
            stream.Write(new[] {(byte)ip.Length}, 0, 1);
            stream.Write(ip, 0, ip.Length);
            stream.WriteInt(endpoint.Port);
        }

        /// <summary>
        /// Writes a UUID/GUID.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="guid"> The GUID. </param>
        public static void WriteUuid(this Stream stream, Guid guid)
        {
            byte[] buffer = guid.ToByteArray();
            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer, 0, 4);
                Array.Reverse(buffer, 4, 2);
                Array.Reverse(buffer, 6, 2);
            }
            stream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Writes the consistency.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="consistency"> The consistency. </param>
        public static void WriteConsistency(this Stream stream, CqlConsistency consistency)
        {
            stream.WriteShort((ushort)consistency);
        }

        /// <summary>
        /// Reads the buffer.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="buffer"> The buffer. </param>
        private static void ReadBuffer(this Stream stream, byte[] buffer)
        {
            ReadBuffer(stream, buffer, buffer.Length);
        }

        /// <summary>
        /// Reads the buffer up to a specified length
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="buffer"> The buffer. </param>
        /// <param name="len"> The len. </param>
        /// <exception cref="System.IO.IOException">Unexpected end of stream reached.</exception>
        private static void ReadBuffer(this Stream stream, byte[] buffer, int len)
        {
            int read = 0;
            while(read != len)
            {
                int actual = stream.Read(buffer, read, len - read);
                if(actual == 0)
                    throw new IOException("Unexpected end of stream reached.");

                read += actual;
            }
        }

        /// <summary>
        /// Reads a short.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <returns> </returns>
        /// <exception cref="System.IO.IOException">Unexpected end of stream reached</exception>
        public static ushort ReadShort(this Stream stream)
        {
            int value = 0;
            for(int i = 0; i < 2; i++)
            {
                int read = stream.ReadByte();
                if(read < 0)
                    throw new IOException("Unexpected end of stream reached");

                value = (value << 8) + read;
            }
            return (ushort)value;
        }

        /// <summary>
        /// Reads an int.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <returns> </returns>
        /// <exception cref="System.IO.IOException">Unexpected end of stream reached</exception>
        public static int ReadInt(this Stream stream)
        {
            int value = 0;
            for(int i = 0; i < 4; i++)
            {
                int read = stream.ReadByte();
                if(read < 0)
                    throw new IOException("Unexpected end of stream reached");

                value = (value << 8) + read;
            }
            return value;
        }

        /// <summary>
        /// Reads a string.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <returns> </returns>
        public static string ReadString(this Stream stream)
        {
            ushort len = stream.ReadShort();
            if(0 == len)
                return string.Empty;

            byte[] bufStr = MemoryPool.Instance.Take(len);
            stream.ReadBuffer(bufStr, len);
            string data = Encoding.UTF8.GetString(bufStr, 0, len);
            MemoryPool.Instance.Return(bufStr);

            return data;
        }

        /// <summary>
        /// Reads a byte array.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <returns> </returns>
        public static byte[] ReadByteArray(this Stream stream)
        {
            int len = stream.ReadInt();
            if(-1 == len)
                return null;

            var data = new byte[len];
            stream.ReadBuffer(data);
            return data;
        }

        /// <summary>
        /// Reads a short byte array.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <returns> </returns>
        public static byte[] ReadShortByteArray(this Stream stream)
        {
            ushort len = stream.ReadShort();
            var data = new byte[len];
            stream.ReadBuffer(data);
            return data;
        }

        /// <summary>
        /// Reads a list of strings
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <returns> </returns>
        public static string[] ReadStringList(this Stream stream)
        {
            ushort len = stream.ReadShort();
            var data = new string[len];
            for(int i = 0; i < len; ++i)
            {
                data[i] = stream.ReadString();
            }
            return data;
        }

        /// <summary>
        /// Reads a string multimap.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <returns> </returns>
        public static Dictionary<string, string[]> ReadStringMultimap(this Stream stream)
        {
            ushort len = stream.ReadShort();
            var data = new Dictionary<string, string[]>(len);
            for(int i = 0; i < len; ++i)
            {
                string key = stream.ReadString();
                string[] value = stream.ReadStringList();
                data.Add(key, value);
            }

            return data;
        }

        /// <summary>
        /// Reads an IP-Address and port
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <returns> </returns>
        /// <exception cref="System.IO.IOException">Unexpected end of stream</exception>
        public static IPEndPoint ReadInet(this Stream stream)
        {
            int length = stream.ReadByte();
            if(length < 0)
                throw new IOException("Unexpected end of stream");

            var address = new byte[length];
            stream.ReadBuffer(address);

            int port = stream.ReadInt();
            var ipAddress = new IPAddress(address);
            var endpoint = new IPEndPoint(ipAddress, port);

            return endpoint;
        }

        /// <summary>
        /// Reads a UUID/GUID.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <returns> </returns>
        public static Guid ReadUuid(this Stream stream)
        {
            var buffer = new byte[16];
            stream.ReadBuffer(buffer);

            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer, 0, 4);
                Array.Reverse(buffer, 4, 2);
                Array.Reverse(buffer, 6, 2);
            }

            return new Guid(buffer);
        }
    }
}