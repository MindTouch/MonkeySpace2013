using System;
using System.Drawing;

namespace MindTouch.TextUI {
    public class Panel : Host, IPanelHost {

        private bool _hasFocus;
        private bool _isVisible = true;

        public Point Position { get; protected set; }
        public virtual Size InnerSize { get { return Size; } }
        public bool HasFocus { get { return _hasFocus && (Parent == null || Parent.HasFocus); } }
        public bool IsVisible { get { return _isVisible && (Parent == null || Parent.IsVisible); } }
        public IPanelHost Parent { get; set; }
        public IEnvironment Environment { get; set; }

        protected override void Update() {
            //OnPanelChanged(true);
        }

        public void Resize(Size size) {
            Size = size;
            var invalidated = Environment.ToGlobal(this, Position, Size);
            OnPanelChanged(invalidated);
            OnDimensionsChanged();
        }

        public void Move(Point position) {
            var previous = Environment.ToGlobal(this, Position, Size);
            Position = position;
            var current = Environment.ToGlobal(this, Position, Size);
            var invalidated = Rectangle.Union(previous, current);
            OnPanelChanged(invalidated);
        }

        public void Focus() {
            _hasFocus = true;
            var invalidated = Environment.ToGlobal(this, Position, Size);

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

        public void Paint(IEnvironment environment, Rectangle rect) {
            throw new NotImplementedException();
        }

        //public void Paint(ICanvas canvas, int left, int top, int width, int height) {
        //    var global = Environment.ToGlobal(this, new Point(Left, Top));
        //    if(!IsVisible || global.Left >= left + width || global.Left + Width <= left || global.Top >= top + height || global.Top + Height <= top) {
        //        return;
        //    }
        //    var boundTop = Math.Max(top, global.Top);
        //    var boundBottom = Math.Min(top + height, global.Top + Height);
        //    var boundLeft = Math.Max(left, global.Left);
        //    var boundRight = Math.Min(left + Width, global.Left + Width);
        //    for(var row = boundTop; row < boundBottom; row++) {

        //    }
        //    //    var buffer = new char[height][];
        //    //    for(var i = 0; i < buffer.Length; i++) {
        //    //        buffer[i] = new char[width];
        //    //    }
        //    //    var panels = from p in _panels
        //    //                 let c = p.GetCanvas(left, top, width, height)
        //    //                 where c != Canvas.Empty
        //    //                 select c;
        //    //    foreach(var panelCanvas in panels) {
        //    //        panelCanvas.CopyTo(buffer);
        //    //    }
        //    //    return new Canvas(0, 0, width, height, buffer);
        //}

        protected void OnPanelChanged(Rectangle invalidated) {
            if(PanelChanged != null) {
                PanelChanged(this, new InvalidationArgs(invalidated));
            }
        }

        public event EventHandler<InvalidationArgs> PanelChanged;


    }
}