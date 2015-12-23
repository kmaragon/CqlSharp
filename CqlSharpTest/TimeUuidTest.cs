// CqlSharp - CqlSharp.Test
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
using System.Collections.Concurrent;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace CqlSharp.Test
{
    [TestClass]
    public class TimeUuidTest
    {
        [TestMethod]
        public void TimeUuidIssue()
        {
            var baseDate = DateTime.UtcNow;
            var generatedGuids = new Guid[50000];

            Parallel.For(0, generatedGuids.Length, i =>
            {
                generatedGuids[i] = baseDate.AddTicks(i % 3).GenerateTimeBasedGuid();
            });

            var hashOfGuids = new HashSet<Guid>(generatedGuids);
            Assert.AreEqual(generatedGuids.Length, hashOfGuids.Count);
        }

        [TestMethod]
        public void MultiThreadedTimeUuidIssue()
        {
            var baseDate = DateTime.UtcNow;
            var generatedGuids = new Guid[10000];

            byte[] mac = { 0x33, 0x30, 0xa0, 0x80, 0x10, 0x88 };

            Parallel.For(0, generatedGuids.Length, i =>
            {
                generatedGuids[i] = baseDate.AddTicks(i % 10).GenerateTimeBasedGuid(mac);
            });

            var hashOfGuids = new HashSet<Guid>(generatedGuids);
            Assert.AreEqual(generatedGuids.Length, hashOfGuids.Count);
            Assert.IsTrue(generatedGuids.All(g => g.ToString().EndsWith("3330a0801088")));
        }

        [TestMethod]
        public void TimeUuidRoundTrip()
        {
            var time = DateTime.UtcNow;
            var guid = time.GenerateTimeBasedGuid();
            var time2 = guid.GetDateTime();

            //allow a clock drift of 1ms. other unit tests have created a lot of
            //guids, and a single ms drift is therefore allowed here.
            Assert.IsTrue(Math.Abs(time.Ticks - time2.Ticks) <= 1);
        }

        [TestMethod]
        public void ValidateTimeUuidGetDateTime()
        {
            const string timeUuid = "92ea3200-9a80-11e3-9669-0800200c9a66";
            var expected = new DateTime(2014, 2, 20, 22, 44, 36, 256, DateTimeKind.Utc);

            var guid = Guid.Parse(timeUuid);
            var actual = guid.GetDateTime();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidateTimeUuidGeneration()
        {
            const string expected = "92ea3200-9a80-11e3-9669-0800200c9a66";

            var dateTime = new DateTime(2014, 2, 20, 22, 44, 36, 256, DateTimeKind.Utc);
            var time = (dateTime - TimeGuid.GregorianCalendarStart).Ticks;
            var node = new byte[] {0x08, 0x00, 0x20, 0x0c, 0x9a, 0x66};
            const int clockId = 5737;

            var guid = TimeGuid.GenerateTimeBasedGuid(time, clockId, node);
            var actual = guid.ToString("D").ToLower();

            Assert.AreEqual(expected, actual);
        }
    }
}