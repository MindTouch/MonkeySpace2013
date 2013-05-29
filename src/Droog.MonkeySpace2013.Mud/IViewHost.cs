using System;
using System.Collections.Generic;

namespace Droog.MonkeySpace2013.Mud {
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