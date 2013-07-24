using System;
using System.Linq;
using System.Net;

namespace MindTouch.MonkeySpace2013.Client {
    public static class RemoteRunner {

        public static void Main(string[] args) {
            var argList = (args ?? new string[0]).ToList();
            var addr = IPAddress.Parse(argList.ElementAtOrDefault(1) ?? "127.0.0.1");
            var port = int.Parse(argList.ElementAtOrDefault(2) ?? "1234");
            var endpoint = new IPEndPoint(addr, port);
            var dungeon = new DungeonRemoteClient(endpoint);
            dungeon.Play(argList.FirstOrDefault());
        }
    }
}