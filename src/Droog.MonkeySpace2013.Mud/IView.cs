using System;

namespace Droog.MonkeySpace2013.Mud {
    public interface IView {
        void WriteLine(string format, params object[] args);
        void WriteLine();
        event ConsoleCancelEventHandler CancelKeyPress;
        void SetCursorPosition(int left, int top);
        ConsoleKeyInfo ReadKey(bool intercept);
        void Clear();
        void Write(string format, params object[] args);
        void Write(object value);
        int WindowWidth { get; }
        int BufferHeight { get; }
        int CursorTop { get; }
    }
}