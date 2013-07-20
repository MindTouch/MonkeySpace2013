using System;

namespace MindTouch.ConsoleUI {
    public class ConsoleLog : ILog {
        public void Debug(string format, params object[] args) {
            Console.WriteLine("DEBUG: " + format, args);
        }

        public void Error(string format, params object[] args) {
            Console.WriteLine("ERROR: " + format, args);
        }
    }
}