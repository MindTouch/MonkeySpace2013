using System;

namespace MindTouch.TextUI {
    public interface IPanel {
        int Top { get; }
        int Left { get; }
        int Width { get; }
        int Height { get; }
        bool HasFocus { get; }
        bool IsVisible { get; }
        IPanelHost Parent { get; set; }
        IEnvironment Environment { get; set; }
        void Resize(int width, int height);
        void Move(int left, int top);
        void Focus();
        void Blur();
        void Show();
        void Hide();
        void Invalidate();
        void Paint(ICanvas canvas, int left, int top, int width, int height);
        event EventHandler PanelChanged;
    }
}