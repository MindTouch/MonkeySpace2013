using System;

namespace MindTouch.TextUI {
    public class Panel : Host, IPanelHost {
        protected override void Update() {
            throw new NotImplementedException();
        }

        public override event EventHandler DimensionsChanged;

        public int Top { get; set; }
        public int Left { get; set; }
        public new int Width { get; set; }
        public new int Height { get; set; }

        protected override int GetWidth() {
            return Width;
        }

        protected override int GetHeight() {
            return Height;
        }

        public bool HasFocus { get; private set; }
        public bool IsVisible { get; private set; }
        public IPanelHost Parent { get; set; }
        public IEnvironment Environment { get; set; }
        public void Focus() {
            throw new NotImplementedException();
        }

        public void Blur() {
            throw new NotImplementedException();
        }

        public void Show() {
            throw new NotImplementedException();
        }

        public void Hide() {
            throw new NotImplementedException();
        }

        public void Invalidate() {
            throw new NotImplementedException();
        }

        public char[][] GetCanvas() {
            throw new NotImplementedException();
        }

        public event EventHandler PanelChanged;
        public int InnerWidth { get; private set; }
        public int InnerHeight { get; private set; }
    }
}