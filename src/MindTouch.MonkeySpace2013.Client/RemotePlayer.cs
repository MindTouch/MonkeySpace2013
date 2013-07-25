using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using MindTouch.Clacks.Client;
using MindTouch.MonkeySpace2013.Mud.Dungeon;

namespace MindTouch.MonkeySpace2013.Client {
    public class RemotePlayer : IPlayer {
        private readonly string _name;
        private readonly ClacksClient _client;

        public RemotePlayer(string name, IPEndPoint addr) {
            _client = new ClacksClient(addr);
            _name = Encoding.ASCII.GetString(_client.Exec("JOIN", r => r.WithData(name).ExpectData("OK")).Data);
        }

        public string Name { get { return _name; } }

        public IEnumerable<string> Listen() {
            var response = _client.Exec("LISTEN", r => r.ExpectData("OK"));
            return Encoding.ASCII.GetString(response.Data).Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string Look() {
            var response = _client.Exec("LOOK", r => r.ExpectData("OK"));
            return Encoding.ASCII.GetString(response.Data);
        }

        public string Go(Direction direction) {
            return Encoding.ASCII.GetString(_client.Exec("GO", r => r.WithArgument(direction.ToString()).ExpectData("OK")).Data);
        }

        public void Say(string said) {
            _client.Exec("SAY", r => r.WithData(Encoding.ASCII.GetBytes(said)));
        }

        public string GetLocation() {
            return Encoding.ASCII.GetString(_client.Exec("LOCATION", r => r.ExpectData("OK")).Data);
        }

        public void Dispose() {
            _client.Exec("LEAVE");
        }
    }
}