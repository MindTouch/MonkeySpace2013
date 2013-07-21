using System;
using System.Net;
using System.Text;
using MindTouch.Clacks.Server;
using MindTouch.ConsoleUI;
using MindTouch.MonkeySpace2013.Mud.Dungeon;

namespace MindTouch.MonkeySpace2013.Mud.Clacks {
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
                    var player = _house.Join(name);
                    _log.Debug("server: '{0}' joined as '{1}'", name, player.Name);
                    return Response.Create("OK").WithArgument(Uri.EscapeDataString(player.Name));
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
