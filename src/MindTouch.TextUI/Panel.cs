using System;

namespace MindTouch.TextUI {
    public class Panel : Host, IPanelHost {

        private bool _hasFocus;
        private bool _isVisible = true;

        public int Top { get; private set; }

        public int Left { get; private set; }
        public new int Width { get; private set; }
        public new int Height { get; private set; }
        public bool HasFocus { get { return _hasFocus && (Parent == null || Parent.HasFocus); } }
        public bool IsVisible { get { return _isVisible && (Parent == null || Parent.IsVisible); } }
        public IPanelHost Parent { get; set; }
        public IEnvironment Environment { get; set; }
        public virtual int InnerWidth { get { return _width; } }
        public virtual int InnerHeight { get { return _height; } }

        protected override void Update() {
            OnPanelChanged();
        }

        protected override int GetWidth() {
            return Width;
        }

        protected override int GetHeight() {
            return Height;
        }

        public void Focus() {
            _hasFocus = true;
            Update();
        }

        public void Blur() {
            _hasFocus = false;
            Update();
        }

        public void Show() {
            _isVisible = true;
            Update();
        }

        public void Hide() {
            _isVisible = false;
            Update();
        }

        public void Invalidate() {
            Update();
        }

        public Canvas GetCanvas(int left, int top, int width, int height) {
            if(IsVisible && Left < left+width && Left+Width > left && Top < top+height && Top+Height > top) {
                return BuildCanvas(left, top, Width, Height);
            }
            return Canvas.Empty;
        }

        public event EventHandler PanelChanged;

        protected void OnPanelChanged() {
            if(PanelChanged != null) {
                PanelChanged(this, EventArgs.Empty);
            }
        }
    }
}