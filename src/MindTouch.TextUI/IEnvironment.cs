using System.Drawing;

namespace MindTouch.TextUI {
    public interface IEnvironment : IHost {
        void ShowCursor(IPanel panel, Point position);
        void HideCursor(IPanel panel);
        Point ToGlobal(IPanel panel, Point point);
        Rectangle ToGlobal(IPanel panel, Point point, Size size);
        void Paint(Point position, char[] line, int start, int length);
    }
}