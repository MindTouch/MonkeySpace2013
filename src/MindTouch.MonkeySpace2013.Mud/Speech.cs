namespace MindTouch.MonkeySpace2013.Mud {
    public class Speech {
        public Room Where;
        public Player Who;
        public string What;

        public Speech(Room where, Player who, string what) {
            Where = where;
            Who = who;
            What = what;
        }
    }
}