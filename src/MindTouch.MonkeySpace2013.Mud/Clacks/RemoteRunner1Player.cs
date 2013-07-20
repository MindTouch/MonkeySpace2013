using System.Linq;
using System.Net;

namespace MindTouch.MonkeySpace2013.Mud.Clacks {
    public static class RemoteRunner1Player {

        public static void Main(string[] args) {
            var argList = (args ?? new string[0]).ToList();
            var addr = IPAddress.Parse(argList.ElementAtOrDefault(1) ?? "127.0.0.1");
            var port = int.Parse(argList.ElementAtOrDefault(2) ?? "1234");
            var endpoint = new IPEndPoint(addr, port);
            var dungeon = new DungeonRemoteClient(endpoint);
            using(new DungeonServer(endpoint, dungeon.Log)) {
                dungeon.Play(argList.FirstOrDefault());
            }
        }
    }
}
