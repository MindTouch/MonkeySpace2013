using System;
using System.Collections.Generic;
using System.Linq;

namespace MindTouch.ConsoleUI {
    public class View : IView, IPane, ILog {
        protected readonly IViewHost _viewHost;
        protected int _top;
        protected int _left;
        protected int _width;
        protected int _height;
        protected readonly ILog _logger;
        private int _cursorTop;
        private int _cursorLeft;
        private int _bufferTop;
        private List<char[]> _buffer;
        private bool _isVisible = true;

        public View(IViewHost viewHost, int left, int top, int width, int height, ILog logger = null) {
            _viewHost = viewHost;
            _top = top;
            _left = left;
            _width = width;
            _height = height;
            _logger = logger ?? NullLog.Instance;
            InitBuffer();
            viewHost.AddView(this);
        }

        private void InitBuffer() {
            _buffer = new List<char[]>();
            for(var i = 0; i < _height; i++) {
                AddBufferLine();
            }
        }

        protected void EnsureBuffer() {
            for(var i = 0; i < _buffer.Count; i++) {
                var line = _buffer[i];
                if(line.Length != _width) {
                    _buffer[i] = GetBufferLine();
                    Array.Copy(line, _buffer[i], Math.Min(line.Length, _width));
                }
            }
            if(_buffer.Count < _height) {
                var n = _height - _buffer.Count;
                for(var i = 0; i < n; i++) {
                    _buffer.Insert(0, GetBufferLine());
                }
            }
        }

        private void AddBufferLine() {
            _buffer.Add(GetBufferLine());
        }

        private char[] GetBufferLine() {
            return Enumerable.Repeat(' ', _width).ToArray();
        }

        public void Invalidate() {
            if(!_isVisible) {
                return;
            }
            _viewHost.Draw(this);
        }

        public void Focus() {
            _viewHost.Focus(this);
        }

        public void Blur() {
            _viewHost.Blur(this);
        }

        public bool HasFocus { get { return _viewHost.Focused == this; } }

        public bool IsVisible {
            get { return _isVisible; }
        }

        public void Show() {
            _isVisible = true;
            Invalidate();
        }

        public void Hide() {
            _isVisible = false;
            _viewHost.Invalidate();
        }

        public void WriteLine(string format, params object[] args) {
            SetText(string.Format(format + "\r\n", args));
        }

        private void SetText(string line) {
            SetText(line.Split(new[] { "\r\n" }, StringSplitOptions.None));
        }

        private void SetText(string[] lines) {
            var lineIdx = 0;
            var line = lines[lineIdx].ToCharArray();
            while(true) {
                var row = _buffer[_bufferTop + _cursorTop];
                Array.Copy(line, 0, row, _cursorLeft, Math.Min(line.Length, _width - _cursorLeft));
                _cursorLeft += line.Length;
                var over = _cursorLeft - _width;
                if(over <= 0) {

                    // didn't go past the end of the line
                    lineIdx++;
                    if(lineIdx == lines.Length) {
                        break;
                    }
                    line = lines[lineIdx].ToCharArray();
                }
                _cursorTop++;
                _cursorLeft = 0;
                if(_cursorTop == _height) {
                    _cursorTop--;
                    AddBufferLine();
                    _bufferTop++;
                }
                if(over <= 0) {
                    continue;
                }
                var rest = new char[over];
                Array.Copy(line, line.Length - over, rest, 0, over);
                line = rest;
            }
            Invalidate();
        }

        public void WriteLine() {
            SetText("\r\n");
        }

        public event ConsoleCancelEventHandler CancelKeyPress {
            add { _viewHost.CancelKeyPress += value; }
            remove { _viewHost.CancelKeyPress -= value; }
        }

        public void SetCursorPosition(int left, int top) {
            _cursorLeft = left;
            _cursorTop = top;
        }

        public ConsoleKeyInfo ReadKey() {
            return _viewHost.ReadKey();
        }

        public void Clear() {
            InitBuffer();
            _cursorLeft = 0;
            _cursorTop = 0;
            Invalidate();
        }

        public void Write(string format, params object[] args) {
            SetText(string.Format(format, args));
        }

        public void Write(object value) {
            SetText(value.ToString());
        }

        public virtual int Top {
            get { return _top; }
            set {
                _top = value;
                _viewHost.Invalidate();
            }
        }

        public virtual int Left {
            get { return _left; }
            set {
                _left = value;
                _viewHost.Invalidate();
            }
        }

        public virtual int Width {
            get { return _width; }
            set {
                _width = value;
                EnsureBuffer();
                _viewHost.Invalidate();
            }
        }
        int IView.Width { get { return _width; } }

        public virtual int Height {
            get { return _height; }
            set {
                _height = value;
                EnsureBuffer();
                _viewHost.Invalidate();
            }
        }
        int IView.Height { get { return _height; } }

        public virtual int CursorTop { get { return _cursorTop; } }
        int IView.CursorTop { get { return _cursorTop; } }

        public virtual int CursorLeft { get { return _cursorLeft; } }
        int IView.CursorLeft { get { return _cursorLeft; } }

        public virtual IEnumerable<char[]> VisibleBuffer {
            get { return _buffer.Skip(_bufferTop).Take(_height); }
        }

        void ILog.Debug(string format, params object[] args) {
            WriteLine("DEBUG: " + format, args);
        }

        void ILog.Error(string format, params object[] args) {
            WriteLine("ERROR: " + format, args);
        }
    }
}