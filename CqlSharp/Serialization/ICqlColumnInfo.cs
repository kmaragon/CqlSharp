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
using System.Reflection;
using CqlSharp.Annotations;

namespace CqlSharp.Serialization
{
    public interface ICqlColumnInfo<TTable> : ICqlColumnInfo
    {
        /// <summary>
        /// Serializes the column value from the provided source using the given type.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="type">The type.</param>
        /// <param name="protocolVersion"></param>
        /// <returns></returns>
        byte[] SerializeFrom(TTable source, CqlType type, byte protocolVersion);

        /// <summary>
        /// Deserializes the provided data using the given type and assigns it to the column member of the given target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="data">The data.</param>
        /// <param name="type">The type of the data.</param>
        /// <param name="protocolVersion"></param>
        void DeserializeTo(TTable target, byte[] data, CqlType type, byte protocolVersion);

        /// <summary>
        /// Writes a value to the member belonging to this column on the specified target.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        void Write<TValue>(TTable target, TValue value);

        /// <summary>
        /// Reads a value from the member belonging to this column from the specified source.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The target.</param>
        TValue Read<TValue>(TTable source);

        /// <summary>
        /// Copies the value of this column on the source object to the specified column on the
        /// target object.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="column">The column.</param>
        void CopyValue<TTarget>(TTable source, TTarget target, ICqlColumnInfo<TTarget> column);

        /// <summary>
        /// Determines whether the column value of the specified source is equal to
        /// the column value of the comparand
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="comperand">The comperand.</param>
        /// <returns></returns>
        bool IsEqual(TTable source, TTable comperand);
    }

    public interface ICqlColumnInfo
    {
        /// <summary>
        /// Gets the column name.
        /// </summary>
        /// <value> The name. </value>
        string Name { get; }

        /// <summary>
        /// Gets the type of the CQL column.
        /// </summary>
        /// <value>
        /// The type of the CQL column.
        /// </value>
        CqlType CqlType { get; }

        /// <summary>
        /// Gets the .NET type.
        /// </summary>
        /// <value> The type. </value>
        Type Type { get; }

        /// <summary>
        /// Gets the order/index of a key column.
        /// </summary>
        /// <value> The order. </value>
        int? Order { get; }

        /// <summary>
        /// Gets a value indicating whether this column is part of the partition key.
        /// </summary>
        /// <value> <c>true</c> if this column is part of the partition key; otherwise, <c>false</c> . </value>
        bool IsPartitionKey { get; }

        /// <summary>
        /// Gets a value indicating whether this column is part of the clustering key.
        /// </summary>
        /// <value> <c>true</c> if this column is part of the clustering key; otherwise, <c>false</c> . </value>
        bool IsClusteringKey { get; }

        /// <summary>
        /// Gets a value indicating whether this column is indexed.
        /// </summary>
        /// <value> <c>true</c> if this column is indexed; otherwise, <c>false</c> . </value>
        bool IsIndexed { get; }

        /// <summary>
        /// Gets the name of the index (if any).
        /// </summary>
        /// <value> The name of the index. </value>
        string IndexName { get; }

        /// <summary>
        /// Gets the member information.
        /// </summary>
        /// <value> The member information. </value>
        MemberInfo MemberInfo { get; }

        /// <summary>
        /// Writes a value to the member belonging to this column on the specified target.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        void Write<TValue>(object target, TValue value);

        /// <summary>
        /// Reads a value from the member belonging to this column from the specified source.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The object the value is read from</param>
        TValue Read<TValue>(object source);

        /// <summary>
        /// Copies the value of this column on the source object to the specified column on the
        /// target object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="column">The column.</param>
        void CopyValue(object source, object target, ICqlColumnInfo column);

        /// <summary>
        /// Serializes the column value from the provided source using the given type.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="type">The type.</param>
        /// <param name="protocolVersion"></param>
        /// <returns></returns>
        byte[] SerializeFrom(object source, CqlType type, byte protocolVersion);

        /// <summary>
        /// Deserializes the provided data using the given type and assigns it to the column member of the given target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="data">The data.</param>
        /// <param name="type">The type of the data.</param>
        /// <param name="protocolVersion"></param>
        void DeserializeTo(object target, byte[] data, CqlType type, byte protocolVersion);
    }
}