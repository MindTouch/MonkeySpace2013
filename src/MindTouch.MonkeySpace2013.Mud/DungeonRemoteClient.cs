using System;
using System.Net;

namespace MindTouch.MonkeySpace2013.Mud {
    public class DungeonRemoteClient : DungeonClient, IDisposable {
        private readonly IPEndPoint _endPoint;

        private readonly DungeonServer _server;

        public DungeonRemoteClient(IPEndPoint endPoint, bool runServer) {
            _endPoint = endPoint;
            if(runServer) {
                _server = new DungeonServer(endPoint, _debug);
            }
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