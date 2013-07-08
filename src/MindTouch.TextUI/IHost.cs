using System;
using System.Collections.Generic;
using System.Drawing;

namespace MindTouch.TextUI {
    public interface IHost {
        Size Size { get; }
        IEnumerable<IPanel> Children { get; }
        void AddChild(IPanel panel);
        void RemoveChild(IPanel panel);
        event EventHandler DimensionsChanged;
    }
}