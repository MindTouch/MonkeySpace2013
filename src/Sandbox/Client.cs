using System;
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

        public void Dispose() {
            _client.Dispose();
        }
    }
}