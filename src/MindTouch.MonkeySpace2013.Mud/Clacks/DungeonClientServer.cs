using System;
using System.Net;
using MindTouch.MonkeySpace2013.Mud.Dungeon;

namespace MindTouch.MonkeySpace2013.Mud.Clacks {
    public class DungeonClientServer : DungeonClient, IDisposable {
        private readonly IPEndPoint _endPoint;

        private readonly DungeonServer _server;

        public DungeonClientServer(IPEndPoint endPoint) {
            _endPoint = endPoint;
            _server = new DungeonServer(_endPoint,_debug);
        }

        protected override IPlayer GetPlayer(string name) {
            return new RemotePlayer(name, _endPoint);
        }

        public void Dispose() {
            if(_server != null) {
                _server.Dispose();
            }
        }
    }
}