using System.Collections.Generic;

namespace MindTouch.ConsoleUI {
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