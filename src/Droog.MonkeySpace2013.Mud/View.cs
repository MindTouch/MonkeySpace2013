using System;
using System.Collections.Generic;
using System.Linq;

namespace Droog.MonkeySpace2013.Mud {
    public class View : IView, ILog {
        private readonly int _top;
        private readonly int _left;
        private readonly int _width;
        private readonly int _height;
        private readonly ILog _logger;
        private int _cursorTop;
        private int _cursorLeft;
        private int _bufferTop;
        private List<char[]> _buffer;

        public View(int left, int top, int width, int height, ILog logger = null) {
            _top = top;
            _left = left;
            _width = width;
            _height = height;
            _logger = logger ?? NullLog.Instance;
            InitBuffer();
        }

        private void InitBuffer() {
            _buffer = new List<char[]>();
            for(int i = 0; i < _height; i++) {
                AddBufferLine();
            }
            Blit();
        }

        private void AddBufferLine() {
            var line = Enumerable.Repeat(' ', _width).ToArray();
            _buffer.Add(line);
        }

        private void Blit() {
            lock(Console.Out) {
                var top = _top;
                foreach(var line in _buffer.Skip(_bufferTop).Take(_height)) {
                    Console.SetCursorPosition(_left, top);
                    Console.Write(line);
                    top++;
                }
                Console.SetCursorPosition(_left + _cursorLeft, _top + _cursorTop);
            }
            _logger.Debug("Blit: [{0},{1}]", _cursorLeft, _cursorTop);
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
            Blit();
        }

        public void WriteLine() {
            SetText("\r\n");
        }

        public event ConsoleCancelEventHandler CancelKeyPress {
            add { Console.CancelKeyPress += value; }
            remove { Console.CancelKeyPress -= value; }
        }

        public void SetCursorPosition(int left, int top) {
            _cursorLeft = left;
            _cursorTop = top;
            _logger.Debug("Set: [{0},{1}]", _cursorLeft, _cursorTop);
        }

        public ConsoleKeyInfo ReadKey(bool intercept) {
            return Console.ReadKey(intercept);
        }

        public void Clear() {
            InitBuffer();
            _cursorLeft = 0;
            _cursorTop = 0;
        }

        public void Write(string format, params object[] args) {
            SetText(string.Format(format, args));
        }

        public void Write(object value) {
            SetText(value.ToString());
        }

        public int WindowWidth {
            get { return _width; }
        }

        public int BufferHeight {
            get { return _height; }
        }

        public int CursorTop {
            get { return _cursorTop; }
        }

        void ILog.Debug(string format, params object[] args) {
            WriteLine("DEBUG: " + format, args);
        }

        void ILog.Error(string format, params object[] args) {
            WriteLine("ERROR: " + format, args);
        }
    }
}