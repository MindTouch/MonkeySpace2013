using System;

namespace MindTouch.MonkeySpace2013.Mud {
    public class DungeonLocalClient : DungeonClient {

        public DungeonLocalClient() {
        }
        protected override IPlayer GetPlayer(string name) {
            return new TheHouse().Join(name);
        }

    }
}