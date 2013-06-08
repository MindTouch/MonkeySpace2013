using System;
using System.Linq;

namespace MindTouch.TextUI {
    public class Line {

        private char[] _data;

        public Line(int width) {
            Width = width;
            Changed = true;
            _data = Enumerable.Repeat(' ', width).ToArray();
        }

        public bool Changed { get; private set; }
        public int Left { get; private set; }
        public int Width { get; private set; }
        public char[] GetData() {
            var data = new char[Width];
            Array.Copy(_data, Left, data, 0, Width);
            return data;
        }

        public void Resize(int width) {
            var data = Enumerable.Repeat(' ', width).ToArray();
            Array.Copy(_data, data, Math.Min(width, Width));
            Width = width;
            _data = data;
        }

        public void Reset() {
            Left = 0;
            Width = _data.Length;
            Changed = false;
        }

        public void Write(int left, char[] line) {
            Array.Copy(line, 0, _data, left, line.Length);
            Left = Math.Min(Left, left);
            Width = Math.Max(Width, left + line.Length);
            Changed = true;
        }
    }
}