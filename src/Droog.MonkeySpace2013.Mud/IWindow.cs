using System;

namespace Droog.MonkeySpace2013.Mud {
    public interface IWindow {
        void WriteLine(string format, params object[] args);
        void WriteLine();
        event ConsoleCancelEventHandler CancelKeyPress;
        void SetCursorPosition(int col, int row);
        ConsoleKeyInfo ReadKey(bool intercept);
        void Clear();
        void Write(string format, params object[] args);
        void Write(object value);
        int WindowWidth { get; set; }
        int BufferHeight { get; set; }
        int CursorTop { get; set; }
    }
}