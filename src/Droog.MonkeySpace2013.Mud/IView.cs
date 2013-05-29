using System;
using System.Collections.Generic;

namespace Droog.MonkeySpace2013.Mud {
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

    public interface IPane {
        int CursorTop { get; }
        int CursorLeft { get; }
        int Top { get; set; }
        int Left { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        IEnumerable<char[]> VisibleBuffer { get; }
        void Invalidate();
        void Focus();
        void Blur();
        bool HasFocus { get; }
        bool IsVisible { get; }
        void Show();
        void Hide();
    }
}