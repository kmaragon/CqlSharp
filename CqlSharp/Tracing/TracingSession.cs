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
using System.Net;
using CqlSharp.Serialization;

namespace CqlSharp.Tracing
{
    [CqlTable("sessions", Keyspace = "system_traces")]
    public class TracingSession
    {
        [CqlColumn("session_id")]
        public Guid SessionId { get; set; }

        [CqlColumn("coordinator")]
        public IPAddress Coordinator { get; set; }

        [CqlColumn("duration")]
        public int Duration { get; set; }

        [CqlColumn("parameters")]
        public Dictionary<string, string> Parameters { get; set; }

        [CqlColumn("request")]
        public string Request { get; set; }

        [CqlColumn("started_at")]
        public DateTime StartedAt { get; set; }

        [CqlIgnore]
        public List<TracingEvent> Events { get; set; }
    }
}