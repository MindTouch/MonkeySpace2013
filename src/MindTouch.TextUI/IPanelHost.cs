using System.Drawing;

namespace MindTouch.TextUI {
    public interface IPanelHost : IPanel, IHost {
        Size InnerSize { get; }
    }
}