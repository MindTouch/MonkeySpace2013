using System.Linq;
using System.Net;

namespace MindTouch.MonkeySpace2013.Mud {
    public static class LocalRunner {

        public static void Main(string[] args) {
            var argList = (args ?? new string[0]).ToList();
            var dungeon = new DungeonLocalClient();
            dungeon.Play(argList.FirstOrDefault());
        }
    }
}