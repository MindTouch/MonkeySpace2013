using System.Linq;
using System.Net;

namespace MindTouch.MonkeySpace2013.Mud {
    public static class RemoteRunner1Player {

        public static void Main(string[] args) {
            var addr = IPAddress.Parse( "127.0.0.1");
            var port = int.Parse("1234");
            var endpoint = new IPEndPoint(addr, port);
            var dungeon = new DungeonRemoteClient(endpoint, true);
            dungeon.Play("bob");
        }
    }
}