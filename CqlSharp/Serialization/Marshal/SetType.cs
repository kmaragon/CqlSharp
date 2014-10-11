// CqlSharp - CqlSharp
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
using System.Data;
using System.IO;
using System.Text;
using CqlSharp.Protocol;

namespace CqlSharp.Serialization.Marshal
{
    public class SetType<T> : CqlType<HashSet<T>>
    {
        private readonly CqlType<T> _valueType;

        public SetType(CqlType valueType)
        {
            _valueType = (CqlType<T>)valueType;
        }

        public CqlType ValueType
        {
            get { return _valueType; }
        }

        public override CqlTypeCode CqlTypeCode
        {
            get { return CqlTypeCode.Set; }
        }

        public override void AppendTypeName(StringBuilder builder)
        {
            builder.Append("org.apache.cassandra.db.marshal.SetType(");
            _valueType.AppendTypeName(builder);
            builder.Append(")");
        }

        public override Type Type
        {
            get { return typeof(List<T>); }
        }

        public override DbType ToDbType()
        {
            return DbType.Object;
        }

        /// <summary>
        /// Gets the maximum size in bytes of values of this type.
        /// </summary>
        /// <value>
        /// The maximum size in bytes.
        /// </value>
        public override int Size
        {
            get { return 2000000000; }
        }

        public override byte[] Serialize(HashSet<T> value, byte protocolVersion)
        {
            if(protocolVersion <= 2)
            {
                using(var ms = new MemoryStream())
                {
                    //write length placeholder
                    ms.Position = 2;
                    ushort count = 0;
                    foreach(T elem in value)
                    {
                        byte[] rawDataElem = _valueType.Serialize(elem, protocolVersion);
                        ms.WriteShortByteArray(rawDataElem);
                        count++;
                    }
                    ms.Position = 0;
                    ms.WriteShort(count);
                    return ms.ToArray();
                }
            }
            using(var ms = new MemoryStream())
            {
                //write length placeholder
                ms.Position = 4;
                int count = 0;
                foreach(T elem in value)
                {
                    byte[] rawDataElem = _valueType.Serialize(elem, protocolVersion);
                    ms.WriteByteArray(rawDataElem);
                    count++;
                }
                ms.Position = 0;
                ms.WriteInt(count);
                return ms.ToArray();
            }
        }

        public override HashSet<T> Deserialize(byte[] data, byte protocolVersion)
        {
            if(protocolVersion <= 2)
            {
                using(var ms = new MemoryStream(data))
                {
                    ushort nbElem = ms.ReadShort();
                    var set = new HashSet<T>();
                    for(int i = 0; i < nbElem; i++)
                    {
                        byte[] elemRawData = ms.ReadShortByteArray();
                        T elem = _valueType.Deserialize(elemRawData, protocolVersion);
                        set.Add(elem);
                    }
                    return set;
                }
            }
            using(var ms = new MemoryStream(data))
            {
                int nbElem = ms.ReadInt();
                var set = new HashSet<T>();
                for(int i = 0; i < nbElem; i++)
                {
                    byte[] elemRawData = ms.ReadByteArray();
                    T elem = _valueType.Deserialize(elemRawData, protocolVersion);
                    set.Add(elem);
                }
                return set;
            }
        }

        public override bool Equals(CqlType other)
        {
            var setType = other as SetType<T>;
            return setType != null && setType._valueType.Equals(_valueType);
        }

        public override string ToString()
        {
            return string.Format("set<{0}>", _valueType);
        }
    }
}