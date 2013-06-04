using System.Linq;
using System.Net;

namespace MindTouch.MonkeySpace2013.Mud {
    public static class RemoteRunner {

        public static void Main(string[] args) {
            var argList = (args ?? new string[0]).ToList();
            var addr = IPAddress.Parse(argList.ElementAtOrDefault(1) ?? "127.0.0.1");
            var port = int.Parse(argList.ElementAtOrDefault(2) ?? "1234");
            var runServer = !string.IsNullOrEmpty(argList.ElementAtOrDefault(3));
            var endpoint = new IPEndPoint(addr, port);
            var dungeon = new DungeonRemoteClient(endpoint, runServer);
            dungeon.Play(argList.FirstOrDefault());
        }
    }
}