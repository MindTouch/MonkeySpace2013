using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
            BlitDemo();
            var dungeon = new Dungeon();
            dungeon.Run();
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
                var top = r.Next(Console.WindowHeight - h - 1);
                var left = r.Next(Console.WindowWidth - w - 1);
                Blit(buffer, left, top);
                //Thread.Sleep(1000);
            }

        }

        private static void Blit(string[] buffer, int left, int top) {
            foreach(var line in buffer) {
                Console.SetCursorPosition(left, top);
                Console.Write(line);
                top++;
                Console.SetCursorPosition(0,0);
            }
        }

        private string[] _commandList = Enum.GetValues(typeof(Cmd)).Cast<Cmd>().Select(x => x.ToString()).ToArray();
        private string[] _directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().Select(x => x.ToString().ToLower()).ToArray();
        private TheHouse theHouse;
        private Player player;

        public Dungeon() {

            theHouse = new TheHouse();
            player = theHouse.AddPlayer();
        }

        public void Run() {
            Console.SetCursorPosition(0, Console.WindowHeight);
            var window = new ConsoleWindow();
            var le = new LineEditor(window, "dungeon") { TabAtStartCompletes = true, AutoCompleteEvent = AutoComplete };
            Console.WriteLine(player.Look());
            string line = "";
            while((line = le.Edit(GetPrompt(), "")) != null) {
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if(parts.Length == 0) {
                    continue;
                }
                Cmd cmd;
                if(Enum.TryParse(parts[0], true, out cmd)) {
                    switch(cmd) {
                    case Cmd.quit:
                        return;
                    case Cmd.look:
                        Console.WriteLine(player.Look());
                        break;
                    case Cmd.go:
                        if(parts.Length == 0) {
                            continue;
                        }
                        Direction direction;
                        Enum.TryParse(parts[1], true, out direction);
                        Console.WriteLine(player.Go(direction));
                        break;
                    case Cmd.say:
                        var said = line.Substring(line.IndexOf(" ")).Trim();
                        player.Say(said);
                        break;
                    case Cmd.info:
                        Console.WriteLine("[{0},{1}],[{2},{3}]", Console.WindowWidth, Console.WindowHeight, Console.CursorLeft, Console.CursorTop);
                        break;
                    default:
                        continue;
                    }
                }
                foreach(var heard in player.Listen()) {
                    Console.WriteLine(heard);
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