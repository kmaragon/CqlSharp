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

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("CqlSharp")]
[assembly: AssemblyDescription("Cassandra Binary Protocol ADO.Net Data Provider")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Joost Reuzel")]
[assembly: AssemblyProduct("CqlSharp")]
[assembly: AssemblyCopyright("Copyright Joost Reuzel ©  2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid("c4c03972-5840-4c9c-b11f-876a69db3338")]

#if DEBUG
[assembly: InternalsVisibleTo("CqlSharp.Fakes")]
[assembly: InternalsVisibleTo("CqlSharp.Test")]
#endif