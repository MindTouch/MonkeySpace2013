using System;

namespace MindTouch.TextUI {
    public interface IPanel {
        int Top { get; set; }
        int Left { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        bool HasFocus { get; }
        bool IsVisible { get; }
        IPanelHost Parent { get; set; }
        IEnvironment Environment { get; set; }
        void Focus();
        void Blur();
        void Show();
        void Hide();
        void Invalidate();
        char[][] GetCanvas();
        event EventHandler PanelChanged;
    }
}