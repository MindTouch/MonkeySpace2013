using System;

namespace MindTouch.ConsoleUI {
    public interface IView {
        event ConsoleCancelEventHandler CancelKeyPress;
        void SetCursorPosition(int left, int top);
        ConsoleKeyInfo ReadKey();
        void Clear();
        void WriteLine(string format, params object[] args);
        void WriteLine();
        void Write(string format, params object[] args);
        void Write(object value);
        int Width { get; }
        int Height { get; }
        int CursorTop { get; }
        int CursorLeft { get; }
    }
}