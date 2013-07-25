using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Clacks.Client;

namespace Sandbox {
    public class Client : IDisposable {
        private readonly ClacksClient _client;

        public Client() {
            _client = new ClacksClient("127.0.0.1", 12345);
        }

        public string Echo(string msg) {
            var response = _client.Exec(Request.Create("ECHO").WithData(Encoding.UTF8.GetBytes(msg)));
            return Encoding.UTF8.GetString(response.Data);
        }

        public IEnumerable<Tuple<int,string>> Repeat(int n, string wat) {
            var responses = _client.Exec(MultiRequest.Create("REPEAT").WithArgument(n.ToString()).WithArgument(wat).ExpectMultiple("OK",false).TerminatedBy("DONE"));
            return responses.Select(x => new Tuple<int, string>(int.Parse(x.Arguments[0]), x.Arguments[1]));
        }

        public void Dispose() {
            _client.Dispose();
        }
    }
}