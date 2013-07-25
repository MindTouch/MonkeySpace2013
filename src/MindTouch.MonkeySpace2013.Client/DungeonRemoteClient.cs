using System.Net;
using MindTouch.MonkeySpace2013.Mud;
using MindTouch.MonkeySpace2013.Mud.Dungeon;

namespace MindTouch.MonkeySpace2013.Client {
    public class DungeonRemoteClient : DungeonClient {
        private readonly IPEndPoint _endPoint;

        public DungeonRemoteClient(IPEndPoint endPoint) {
            _endPoint = endPoint;
        }

        protected override IPlayer GetPlayer(string name) {
            return new RemotePlayer(name, _endPoint);
        }
    }
}