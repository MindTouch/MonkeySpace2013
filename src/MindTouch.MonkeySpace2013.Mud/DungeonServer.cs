using System;
using System.Linq;
using System.Net;
using System.Text;
using MindTouch.Clacks.Server;
using MindTouch.ConsoleUI;

namespace MindTouch.MonkeySpace2013.Mud {

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
    public class DungeonServer : IDisposable {
        private readonly ILog _log;

        private readonly TheHouse _house;
        private readonly ClacksServer _server;

        public DungeonServer(IPEndPoint endpoint, ILog log) {
            _log = log;
            _house = new TheHouse();
            _server = ServerBuilder.CreateAsync(endpoint)
                .WithCommand("JOIN").ExpectsNoData().HandledBy(r => {
                    var name = GetName(r);
                    _house.Join(name);
                    _log.Debug("server: '{0}' joined", name);
                    return Response.Create("OK");
                }).Register()
                .WithCommand("LEAVE").ExpectsNoData().HandledBy(r => {
                    var player = GetPlayer(r);
                    _house.Leave(player);
                    _log.Debug("server: '{0}' left", player.Name);
                    return Response.Create("OK");
                }).Register()
                .WithCommand("LISTEN").ExpectsNoData().HandledBy(r => {
                    var player = GetPlayer(r);
                    return Response.Create("OK").WithData(Encode(string.Join("\r\n", player.Listen())));
                }).Register()
                .WithCommand("LOOK").ExpectsNoData().HandledBy(r => {
                    var player = GetPlayer(r);
                    return Response.Create("OK").WithData(Encode(player.Look()));
                }).Register()
                .WithCommand("LOCATION").ExpectsNoData().HandledBy(r => {
                    var player = GetPlayer(r);
                    return Response.Create("OK").WithData(Encode(player.Location.Name));
                }).Register()
                .WithCommand("GO").ExpectsNoData().HandledBy(r => {
                    var player = GetPlayer(r);
                    var direction = (Direction)Enum.Parse(typeof(Direction), r.Arguments[1]);
                    return Response.Create("OK").WithData(Encode(player.Go(direction)));
                }).Register()
                .WithCommand("SAY").ExpectsData().HandledBy(r => {
                    var player = GetPlayer(r);
                    player.Say(Encoding.ASCII.GetString(r.Data));
                    return Response.Create("OK");
                }).Register()
                .Build();
            log.Debug("started server on {0}", endpoint);
        }

        private byte[] Encode(string data) {
            return Encoding.ASCII.GetBytes(data);
        }

        private Player GetPlayer(IRequest r) {
            var player = _house.Players[GetName(r)];
            return player;
        }

        private string GetName(IRequest r) {
            var name = Uri.UnescapeDataString(r.Arguments[0]);
            return name;
        }

        public void Dispose() {
            _server.Dispose();
        }
    }
}
