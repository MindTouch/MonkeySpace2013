using System;
using System.Linq;
using System.Net;
using MindTouch.ConsoleUI;

namespace MindTouch.MonkeySpace2013.Mud.Clacks {
    public static class RemoteRunner {

        public static void Main(string[] args) {
            var argList = (args ?? new string[0]).ToList();
            var server = args.FirstOrDefault() == "server";
            var addr = IPAddress.Parse(argList.ElementAtOrDefault(1) ?? "127.0.0.1");
            var port = int.Parse(argList.ElementAtOrDefault(2) ?? "1234");
            var endpoint = new IPEndPoint(addr, port);
            if(server) {
                var host = new ConsoleViewHost();
                var log = (new FramedView(host, 0, 0, Console.WindowWidth, Console.WindowHeight) { Title = "Dungeon Server" }) as ILog;
                using(new DungeonServer(endpoint, log)) {
                    Console.WriteLine("PRESS ANY KEY TO EXIT");
                    Console.ReadKey();
                    return;
                }
            }
            var dungeon = new DungeonRemoteClient(endpoint);
            dungeon.Play(argList.FirstOrDefault());
        }
    }
}