using System;
using System.Collections.Generic;
using System.Linq;

namespace MindTouch.ConsoleUI {
    public class FramedView : View {
        private int _left;
        private int _top;
        private int _width;
        private int _height;
        private string _title;

        public FramedView(IViewHost viewHost, int left, int top, int width, int height, ILog logger = null)
            : base(viewHost, left + 1, top + 1, width - 2, height - 2, logger) {
            _left = left;
            _top = top;
            _width = width;
            _height = height;
            if(_width <= 2 || _height <= 2) {
                Hide();
            }
        }

        public override int Top {
            get { return _top; }
            set {
                _top = value;
                Invalidate();
            }
        }
        public override int Left {
            get { return _left; }
            set {
                _left = value;
                Invalidate();
            }
        }
        public override int Width {
            get { return _width; }
            set {
                _width = value;
                base.Width = _width - 2;
            }
        }
        public override int Height {
            get { return _height; }
            set {
                _height = value;
                base.Height = _height - 2;
            }
        }

        public override int CursorLeft { get { return base.CursorLeft + 1; } }
        public override int CursorTop { get { return base.CursorTop + 1; } }

        public string Title {
            get { return _title; }
            set {
                _title = value;
                Invalidate();
            }
        }

        public override IEnumerable<char[]> VisibleBuffer {
            get {
                var inside = base.VisibleBuffer.ToArray();
                var buffer = new char[_height][];
                if(string.IsNullOrEmpty(_title) || _width < 8) {
                    buffer[0] = new[] { '┌' }.Concat(Enumerable.Repeat('─', _width - 2)).Concat(new[] { '┐' }).ToArray();
                } else {
                    var line = buffer[0] = new char[_width];
                    line[0] = '┌';
                    line[1] = '─';
                    line[2] = '[';
                    var titleWidth = Math.Min(_title.Length, _width - 6);
                    Array.Copy(_title.Substring(0, titleWidth).ToCharArray(), 0, line, 3, titleWidth);
                    line[titleWidth + 3] = ']';
                    var tailWidth = _width - 6 - titleWidth;
                    if(titleWidth > 0) {
                        Array.Copy(Enumerable.Repeat('─', tailWidth).ToArray(), 0, line, titleWidth + 4, tailWidth);
                    }
                    line[_width - 2] = '─';
                    line[_width - 1] = '┐';
                }
                buffer[_height - 1] = new[] { '└' }.Concat(Enumerable.Repeat('─', _width - 2)).Concat(new[] { '┘' }).ToArray();
                for(var i = 0; i < _height - 2; i++) {
                    var line = buffer[i + 1] = new char[_width];
                    line[0] = '│';
                    line[_width - 1] = '│';
                    Array.Copy(inside[i], 0, line, 1, inside[i].Length);
                }
                return buffer;
            }
        }
    }
}