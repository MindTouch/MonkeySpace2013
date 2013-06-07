namespace MindTouch.TextUI {
    public interface IPanelHost : IPanel, IHost {
        int InnerWidth { get; }
        int InnerHeight { get; }
    }
}