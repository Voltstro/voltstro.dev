---
layout: post
title:  "Introducing VoltRpc"
date:   2021-08-04 00:05:00 +1000
---

Hello, it’s me again, making my once a semester blog post. This blog post is going to be about my new .NET library, [VoltRpc](https://github.com/Voltstro-Studios/VoltRpc).

So, wtf is VoltRpc. Well as the name implies it is an RPC library, allowing a client to call a method on a server.

But why build another RPC library? Well for a few reasons, but the main ones include:

* Its needs to be fast
* Have minimal dependencies (Including stuff like ASP.NET)
*  .NET trimming support

## **Fast Boi**

One of the main issues that I had with many .NET RPC libraries is that many of them are slow. For what I am using them for the ideal speed would be few MS. But as we can see from the benchmarks, it's even faster than a few MS.

![VoltRpc Pipes Benchmark](/assets/blog/2021/08/04/introducing-voltrpc/PipesBenchmark.png)

``` ini
BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1110 (21H1/May2021Update)
Intel Core i5-10600KF CPU 4.10GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.302
  [Host]     : .NET 5.0.8 (5.0.821.31504), X64 RyuJIT
  Job-GCFSJA : .NET 5.0.8 (5.0.821.31504), X64 RyuJIT

Jit=Default  Platform=AnyCpu
```

|               Method | array         |      message |     Mean |     Error |    StdDev |
|--------------------- |-------------- |------------- |---------:|----------:|----------:|
|            BasicVoid |     ?         |            ? | 6.821 μs | 0.0889 μs | 0.0832 μs |
|          BasicReturn |     ?         |            ? | 6.883 μs | 0.1264 μs | 0.1120 μs |
|   BasicParameterVoid |     ?         | Hello World! | 7.088 μs | 0.0485 μs | 0.0430 μs |
| BasicParameterReturn |     ?         | Hello World! | 7.506 μs | 0.1465 μs | 0.2407 μs |
|          ArrayReturn |     ?         |            ? | 7.024 μs | 0.0449 μs | 0.0420 μs |
|   ArrayParameterVoid |Byte[50]       |            ? | 6.952 μs | 0.0576 μs | 0.0481 μs |
|   ArrayParameterVoid |Byte[8,294,400]|            ? | 6.926 μs | 0.0649 μs | 0.0607 μs |
| ArrayParameterReturn |Byte[50]       |            ? | 7.237 μs | 0.0605 μs | 0.0536 μs |
| ArrayParameterReturn |Byte[8,294,400]|            ? | 7.176 μs | 0.1252 μs | 0.1110 μs |

### Minimal Dependencies

Another thing I wanted was to have minimal dependencies. Many RPC libraries assume that you are going to use ASP.NET (which is fair enough, that is where it is mainly used), and as such, they have a hard dependency on ASP.NET.

I don’t want to set up my project as an ASP.NET Core project and distribute it like that, because that would mean a slower start-up speed as well as hundreds of different DLLs to include in a final process just to provide support to another process.


## .NET Trimming Support

This is a big one for me. Many of the existing libraries use a dynamic generator for its proxy. While this is convenient for the end developer, it is annoying the moment you need to do anything with trimming or AOT.

The .NET trimmer relies on statically analysing code, as well as a few attributes to mark something is being used dynamically. However, I always found this was a piece of shit to do, to go around marking attributes on everything, and half the time it still didn’t even work, on top of that this still doesn’t fix AOT.

One of the solution to get around both of these issues is to just do it at compile time, and with the introduction of [.NET Source generators](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/) last year, it is now easier to do. Generating code at compile time not only fixes trimming (since the analyser can now see the final outputted code), but as well AOT.

So VoltRpc also has a .NET Source generator to generate all the proxy code.

I will say one thing about the VoltRpc generator, this is the first time I’ve ever programmed using Roslyn (Where .NET Source generators are executed), so there is probably shittness in the code. If you gotta problem with any of the code, please just open an issue explaining why or if you really feel nice submit a PR.

## Getting Started

### Install

Using VoltRpc is easy. You can install it as a [NuGet package](https://nuget.org/packages/VoltRpc).

You can use the command below to install it:

```powershell
Package-Install VoltRpc
```

## Example

### Shared

```csharp
using VoltRpc.Proxy;

namespace VoltRpcExample.Shared
{
    [GenerateProxy(GeneratedName = "TestProxy")]
    public interface ITest
    {
        public void Basic();

        public string Hello();
    }
}
```

## Client

```csharp
using System;
using System.Net;
using VoltRpc.Communication.TCP;
using VoltRpc.Proxy.Generated;
using VoltRpcExample.Shared;

namespace VoltRpcExample.Client
{
    public class Program
    {
        public static void Main()
        {
            VoltRpc.Communication.Client client = new TCPClient(new IPEndPoint(IPAddress.Loopback, 7767));

            //While a lot of other libraries don't require to define an interface this way, we do for caching reasons.
            client.AddService<ITest>();
            client.Connect();

            ITest testProxy = new TestProxy(client);
            testProxy.Basic();
            Console.WriteLine($"Got from server: {testProxy.Hello()}");

            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();

            client.Dispose();
        }
    }
}
```

## Server

```csharp
using System;
using System.Net;
using VoltRpc.Communication;
using VoltRpc.Communication.TCP;
using VoltRpcExample.Shared;

namespace VoltRpcExample
{
    public class Program
    {
        public static void Main()
        {
            Host host = new TCPHost(new IPEndPoint(IPAddress.Loopback, 7767));
            host.AddService<ITest>(new TestImpl());
            host.StartListening();

            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            host.Dispose();
        }

        public class TestImpl : ITest
        {
            public void Basic()
            {
                Console.WriteLine("Hello!");
            }

            public string Hello()
            {
                return "Hello World!";
            }
        }
    }
}
```

## Project using VoltRpc

VoltRpc was built by me to use in another project, and that project being [UnityWebBrowser](https://github.com/Voltstro-Studios/UnityWebBrowser).

A lot of work has been done to my Unity Web Browser and one of these changes was to use a RPC library in favour of ZMQ.

You can use that project for a more in-depth usage example.

## Links

* [NuGet](https://nuget.org/packages/VoltRpc)
* [GitHub](https://github.com/Voltstro-Studios/VoltRpc)
* [Docs](https://voltrpc.voltstro.dev/)
