namespace MindTouch.TextUI {
    public interface IEnvironment : IHost {
        void ShowCursor(IPanel panel, int left, int top);
        void HideCursor(IPanel panel);
    }
}