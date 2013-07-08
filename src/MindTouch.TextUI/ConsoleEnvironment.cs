using System;
using System.Drawing;

namespace MindTouch.TextUI {
    public class ConsoleEnvironment : Host, IEnvironment {

        private int _cursorLeft;
        private int _cursorTop;
        private IPanel _cursorPanel;
        private readonly Screen _screen;

        public ConsoleEnvironment() {
            _screen = new Screen();
        }

        //protected override void Update() {
        //    lock(_syncroot) {
        //        Console.CursorVisible = false;
        //        if(_canvas.Width != Width || _canvas.Height != Height) {
        //            _canvas.Resize(Width, Height);
        //        }
        //        foreach(var panel in _panels) {
        //            panel.Paint(_canvas, 0, 0, Width, Height);
        //        }
        //        for(var row = 0; row < Height; row++) {
        //            var line = _canvas.GetLine(row);
        //            if(!line.Changed) {
        //                continue;
        //            }
        //            Console.SetCursorPosition(line.Left, row);
        //            if(row == Height - 1 && (line.Left + line.Width) == Width) {

        //                // this is the last console line and it goes all the way to the left edge. Time for
        //                // some hackery to avoid the cursor pushing the buffer up by a line
        //                var data = line.GetData();
        //                Console.Write(data.Last());
        //                Console.MoveBufferArea(line.Left, row, 1, 1, Console.WindowWidth - 1, row);
        //                Console.SetCursorPosition(line.Left, row);
        //                Console.Write(data.Take(data.Length - 1).ToArray());
        //            } else {
        //                Console.Write(line.GetData());
        //            }
        //        }
        //        ShowCursor();
        //    }
        //}

        protected int GetWidth() {
            return Console.WindowWidth;
        }

        protected int GetHeight() {
            return Console.WindowHeight;
        }

        public void ShowCursor(IPanel panel, int left, int top) {
            if(panel.HasFocus) {
                _cursorLeft = left;
                _cursorTop = top;
                _cursorPanel = panel;
            }
            ShowCursor();
        }

        public void ShowCursor(IPanel panel, Point position) {
            throw new NotImplementedException();
        }

        public void HideCursor(IPanel panel) {
            if(_cursorPanel == panel) {
                _cursorPanel = null;
            }
            Console.CursorVisible = false;
        }

        public Point ToGlobal(IPanel panel, Point point) {
            throw new NotImplementedException();
        }

        public Rectangle ToGlobal(IPanel panel, Point point, Size size) {
            throw new NotImplementedException();
        }

        public void Paint(Point position, char[] line, int start, int length) {
            throw new NotImplementedException();
        }

        private void ShowCursor() {
            if(!CursorPanelHasFocus(_cursorPanel)) {
                Console.CursorVisible = false;
                return;
            }
            Console.CursorLeft = _cursorLeft;
            Console.CursorTop = _cursorTop;
            Console.CursorVisible = true;

        }

        private bool CursorPanelHasFocus(IPanel panel) {
            if(!panel.HasFocus) {
                return false;
            }
            if(panel.Parent != null) {
                return CursorPanelHasFocus(panel.Parent);
            }
            return panel.Environment == this;
        }

        protected override void Update() {
            throw new NotImplementedException();
        }
    }
}