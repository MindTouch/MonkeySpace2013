using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MindTouch.ConsoleUI;

namespace MindTouch.MonkeySpace2013.Mud {
    public class DungeonRemoteClient : IDisposable {
        private readonly IPEndPoint _endPoint;
        private readonly string[] _commandList = new[] { "quit", "go", "look", "say", "debug" };
        private readonly string[] _directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().Select(x => x.ToString().ToLower()).ToArray();

        private IPlayer _player;
        private readonly FramedView _view;
        private readonly FramedView _input;
        private readonly FramedView _debug;
        private readonly DungeonServer _server;
        private readonly int _viewHeight;
        private readonly int _viewTop;

        public DungeonRemoteClient(IPEndPoint endPoint, bool runServer) {
            _endPoint = endPoint;
            var host = new ConsoleViewHost();
            var topHalf = Console.WindowHeight / 2;
            _viewHeight = Console.WindowHeight - topHalf;
            _viewTop = topHalf - 3;
            _debug = new FramedView(host, 0, 0, Console.WindowWidth, _viewTop) { Title = "Debug" };
            _debug.Hide();
            _view = new FramedView(host, 0, 0, Console.WindowWidth, Console.WindowHeight - 3, _debug);
            _input = new FramedView(host, 0, Console.WindowHeight - 3, Console.WindowWidth, 3, _debug) { Title = "Command" };
            if(runServer) {
                _server = new DungeonServer(endPoint, _debug);
            }
        }

        public void Play(string name) {
            using(var listener = new System.Timers.Timer(200))
            using(_player = new RemotePlayer(name, _endPoint)) {
                _input.Focus();
                var le = new LineEditor(_input, "dungeon") { TabAtStartCompletes = true, AutoCompleteEvent = AutoComplete };
                _view.WriteLine(_player.Look());
                string line = "";
                listener.Elapsed += (sender, args) => {
                    lock(_view) {
                        foreach(var heard in _player.Listen()) {
                            _view.WriteLine(heard);
                        }
                    }
                };
                listener.Start();
                while((line = le.Edit(GetPrompt(), "")) != null) {
                    lock(_view) {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if(parts.Length == 0) {
                            continue;
                        }
                        var handled = true;
                        switch(parts[0]) {
                        case "quit":
                            return;
                        case "l":
                        case "look":
                            _view.WriteLine(_player.Look());
                            break;
                        case "n":
                            _view.WriteLine(_player.Go(Direction.North));
                            break;
                        case "s":
                            _view.WriteLine(_player.Go(Direction.South));
                            break;
                        case "e":
                            _view.WriteLine(_player.Go(Direction.East));
                            break;
                        case "w":
                            _view.WriteLine(_player.Go(Direction.West));
                            break;
                        case "u":
                            _view.WriteLine(_player.Go(Direction.Up));
                            break;
                        case "d":
                            _view.WriteLine(_player.Go(Direction.Down));
                            break;
                        case "go":
                            if(parts.Length < 2) {
                                break;
                            }
                            Direction direction;
                            Enum.TryParse(parts[1], true, out direction);
                            _view.WriteLine(_player.Go(direction));
                            break;
                        case "say":
                            if(parts.Length < 2) {
                                _view.WriteLine("you gotta say something");
                                continue;
                            }
                            var said = string.Join(" ", parts.Skip(1));
                            _player.Say(said);
                            break;
                        case "debug":
                            if(_debug.IsVisible) {
                                _debug.Hide();
                                _view.Top = 0;
                                _view.Height = Console.WindowHeight - 3;
                            } else {
                                _debug.Show();
                                _view.Top = _viewTop;
                                _view.Height = _viewHeight;
                            }
                            break;
                        default:
                            handled = false;
                            break;
                        }
                        if(!handled) {
                            _view.WriteLine("Unknown command: {0}", line);
                            _view.WriteLine();
                        }
                    }
                }
            }
        }

        private LineEditor.Completion AutoComplete(string line, int pos) {
            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if(parts.Length == 0) {
                return new LineEditor.Completion("", _commandList);
            }
            switch(parts[0]) {
            case "go":
                if(parts.Length == 1) {
                    if(char.IsWhiteSpace(line.Last())) {
                        return new LineEditor.Completion("", _directions);
                    }
                    return new LineEditor.Completion("", new[] { " " });
                }
                return new LineEditor.Completion(line, GetCompletions(parts[1], _directions));
            }
            if(parts.Length == 1) {
                var cmdStr = parts[0];
                return new LineEditor.Completion(cmdStr, GetCompletions(cmdStr, _commandList));
            }
            return new LineEditor.Completion("", new string[0]);
        }

        private string[] GetCompletions(string partial, IEnumerable<string> candidates) {
            return candidates.Where(x => x.StartsWith(partial)).Select(x => x.Substring(partial.Length)).ToArray();
        }


        private string GetPrompt() {
            return _player.GetLocation() + " > ";
        }

        public void Dispose() {
            if(_server != null) {
                _server.Dispose();
            }
        }
    }
}