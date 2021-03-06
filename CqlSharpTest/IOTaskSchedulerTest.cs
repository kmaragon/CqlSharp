﻿// SingleThreadScheduler - SingleThreadScheduler.Test
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using CqlSharp.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CqlSharp.Test
{
    [TestClass]
    public class IOTaskSchedulerTest
    {
        public static int ThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        [TestMethod]
        public void Yield()
        {
            SyncContextHelper.Invoke(async () =>
            {
                await Scheduler.RunOnIOThread(async () =>
                {
                    Assert.IsNull(SynchronizationContext.Current);
                    Assert.IsInstanceOfType(TaskScheduler.Current, typeof(IOTaskScheduler));

                    await Task.Yield();

                    Assert.IsNull(SynchronizationContext.Current);
                    Assert.IsInstanceOfType(TaskScheduler.Current, typeof(IOTaskScheduler));
                });

                Assert.IsInstanceOfType(SynchronizationContext.Current, typeof(DispatcherSynchronizationContext));
            });

        }


        [TestMethod]
        public void CompletedTask()
        {
            SyncContextHelper.Invoke(async () =>
            {
                await Scheduler.RunOnIOThread(async () =>
                {
                    Assert.IsNull(SynchronizationContext.Current);
                    Assert.IsInstanceOfType(TaskScheduler.Current, typeof(IOTaskScheduler));

                    await Task.FromResult(true).AutoConfigureAwait();

                    Assert.IsNull(SynchronizationContext.Current);
                    Assert.IsInstanceOfType(TaskScheduler.Current, typeof(IOTaskScheduler));
                });

                Assert.IsInstanceOfType(SynchronizationContext.Current, typeof(DispatcherSynchronizationContext));
            });
        }

        [TestMethod]
        public void Delay()
        {
            SyncContextHelper.Invoke(async () =>
            {
                await Scheduler.RunOnIOThread(async () =>
                {
                    Assert.IsNull(SynchronizationContext.Current);
                    Assert.IsInstanceOfType(TaskScheduler.Current, typeof(IOTaskScheduler));

                    await Task.Delay(10).AutoConfigureAwait();

                    Assert.IsNull(SynchronizationContext.Current);
                    Assert.IsInstanceOfType(TaskScheduler.Current, typeof(IOTaskScheduler));
                });

                Assert.IsInstanceOfType(SynchronizationContext.Current, typeof(DispatcherSynchronizationContext));
            });
        }

        [TestMethod]
        public void DelayIndirect()
        {
            SyncContextHelper.Invoke(async () =>
            {
                await Scheduler.RunOnIOThread(async () =>
                {
                    Assert.IsNull(SynchronizationContext.Current);
                    Assert.IsInstanceOfType(TaskScheduler.Current, typeof(IOTaskScheduler));

                    await DummyWork().AutoConfigureAwait();

                    Assert.IsNull(SynchronizationContext.Current);
                    Assert.IsInstanceOfType(TaskScheduler.Current, typeof(IOTaskScheduler));
                });

                Assert.IsInstanceOfType(SynchronizationContext.Current, typeof(DispatcherSynchronizationContext));
            });
        }
        

        [TestMethod]
        public void DelayWithResult()
        {
             SyncContextHelper.Invoke(async () =>
                {
                    const int value = 100;
                    int actual = await Scheduler.RunOnIOThread(async () =>
                    {
                        Assert.IsNull(SynchronizationContext.Current);
                        Assert.IsInstanceOfType(TaskScheduler.Current, typeof(IOTaskScheduler));

                        await Task.Delay(10).AutoConfigureAwait();

                        Assert.IsNull(SynchronizationContext.Current);
                        Assert.IsInstanceOfType(TaskScheduler.Current, typeof(IOTaskScheduler));

                        return value;
                    });

                    Assert.AreEqual(value, actual);

                    Assert.IsInstanceOfType(SynchronizationContext.Current, typeof(DispatcherSynchronizationContext));
                });
            
        }

        [TestMethod]
        public async Task DelayThenException()
        {
            try
            {
                await Scheduler.RunOnIOThread(async () =>
                {
                    await Task.Delay(10).AutoConfigureAwait();
                    throw new Exception("Yikes");
                });

            }
            catch(Exception ex)
            {
                Assert.AreEqual("Yikes", ex.Message);
                return;
            }

            Assert.Fail("Exception expected");
        }

        public async Task DummyWork()
        {
            await Task.Delay(10).AutoConfigureAwait();
        }
    }
}