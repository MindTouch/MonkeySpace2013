using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using MindTouch.Clacks.Server;
using Response = MindTouch.Clacks.Server.Response;

namespace Sandbox {
    public class Server : IDisposable {
        private readonly ClacksServer _server;

        public Server() {
            _server = ServerBuilder.CreateAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345))
                .WithCommand("ECHO")
                    .ExpectsData()
                    .HandledBy(r => Response.Create("HI").WithData(r.Data))
                    .Register()
                .WithCommand("REPEAT")
                    .ExpectsData()
                    .HandledBy(r => {
                        var n = int.Parse(r.Arguments[0]);
                        return Enumerable.Repeat(Response.Create("OK").WithData(r.Data), n);
                    })
                    .Register()
                .WithDefaultHandler(r => Response.Create("WAT"))
                .WithErrorHandler((r, e) => Response.Create("ERROR").WithData(Encoding.UTF8.GetBytes(e.StackTrace)))
                .Build();
        }

        public void Dispose() {
            _server.Dispose();
        }
    }
}
