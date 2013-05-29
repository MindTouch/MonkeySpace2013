using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Droog.MonkeySpace2013.Mud {
    public class Dungeon {

        public static void Main() {
            //ViewDemo();
            var dungeon = new Dungeon();
            dungeon.Run();
        }

        private static void ViewDemo() {
            var text = new[] {
@"Lorem ipsum dolor sit amet, consectetur adipiscing elit.
Donec vel nisi ac enim ullamcorper laoreet at ultricies arcu.
Sed vel nibh ut nunc accumsan laoreet sed non massa. ",
@"Curabitur nulla neque, sodales semper volutpat sit amet, ornare quis elit.
Donec fringilla nisl id dolor tempor quis ullamcorper enim adipiscing.",
@"

In vel nisi odio. Phasellus metus nisi, pulvinar ut pellentesque at, porta quis quam.
Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.
",
@"Nulla aliquam lacus quis lectus ullamcorper non ornare nisi tincidunt.
Donec venenatis pharetra enim eu sodales. Nulla tempor vestibulum lorem, sed sagittis purus tristique in. Vestibulum in nibh quis nunc ullamcorper tempus vel in turpis.

",
@"Fusce laoreet pulvinar felis at ornare.
Cras et tortor ipsum, a fermentum sapien. Nam congue massa at quam sollicitudin tincidunt.
Donec ut bibendum urna. Praesent vel nisl purus.
",
@"Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Aenean nibh massa, venenatis ac consectetur non, mollis vel ante. Sed imperdiet mauris nunc, sit amet volutpat magna."};
            var host = new ConsoleViewHost();
            var v1 = new FramedView(host, 5, 0, 70, 10);
            var v2 = new FramedView(host, 10, 15, 40, 10);
            v1.Title = "Top Pane";
            var v3 = new FramedView(host, 40, 10, Console.WindowWidth - 40, Console.WindowHeight - 10);
            host.Invalidate();
            while(true) {
                foreach(var t in text) {
                    v1.Write(t);
                    v2.Write(t);
                    v3.Write(t);
                    Thread.Sleep(500);
                }
            }
        }

        private readonly string[] _commandList = new[] { "quit", "go", "look", "say", "debug" };
        private readonly string[] _directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().Select(x => x.ToString().ToLower()).ToArray();
        private readonly TheHouse theHouse;
        private readonly Player player;

        public Dungeon() {
            theHouse = new TheHouse();
            player = theHouse.AddPlayer();
        }

        public void Run() {
            var host = new ConsoleViewHost();
            var topHalf = Console.WindowHeight / 2;
            var viewHeight = Console.WindowHeight - topHalf;
            var viewTop = topHalf - 3;
            var debug = new FramedView(host, 0, 0, Console.WindowWidth, viewTop) { Title = "Debug" };
            debug.Hide();
            var view = new FramedView(host, 0, 0, Console.WindowWidth, Console.WindowHeight - 3, debug);
            var input = new FramedView(host, 0, Console.WindowHeight - 3, Console.WindowWidth, 3, debug) { Title = "Command" };
            input.Focus();
            //var view = new ConsoleView();
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