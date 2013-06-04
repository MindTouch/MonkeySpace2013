using System;

namespace MindTouch.MonkeySpace2013.Mud {
    public class TheVoid : Room {
        public Room Exit;
        public override string Look(Player who) {
            return Description;
        }

        public override Space this[Direction direction] {
            get { return new Random().Next(3) == 0 ? Exit : this; }
            set { base[direction] = value; }
        }
    }
}