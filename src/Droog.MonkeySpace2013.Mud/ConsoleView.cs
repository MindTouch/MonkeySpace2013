using System;

namespace Droog.MonkeySpace2013.Mud {
    public class ConsoleView : IView {
        public void Focus() {
        }

        public void Blur() {
        }

        public bool HasFocus{
            get { return true; }
        }

        public void WriteLine(string format, params object[] args) {
            Console.WriteLine(format, args);
        }

        public void WriteLine() {
            Console.WriteLine();
        }

        public event ConsoleCancelEventHandler CancelKeyPress {
            add { Console.CancelKeyPress += value; }
            remove { Console.CancelKeyPress -= value; }
        }

        public void SetCursorPosition(int left, int top) {
            Console.SetCursorPosition(left, top);
        }

        public ConsoleKeyInfo ReadKey(bool intercept) {
            return Console.ReadKey(intercept);
        }

        public void Clear() {
            Console.Clear();
        }

        public void Write(string format, params object[] args) {
            Console.Write(format, args);
        }

        public void Write(object value) {
            Console.Write(value);
        }

        public int WindowWidth {
            get { return Console.WindowWidth; }
            set { Console.WindowHeight = value; }
        }

        public int BufferHeight {
            get { return Console.BufferHeight; }
            set { Console.BufferHeight = value; }
        }

        public int CursorTop {
            get { return Console.CursorTop; }
        }

        public int Top { get { return 0; } }
        public int Left { get { return 0; } }
        public int CursorLeft {
            get { return Console.CursorLeft; }
        }
    }
}