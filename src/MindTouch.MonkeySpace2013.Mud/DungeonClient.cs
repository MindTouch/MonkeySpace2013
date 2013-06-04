using System;
using System.Collections.Generic;
using System.Linq;
using MindTouch.ConsoleUI;

namespace MindTouch.MonkeySpace2013.Mud {
    public class DungeonClient {
        private readonly string[] _commandList = new[] { "quit", "go", "look", "say", "debug" };
        private readonly string[] _directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().Select(x => x.ToString().ToLower()).ToArray();
        private Player player;
        public void Play(string name) {
            using(player = new TheHouse().Join(name)) {
                var host = new ConsoleViewHost();
                var topHalf = Console.WindowHeight / 2;
                var viewHeight = Console.WindowHeight - topHalf;
                var viewTop = topHalf - 3;
                var debug = new FramedView(host, 0, 0, Console.WindowWidth, viewTop) { Title = "Debug" };
                debug.Hide();
                var view = new FramedView(host, 0, 0, Console.WindowWidth, Console.WindowHeight - 3, debug);
                var input = new FramedView(host, 0, Console.WindowHeight - 3, Console.WindowWidth, 3, debug) { Title = "Command" };
                input.Focus();
                var le = new LineEditor(input, "dungeon") { TabAtStartCompletes = true, AutoCompleteEvent = AutoComplete };
                view.WriteLine(player.Look());
                string line = "";
                while((line = le.Edit(GetPrompt(), "")) != null) {
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
                        view.WriteLine(player.Look());
                        break;
                    case "n":
                        view.WriteLine(player.Go(Direction.North));
                        break;
                    case "s":
                        view.WriteLine(player.Go(Direction.South));
                        break;
                    case "e":
                        view.WriteLine(player.Go(Direction.East));
                        break;
                    case "w":
                        view.WriteLine(player.Go(Direction.West));
                        break;
                    case "u":
                        view.WriteLine(player.Go(Direction.Up));
                        break;
                    case "d":
                        view.WriteLine(player.Go(Direction.Down));
                        break;
                    case "go":
                        if(parts.Length < 2) {
                            break;
                        }
                        Direction direction;
                        Enum.TryParse(parts[1], true, out direction);
                        view.WriteLine(player.Go(direction));
                        break;
                    case "say":
                        var said = line.Substring(line.IndexOf(" ")).Trim();
                        player.Say(said);
                        break;
                    case "debug":
                        if(debug.IsVisible) {
                            debug.Hide();
                            view.Top = 0;
                            view.Height = Console.WindowHeight - 3;
                        } else {
                            debug.Show();
                            view.Top = viewTop;
                            view.Height = viewHeight;
                        }
                        break;
                    default:
                        handled = false;
                        break;
                    }
                    if(!handled) {
                        view.WriteLine("Unknown command: {0}", line);
                        view.WriteLine();
                    }
                    foreach(var heard in player.Listen()) {
                        view.WriteLine(heard);
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
            return player.Location.Name + " > ";
        }
    }
}