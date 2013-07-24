using System;
using System.Linq;
using System.Net;
using MindTouch.ConsoleUI;

namespace MindTouch.MonkeySpace2013.Server {
    public static class ServerRunner {
        public static void Main(string[] args) {
            var argList = (args ?? new string[0]).ToList();
            var addr = IPAddress.Parse(argList.FirstOrDefault() ?? "127.0.01");
            var port = Int32.Parse(argList.ElementAtOrDefault(1) ?? "1234");
            var host = new ConsoleViewHost();
            var log = (new FramedView(host, 0, 0, Console.WindowWidth, Console.WindowHeight) { Title = "Dungeon Server" }) as ILog;
            using(new DungeonServer(new IPEndPoint(addr, port), log)) {
                Console.ReadKey();
            }
        }
    }
}