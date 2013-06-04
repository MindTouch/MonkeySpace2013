using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using MindTouch.Clacks.Client;

namespace MindTouch.MonkeySpace2013.Mud {
    public class RemotePlayer : IPlayer {
        private readonly string _name;
        private readonly ClacksClient _client;

        public RemotePlayer(string name, IPEndPoint addr) {
            _name = name;
            _client = new ClacksClient(addr);
            _client.Exec(R("JOIN"));
        }

        private Request R(string command) {
            return Request.Create(command).WithArgument(Uri.EscapeDataString(_name));
        }

        public IEnumerable<string> Listen() {
            var r = _client.Exec(R("LISTEN").ExpectData("OK"));
            return Encoding.ASCII.GetString(r.Data).Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string Look() {
            var response = _client.Exec(R("LOOK").ExpectData("OK"));
            return Encoding.ASCII.GetString(response.Data);
        }

        public string Go(Direction direction) {
            return Encoding.ASCII.GetString(_client.Exec(R("GO").WithArgument(direction.ToString()).ExpectData("OK")).Data);
        }

        public void Say(string said) {
            _client.Exec(R("SAY").WithData(Encoding.ASCII.GetBytes(said)));
        }

        public string GetLocation() {
            return Encoding.ASCII.GetString(_client.Exec(R("LOCATION").ExpectData("OK")).Data);
        }

        public void Dispose() {
            _client.Exec(R("LEAVE"));
        }


    }
}