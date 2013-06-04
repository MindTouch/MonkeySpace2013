namespace MindTouch.MonkeySpace2013.Mud {
    public class Connector : Space {
        public Room From;
        public Room To;

        public Room GetTo(Room room) {
            return To == room ? From : To;
        }
    }
}