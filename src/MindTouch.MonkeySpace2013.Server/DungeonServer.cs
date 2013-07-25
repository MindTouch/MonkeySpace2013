using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using MindTouch.Clacks.Server;
using MindTouch.ConsoleUI;
using MindTouch.MonkeySpace2013.Mud.Dungeon;

namespace MindTouch.MonkeySpace2013.Server {
    public class DungeonServer : IDisposable {
        private readonly ILog _log;

        private readonly TheHouse _house;
        private readonly ClacksServer _server;
        private readonly Dictionary<IPEndPoint, Player> _playerConnectionLookup = new Dictionary<IPEndPoint, Player>();

        public DungeonServer(IPEndPoint endpoint, ILog log) {
            _log = log;
            _house = new TheHouse();
            _server = ServerBuilder.CreateAsync(endpoint)
                .WithCommand("JOIN").ExpectsData().HandledBy(r => {
                    var name = Encoding.ASCII.GetString(r.Data);
                    var player = _house.Join(name);
                    _playerConnectionLookup[r.Client] = player;
                    _log.Debug("server: '{0}' joined as '{1}'", name, player.Name);
                    return Response.Create("OK").WithData(Encode(name));
                }).Register()
                .WithCommand("LEAVE").ExpectsNoData().HandledBy(r => {
                    var player = _playerConnectionLookup[r.Client];
                    _house.Leave(player);
                    _log.Debug("server: '{0}' left", player.Name);
                    return Response.Create("OK");
                }).Register()
                .WithCommand("LISTEN").ExpectsNoData().HandledBy(r => {
                    var player = _playerConnectionLookup[r.Client];
                    return Response.Create("OK").WithData(Encode(string.Join("\r\n", player.Listen())));
                }).Register()
                .WithCommand("LOOK").ExpectsNoData().HandledBy(r => {
                    var player = _playerConnectionLookup[r.Client];
                    return Response.Create("OK").WithData(Encode(player.Look()));
                }).Register()
                .WithCommand("LOCATION").ExpectsNoData().HandledBy(r => {
                    var player = _playerConnectionLookup[r.Client];
                    return Response.Create("OK").WithData(Encode(player.Location.Name));
                }).Register()
                .WithCommand("GO").ExpectsNoData().HandledBy(r => {
                    var player = _playerConnectionLookup[r.Client];
                    var direction = (Direction)Enum.Parse(typeof(Direction), r.Arguments[0], true);
                    return Response.Create("OK").WithData(Encode(player.Go(direction)));
                }).Register()
                .WithCommand("SAY").ExpectsData().HandledBy(r => {
                    var player = _playerConnectionLookup[r.Client];
                    player.Say(Encoding.ASCII.GetString(r.Data));
                    return Response.Create("OK");
                }).Register()
                .OnClientDisconnected((id, ip) => {
                    var player = _playerConnectionLookup[ip];
                    player.Dispose();
                    _playerConnectionLookup.Remove(ip);
                })
                .Build();
            log.Debug("started server on {0}", endpoint);
        }

        private byte[] Encode(string data) {
            return Encoding.ASCII.GetBytes(data);
        }

        public void Dispose() {
            _server.Dispose();
        }
    }
}
