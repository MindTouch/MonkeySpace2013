using System;
using System.Linq;
using System.Net;
using System.Text;
using MindTouch.Clacks.Server;
using Response = MindTouch.Clacks.Server.Response;

namespace Sandbox {

    public static class App {

        public static void Main(string[] args) {
            using(new Server()) {
                Console.WriteLine("server running");
                Console.ReadKey();
            }
        }
    }
    public class Server : IDisposable {


        private readonly ClacksServer _server;

        public Server() {
            _server = ServerBuilder.CreateAsync(new IPEndPoint(IPAddress.Parse("11.11.11.10"), 12345))
                .WithCommand("ECHO")
                    .HandledBy(r => {
                        Console.WriteLine("got ECHO");
                        return Response.Create("ECHO").WithArguments(r.Arguments);
                    })
                    .Register()
                .WithCommand("REPEAT")
                    .ExpectsData()
                    .HandledBy(r => {
                        var n = int.Parse(r.Arguments[0]);
                        return Enumerable.Repeat(Response.Create("OK").WithData(r.Data), n);
                    })
                    .Register()
                .WithDefaultHandler(r => {
                    Console.WriteLine("Got '{0}'", r.Command);
                    return Response.Create("WAT");
                })
                .WithErrorHandler((r, e) => Response.Create("ERROR").WithData(Encoding.UTF8.GetBytes(e.StackTrace)))
                .OnClientConnected((id,ip) => Console.WriteLine("Got client '{0}' from '{1}", id, ip))
                .OnClientDisconnected(id => Console.WriteLine("Client '{0}' left", id))
                .OnReceivedCommand(info => Console.WriteLine("Args: {0}", string.Join(",", info.Args)))      
                .Build();
        }

        public void Dispose() {
            _server.Dispose();
        }
    }
}
