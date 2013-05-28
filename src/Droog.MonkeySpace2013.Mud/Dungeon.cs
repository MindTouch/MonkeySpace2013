using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Droog.MonkeySpace2013.Mud {
    public class Dungeon {

        public enum Cmd {
            look,
            go,
            quit,
            say,
            info
        };

        public static void Main() {
            //BlitDemo();
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
            Drawbox(4, 1, 72, 12);
            var v1 = new View(5, 2, 70, 10);
            Drawbox(9, 14, 42, 12);
            var v2 = new View(10, 15, 40, 10);
            while(true) {
                foreach(var t in text) {
                    v1.Write(t);
                    v2.Write(t);
                    Console.ReadKey();
                }
            }
        }

        private static void BlitDemo() {
            var buffer = new[] {
                "+------------------------+",
                "| Hi there               |",
                "| this is just some text |",
                "| to blit to the console |",
                "+------------------------+"
            };
            var r = new Random();
            var w = buffer[0].Length;
            var h = buffer.Length;
            //Blit(buffer, 0, 0);
            //Thread.Sleep(1000);
            //Blit(buffer, 0, Console.WindowHeight - h);
            //Thread.Sleep(1000);
            //Blit(buffer, Console.WindowWidth - w, 0);
            //Thread.Sleep(1000);
            //Blit(buffer, Console.WindowWidth - w, Console.WindowHeight - h);
            //Console.ReadKey();
            Console.CursorVisible = false;
            while(true) {
                var top = r.Next(Console.WindowHeight - h - 4) + 2;
                var left = r.Next(Console.WindowWidth - w - 4) + 2;
                Blit(buffer, left, top);
                //Thread.Sleep(1000);
            }

        }

        private static void Drawbox(int left, int top, int width, int height) {
            Console.SetCursorPosition(left, top);
            Console.Write("+" + Enumerable.Repeat("-", width - 2).Aggregate((x, y) => x + y) + "+");
            top++;
            Console.SetCursorPosition(left, top);
            for(var i = 0; i < height - 2; i++) {
                Console.Write("|" + Enumerable.Repeat(" ", width - 2).Aggregate((x, y) => x + y) + "|");
                top++;
                Console.SetCursorPosition(left, top);
            }
            Console.Write("+" + Enumerable.Repeat("-", width - 2).Aggregate((x, y) => x + y) + "+");
        }

        private static void Blit(string[] buffer, int left, int top) {
            foreach(var line in buffer) {
                Console.SetCursorPosition(left, top);
                Console.Write(line);
                top++;
                Console.SetCursorPosition(0, 0);
            }
        }

        private readonly string[] _commandList = Enum.GetValues(typeof(Cmd)).Cast<Cmd>().Select(x => x.ToString()).ToArray();
        private readonly string[] _directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().Select(x => x.ToString().ToLower()).ToArray();
        private readonly TheHouse theHouse;
        private readonly Player player;

        public Dungeon() {
            theHouse = new TheHouse();
            player = theHouse.AddPlayer();
        }

        public void Run() {
            var topHalf = Console.WindowHeight / 2;
            var bottomHalf = Console.WindowHeight - topHalf;
            var debug = new FramedView(0, 0, Console.WindowWidth, topHalf);
            var view = new FramedView(0, topHalf, Console.WindowWidth, bottomHalf, debug);
            //var view = new ConsoleView();
            var le = new LineEditor(view, "dungeon") { TabAtStartCompletes = true, AutoCompleteEvent = AutoComplete };
            view.WriteLine(player.Look());
            string line = "";
            while((line = le.Edit(GetPrompt(), "")) != null) {
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if(parts.Length == 0) {
                    continue;
                }
                Cmd cmd;
                var handled = false;
                if(Enum.TryParse(parts[0], true, out cmd)) {
                    switch(cmd) {
                    case Cmd.quit:
                        return;
                    case Cmd.look:
                        view.WriteLine(player.Look());
                        handled = true;
                        break;
                    case Cmd.go:
                        if(parts.Length < 2) {
                            break;
                        }
                        Direction direction;
                        Enum.TryParse(parts[1], true, out direction);
                        view.WriteLine(player.Go(direction));
                        handled = true;
                        break;
                    case Cmd.say:
                        var said = line.Substring(line.IndexOf(" ")).Trim();
                        player.Say(said);
                        handled = true;
                        break;
                    }
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
            Cmd cmd;
            if(Enum.TryParse(parts[0], true, out cmd)) {
                switch(cmd) {
                case Cmd.go:
                    if(parts.Length == 1) {
                        if(char.IsWhiteSpace(line.Last())) {
                            return new LineEditor.Completion("", _directions);
                        }
                        return new LineEditor.Completion("", new[] { " " });
                    }
                    return new LineEditor.Completion(line, GetCompletions(parts[1], _directions));
                }
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