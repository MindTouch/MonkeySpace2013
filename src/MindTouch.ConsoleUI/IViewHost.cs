using System;

namespace MindTouch.ConsoleUI {
    public interface IViewHost {
        void Draw(IPane pane);
        void Focus(IPane view);
        void Blur(IPane view);
        IPane Focused { get; }
        ConsoleKeyInfo ReadKey();
        event ConsoleCancelEventHandler CancelKeyPress;
        void AddView(IPane view);
        void Invalidate();
    }
}