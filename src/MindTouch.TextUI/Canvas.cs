namespace MindTouch.TextUI {
    public interface ICanvas {
        void Write(int left, int top, char[] line);
    }

    public class Canvas : ICanvas {
        private readonly Line[] _lines;

        public Canvas(int width, int height) {
            Width = width;
            Height = height;
            _lines = new Line[height];
            for(var i = 0; i < height; i++) {
                _lines[i] = new Line(width);
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }


        public void Write(int left, int top, char[] line) {
            _lines[top].Write(left, line);
        }

        public Line GetLine(int top) {
            return _lines[top];
        }

        public void Resize(int width, int height) {
            var lines = new Line[height];
            for(var i = 0; i < height; i++) {
                if(i < Height) {
                    lines[i] = _lines[i];
                    lines[i].Resize(width);
                } else {
                    lines[i] = new Line(width);
                }
            }
        }
        public void Reset() {
            foreach(var line in _lines) {
                line.Reset();
            }
        }
    }
}