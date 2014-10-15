CqlSharp
========

CqlSharp is a high performance, asynchronous ADO.NET data provider for [Cassandra](http://cassandra.apache.org/) implementing the CQL binary protocol.

Please see the [Wiki](https://github.com/reuzel/CqlSharp/wiki) for more extensive [documentation](https://github.com/reuzel/CqlSharp/wiki).

Installation
------------

The latest release of CqlSharp can be found as a [package on NuGet](http://nuget.org/packages/CqlSharp/). Please install from there.

If you would like to have Linq support, please have a look at [CqlSharp.Linq](https://github.com/reuzel/CqlSharp.Linq), which can be installed via the [CqlSharp.Linq NuGet package](http://nuget.org/packages/CqlSharp.Linq/).

Features
--------

* The API implements the ADO.NET interfaces. If you are familiar with SqlConnection, SqlCommand, and SqlReader, you should be able to use CqlSharp with no difficulty.
* CqlSharp is an implementation of the (new) CQL Binary Protocol and therefore requires Cassandra 1.2 and up
* CqlSharp implements version 1, 2 and 3 of the cql binary protocol. Protocol versions are automatically negotiated with the servers. A mix of versions towards the same Cassandra cluster is supported. 
* CqlSharp supports all the binary protocol v2 features: batching, paging, bound query variables, result schema caching, check-and-set (CAS) statements, and sasl-authentication
* CqlSharp also supports the binary protocol v3 features: setting default writetime, CAS consistency on batches, User Defined Types, Tuples, and the increased number of queries per connection.
* Simple use of User Defined Types. Either use the dynamic keyword, or define your own classes and they will be mapped automatically to the right UDT fields by name. Nesting of UDT types is supported.
* Supports simple mapping of objects to query parameters, or query results to objects. Mapping is tunable by decorating your classes via Table and Column attributes.
* Extremely flexible type conversion system: data will be converted automatically to the requested type (e.g. int to string, string to long, long to datetime, etc.)
* CqlSharp allows for partition/token aware routing of queries. This allows queries to be directly sent to the Cassandra nodes that are the 'owner' of that data.
* Query timeouts and cancellation is supported
* Query tracing is supported.
* CqlSharp supports Snappy compression of queries and responses
* CqlSharp is 100% written in C#, and requires .NET 4.5. It has no dependencies on other packages or libraries.
* Configuration is done through connection strings. The simultaneous use of multiple Cassandra clusters is supported.
* Most behavioral aspects of the CqlSharp are configurable: max number of connections, new connection threshold, discovery scope, max connection idle time, etc. etc.
* Relative Node Discovery: given the 'seed' nodes in your connection string, CqlSharp may find other nodes for you: all nodes in your cluster, nodes in the same data center, or the nodes in the same rack
* Load balanced connection management: you can give your queries a load 'factor' and the client will take that into account when picking connections to send queries over.
* Queries will be automatically retried when connections or nodes fail.
* Node monitoring: Node failure is automatically detected. Recovery checks occur using an exponential back-off algorithm
* CqlSharp listens to Cassandra events for node up, new node and node removed messages such that Cluster changes are automatically incorporated.


Some non-functionals
--------------------

* Serialization and object mapping is extremely fast through dynamically compiled code.
* Rows returned from select queries are read via a pull-model, allowing for large result sets to be processed efficiently. Query results may be buffered in memory as well.
* All I/O is done asynchronously using the wonderful TPL and .NET 4.5 async/await support. As a result, the client should be quite efficient in terms of threading. This makes it especially suitable in server environments where all threads are necessary to handle incoming requests.
* Buffering, MemoryPools and Task result caching reduce GC activity, especially important when doing high volumes of queries in a short time frame
* CqlSharp uses multiple connections in parallel to execute queries faster. Queries are also multiplexed on single connections, leading to efficient connection usage.


License
-------

CqlSharp and CqlSharp.Linq are both available under the [Apache License version 2.0](http://www.apache.org/licenses/LICENSE-2.0).

Wish list
---------

* ~~Even better performance, e.g. by creating using more sophisticated memory pools that support multiple buffer sizes.~~
* ~~Linq2Cql~~ 
* Alternative retry models (e.g. retry with reduced consistency)

