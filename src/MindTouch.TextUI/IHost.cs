using System;
using System.Collections.Generic;

namespace MindTouch.TextUI {
    public interface IHost {
        int Width { get; }
        int Height { get; }
        IEnumerable<IPanel> Children { get; }
        void AddChild(IPanel panel);
        void RemoveChild(IPanel panel);
        event EventHandler DimensionsChanged;
    }
}