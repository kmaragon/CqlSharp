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

namespace CqlSharp
{
    /// <summary>
    /// Represents a result of a Cql ResultMetaData changing query (create/alter/drop/etc.)
    /// </summary>
    public class CqlSchemaChange : ICqlQueryResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CqlSchemaChange" /> class.
        /// </summary>
        /// <param name="keyspace">The keyspace.</param>
        /// <param name="table">The table.</param>
        /// <param name="change">The change.</param>
        /// <param name="tracingId">The tracing unique identifier.</param>
        internal CqlSchemaChange(string keyspace, string table, string change, Guid? tracingId)
        {
            Keyspace = keyspace;
            Table = table;
            Change = change;
            TracingId = tracingId;
        }

        /// <summary>
        /// Gets the keyspace.
        /// </summary>
        /// <value> The keyspace. </value>
        public string Keyspace { get; private set; }

        /// <summary>
        /// Gets the table impacted.
        /// </summary>
        /// <value> The table. </value>
        public string Table { get; private set; }

        /// <summary>
        /// Gets the type of change.
        /// </summary>
        /// <value> The change. </value>
        public string Change { get; private set; }

        #region ICqlQueryResult Members

        /// <summary>
        /// Gets the type of the result.
        /// </summary>
        /// <value> The type of the result. </value>
        public CqlResultType ResultType
        {
            get { return CqlResultType.SchemaChange; }
        }

        /// <summary>
        /// Gets the tracing id.
        /// </summary>
        /// <value> The tracing id. </value>
        public Guid? TracingId { get; private set; }

        #endregion
    }
}