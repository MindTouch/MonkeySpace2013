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
                    .HandledBy((r,c) => {
                        Console.WriteLine("got ECHO");
                        c(Response.Create("ECHO").WithArguments(r.Arguments));
                    })
                    .Register()
                .WithCommand("REPEAT")
                    .ExpectsData()
                    .HandledBy((r,c) => {
                        var n = int.Parse(r.Arguments[0]);
                        var wat = r.Arguments[1];
                        var enumerator = Enumerable.Repeat(wat, n).Select((X,I) => new {X,I}).GetEnumerator();
                        Action f = null;
                        f = () => {
                            if(!enumerator.MoveNext()) {
                                c(Response.Create("DONE"), null);
                                return;
                            }
                            var v = enumerator.Current;
                            c(Response.Create("OK").WithArgument(v.I).WithArgument(v.X), f);
                        };
                        f();
                    })
                    .Register()
                .WithDefaultHandler(r => {
                    Console.WriteLine("Got '{0}'", r.Command);
                    return Response.Create("WAT");
                })
                .Build();
        }

        public void Dispose() {
            _server.Dispose();
        }
    }
}
