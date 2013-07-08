using System;
using System.Drawing;

namespace MindTouch.TextUI {
    public interface IPanel {
        Point Position { get; }
        Size Size { get; }
        bool HasFocus { get; }
        bool IsVisible { get; }
        IPanelHost Parent { get; set; }
        IEnvironment Environment { get; set; }
        void Resize(Size size);
        void Move(Point position);
        void Focus();
        void Blur();
        void Show();
        void Hide();
        void Invalidate();
        void Paint(IEnvironment environment, Rectangle rect);
        event EventHandler<InvalidationArgs> PanelChanged;
    }

    public class InvalidationArgs : EventArgs {
        public readonly Rectangle Rect;
        public InvalidationArgs(Rectangle rect) {
            Rect = rect;
        }
    }
}